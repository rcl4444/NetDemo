using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOPoco.Domain;
using Core;
using System.Linq.Expressions;
using Repository.Interface;
using System.Data.SqlClient;
using AEOPoco.Other;

namespace AEOService.Services
{
    public class FileRequireService : BaseService<FileRequire>,IFileRequireService
    {
        protected readonly IFileResultService _fileResultService;
        protected readonly IFileOperationNoteService _fileOperationNoteService;

        public FileRequireService(
            IRepository<FileRequire> selfRepository,
            IFileResultService fileResultService,
            IFileOperationNoteService fileOperationNoteService)
            : base(selfRepository)
        {
            this._fileResultService = fileResultService;
            this._fileOperationNoteService = fileOperationNoteService;
        }

        public IEnumerable<FileAuditResult> FileAuditSearch(int companyID, int reviewerID, bool isManager)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string sql = @"select Id,
FinishTime,--预计完成
ClausesName,--条
ItemName,--项
FineItemName,--细项内容
[Description],--文件要求
PersonName--主办人
from
(
	select
	frq.Id,
	fs.FinishTime,--预计完成
	c.ClausesName,--条
	i.ItemName,--项
	fi.FineItemName,--细项内容
	frq.[Description],--文件要求
	ca.PersonName,--主办人
	fr.[Status],
	rn=row_number()over(partition by fr.FileRequireID order by fr.CreateTime desc,fr.id desc)
	from FileRequire frq inner join FileSchedule fs on frq.Id = fs.Id
	inner join CustomerAccount ca on fs.AuditorID = ca.Id
	inner join FineItem fi on frq.FineItemID = fi.Id
	inner join Item i on i.Id = fi.ItemID
	inner join Clauses c on i.ClausesID = c.Id
	inner join FileResult fr on fr.FileRequireID = frq.Id
	where frq.CustomerCompanyID = @companyID and fr.status != @cancel {0}
) tb where rn = 1 and [Status] = @status";
            if (!isManager)
            {
                sql = string.Format(sql, "and (fs.AuditorID = @auditorID or exists (select 1 from ClausesPersonLiable where CustomerCompanyID = @companyID and CustomerAccountID = @auditorID and ClausesID = c.Id))");
                sqlParams.Add(new SqlParameter { ParameterName = "@auditorID", Value = reviewerID });
            }
            else
            {
                sql = string.Format(sql, "");
            }
            sqlParams.Add(new SqlParameter { ParameterName = "@companyID", Value = companyID });
            sqlParams.Add(new SqlParameter { ParameterName = "@status", Value = FileStatus.ApplyAudit });
            sqlParams.Add(new SqlParameter { ParameterName = "@cancel", Value = FileStatus.Cancel });
            return this.SqlQuery<FileAuditResult>(sql, sqlParams.ToArray());
        }

        public bool DeleteFileRequire(int companyID, int FileRequireID , out string message)
        {
            var FileRequire = this.NoTrackingQuery.Where(o => o.CustomerCompanyID.Equals(companyID) && o.Id.Equals(FileRequireID)).FirstOrDefault();
            if (FileRequire == null)
            {
                message = "该细项无文件要求";
                return false;
            }
            else
            {

                var list = this._fileResultService.NoTrackingQuery.Where(o => o.FileRequireID == FileRequire.Id).Select(o =>o.PhysicalFullPath).ToList();
                using (var tran = this.BeginTransaction())
                {
                    try
                    {
                        foreach (var path in list)
                        {
                            DeleteFile(path, out message);
                        }
                        this.ExecuteSqlCommand(@"delete from UserRole from UserRole t1 inner join FileSchedule t2 on t1.RoleID = @chargeRole and t1.CustomerAccountID = t2.ChargePersonID
where not exists(select 1 from FileSchedule where ChargePersonID = t1.CustomerAccountID and Id != t2.Id) and t2.Id = @id;
delete from UserRole from UserRole t1 inner join FileSchedule t2 on t1.RoleID = @chargeRole and t1.CustomerAccountID = t2.ChargePersonID
where not exists(select 1 from FileSchedule where ChargePersonID = t1.CustomerAccountID and Id != t2.Id) and t2.Id = @id;
delete from OperationNote where FileRequireID = @id;
delete from FileResult where FileRequireID = @id;
delete from FileSchedule where id = @id;
delete from FileRequire where id = @id;", 
                                new SqlParameter { ParameterName = "@id", Value = FileRequireID }, 
                                new SqlParameter { ParameterName = "@chargeRole", Value = (int)Role.Charge },
                                new SqlParameter { ParameterName = "@reviewerRole", Value = (int)Role.Reviewer });
                        tran.Commit();
                        message = "删除成功";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        message = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool AddFileRequire(int companyID, int FindItemID,string SuggestFileName, string Description, out string message)
        {
            try
            {
                var FileRequire =new FileRequire{
                    CustomerCompanyID = companyID,
                    FineItemID = FindItemID,
                    SuggestFileName = SuggestFileName,
                    Description = Description,
                    CreateTime = DateTime.Now
                };
                this.Add(FileRequire);
                message = "新增成功";
                return true;
            }
            catch (Exception)
            {
                message = "新增失败";
                return false;
            }
        }

        public void DeleteFile(string PhysicalFullPath, out string message)
        {
            var ServerPath = System.Web.HttpContext.Current.Server.MapPath(PhysicalFullPath);
            if (System.IO.File.Exists(ServerPath))
            {
                System.IO.File.Delete(ServerPath);
                message = "文件删除成功";
            }
            else
            {
                message = "文件不存在";
            }
        }


        public bool UpdateFileRequire(int companyID, int FileRequireID, string SuggestFileName, string Description, out string message)
        {
            try
            {
                var FileRequire = this.Query.Where(o => o.CustomerCompanyID == companyID && o.Id == FileRequireID).FirstOrDefault();
                FileRequire.Description = Description;
                FileRequire.SuggestFileName = SuggestFileName;
                this.Update(FileRequire);
                message = "更新成功";
                return true;
            }
            catch (Exception)
            {
                message = "更新失败";
                return false;
            }
        }

        public bool SortFileRequire(int companyID,int oldFileRequireID, int newFileRequire, out string message)
        {
            try
            {
                using (var tran = this.BeginTransaction())
                {
                    var oldId = this.Query.Where(o => o.CustomerCompanyID == companyID && o.Id == oldFileRequireID).FirstOrDefault();
                    var newId = this.Query.Where(o => o.CustomerCompanyID == companyID && o.Id == newFileRequire).FirstOrDefault();

                    var description = oldId.Description;
                    var suggestFileName = oldId.SuggestFileName;

                    oldId.Description = newId.Description;
                    oldId.SuggestFileName = newId.SuggestFileName;

                    newId.Description = description;
                    newId.SuggestFileName = suggestFileName;
                    this.Update(oldId);
                    this.Update(newId);
                    tran.Commit();
                    message = "排序成功";
                }
                return true;
            }
            catch (Exception)
            {
                message = "排序失败";
                return false;
            }
        }

        public bool UploadFile(int fileRequireID, CustomerAccount currentAccount, string uploadFileName, string contentType, Func<string, string, IFileManager.FileResult> fileSave, out string message)
        {
            var fileRequire = this.GetByID(fileRequireID);
            string physicalFileName = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(uploadFileName));
            string physicalContents = string.Format("/document/{0}/{1}/{2}/{3}/{4}/",
                currentAccount.CustomerCompany.CompanyName,
                fileRequire.FineItem.Item.Clauses.Id,
                fileRequire.FineItem.Item.Id,
                fileRequire.FineItem.Id,
                fileRequire.Id);
            if (fileRequire != null)
            {
                if (this._fileResultService.GetEffectiveRecord(fileRequireID,currentAccount.CustomerCompanyID).Status == FileStatus.Audit)
                {
                    message = "该文件要求已审核完毕,不可上传";
                    return false;
                }
                if (fileRequire.FileSchedule.ChargePersonID != currentAccount.Id)
                {
                    message = "只有主办人才可上传文件";
                    return false;
                }
                IFileManager.FileResult file = fileSave(physicalContents, physicalFileName);
                using (var tran = this.BeginTransaction())
                {
                    try
                    {
                        this._fileResultService.Add(new AEOPoco.Domain.FileResult()
                        {
                            FileRequire = fileRequire,
                            FileName = uploadFileName,
                            //PhysicalFullPath = physicalContents + physicalFileName,
                            PhysicalFullPath = file.SubDir +"/" + file.Key,
                            UploadTime = DateTime.Now,
                            Status = FileStatus.Upload,
                            UploadPerson = currentAccount,
                            ContentType = contentType,
                            CreateTime = DateTime.Now
                        });
                        this._fileOperationNoteService.Add(new FileOperationNote()
                        {
                            FileRequire = fileRequire,
                            Operationer = currentAccount,
                            Description = string.Format("上传文件{0}", uploadFileName),
                            CreateTime = DateTime.Now
                        });
                        tran.Commit();
                        message = "";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        tran.Rollback();
                        return false;
                    }
                }
            }
            else
            {
                message = "不存在该文件要求";
                return false;
            }
        }
    }
}
