using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;
using System.Data.SqlClient;
using AEOPoco.Other;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace AEOService.Services
{
    public class FileResultService : BaseService<FileResult>, IFileResultService
    {
        private readonly IFileOperationNoteService _fileOperationNoteService;
        private readonly IFileManager.IFileManager _fileManager;

        public FileResultService(IRepository<FileResult> selfRepository,
            IFileOperationNoteService fileOperationNoteService,
            IFileManager.IFileManager fileManager)
            : base(selfRepository)
        {
            this._fileOperationNoteService = fileOperationNoteService;
            this._fileManager = fileManager;
        }

        private string GetFileStatusDescription(FileStatus fs)
        {
            string description;
            switch (fs)
            {
                case FileStatus.Reject:
                    description = "否决";
                    break;
                case FileStatus.Upload:
                    description = "上传";
                    break;
                case FileStatus.ApplyAudit:
                    description = "申请审核";
                    break;
                case FileStatus.Audit:
                    description = "审核通过";
                    break;
                default:
                    description = "其他";
                    break;
            }
            return description;
        }

        private bool StatusChange(
            List<int> ids, 
            CustomerAccount currentAccount,
            List<FileStatus> constraintStatus, 
            Action<FileResult> voluation, 
            out string message,string operationFormat = "文件:{0},{1}",
            Func<FileResult,string> appendValidate = null,
            Func<List<FileResult>,IEnumerable<FileResult>,List<FileStatus>,string> statusUnMatch = null)
        {
            if (ids == null || ids.Count == 0)
            {
                message = "请选择";
                return false;
            }
            IEnumerable<FileResult> query = this.Query.Where(o => ids.Contains(o.Id));
            var list = query.ToList();
            if (ids.Count != list.Count)
            {
                message = string.Format("不存在", string.Join(",", ids.Where(o => !list.Select(oi => oi.Id).Contains(o))));
                return false;
            }
            var errorStatusli = list.Where(o => !constraintStatus.Contains(o.Status));
            if (errorStatusli.Count()>0)
            {
                if (statusUnMatch == null)
                {
                    message = string.Format("状态不是{1}", string.Join(",", errorStatusli.Select(o => o.Id)), string.Join(",", constraintStatus.Select(o => GetFileStatusDescription(o))));
                }
                else
                {
                    message = statusUnMatch(list, errorStatusli, constraintStatus);
                }
                return false;
            }
            if (appendValidate != null)
            {
                foreach (var item in list)
                {
                    message = appendValidate(item);
                    if (!string.IsNullOrEmpty(message))
                    {
                        return false;
                    }
                }
            }
            list.ForEach(voluation);
            using (var tran = this.BeginTransaction())
            {
                try
                {
                    this.Update(list);
                    this._fileOperationNoteService.Add(list.Select(o=> new FileOperationNote()
                    {
                        FileRequire = o.FileRequire,
                        Operationer = currentAccount,
                        Description = string.Format(operationFormat, o.FileName, GetFileStatusDescription(o.Status)),
                        CreateTime = DateTime.Now
                    }));
                    tran.Commit();
                    message = "成功执行";
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

        public IEnumerable<FileRequireResult> GetEffectiveRecords(int itemid,int companyId)
        {
            return this.SqlQuery<FileRequireResult>(@"select
frq.Id as FileRequireId,
fr.FileResultId,
i.ItemName,
fs.FinishTime,
fr.UploadTime,
frq.[Description],
fr.[Status],
fr.AuditTime,
fr.PhysicalFullPath,
frq.SuggestFileName,
ca.PersonName as AuditPerson
from Item i
inner join FineItem fi on i.Id = fi.ItemID
inner join FileRequire frq on fi.Id = frq.FineItemID
left join FileSchedule fs on frq.Id = fs.Id
left join CustomerAccount ca on fs.AuditorID = ca.Id
left join
(
	select
	frq.Id as FileRequireId,
	fr.Id as FileResultId,
	fr.UploadTime,
	fr.[Status],
	fr.AuditTime,
	fr.PhysicalFullPath,
	rn=row_number()over(partition by fr.FileRequireID order by fr.CreateTime desc,fr.id desc) 
	from FineItem fi
	inner join FileRequire frq on fi.Id = frq.FineItemID
	inner join FileResult fr on fr.FileRequireID = frq.Id
	where fi.ItemId = @itemid and fr.[Status]!= @cancel and frq.CustomerCompanyID = @companyId
) fr on frq.Id = fr.FileRequireId and fr.rn = 1
where i.Id = @itemid and frq.CustomerCompanyID = @companyId;", new SqlParameter { ParameterName = "@itemid", Value = itemid }, 
new SqlParameter { ParameterName = "@cancel", Value = FileStatus.Cancel },
new SqlParameter { ParameterName = "@companyId", Value = companyId });
        }

        public FileRequireResult GetEffectiveRecord(int fileRequireID, int companyId)
        {
            return this.SqlQuery<FileRequireResult>(@"select
frq.Id as FileRequireId,
fr.FileResultId,
i.ItemName,
fs.FinishTime,
fr.UploadTime,
frq.[Description],
fr.[Status],
fr.AuditTime,
fr.PhysicalFullPath,
frq.SuggestFileName,
ca.PersonName as AuditPerson
from Item i
inner join FineItem fi on i.Id = fi.ItemID
inner join FileRequire frq on fi.Id = frq.FineItemID
left join FileSchedule fs on frq.Id = fs.Id
left join CustomerAccount ca on fs.AuditorID = ca.Id
left join
(
	select
	frq.Id as FileRequireId,
	fr.Id as FileResultId,
	fr.UploadTime,
	fr.[Status],
	fr.AuditTime,
	fr.PhysicalFullPath,
	rn=row_number()over(partition by fr.FileRequireID order by fr.CreateTime desc,fr.id desc) 
	from FileRequire frq inner join FileResult fr on fr.FileRequireID = frq.Id
	where frq.Id = @fileRequireID and fr.[Status]!= @cancel and frq.CustomerCompanyID = @companyId
) fr on frq.Id = fr.FileRequireId and fr.rn = 1
where frq.Id = @fileRequireID and frq.CustomerCompanyID = @companyId;", new SqlParameter { ParameterName = "@fileRequireID", Value = fileRequireID },
new SqlParameter { ParameterName = "@cancel", Value = FileStatus.Cancel },
new SqlParameter { ParameterName = "@companyId", Value = companyId }).FirstOrDefault();
        }

        public IEnumerable<FileRequireStatus> GetFileRequireStatus(IEnumerable<int> fileRequireIDs)
        {
            return this.SqlQuery<FileRequireStatus>(@"select 
FileRequireId,
[Status],
AuditTime
from
(
	select
	fr.FileRequireId,
	fr.[Status],
	fr.AuditTime,
	rn=row_number()over(partition by fr.FileRequireID order by fr.CreateTime desc,fr.id desc) 
	from FileResult fr
	where fr.FileRequireID in ("+string.Join(",", fileRequireIDs) +@") and fr.[Status]!= @cancel
) tb where tb.rn =1",new SqlParameter { ParameterName = "@cancel", Value = FileStatus.Cancel }).ToList();
        }

        public bool SetApplyAudit(int fileResultID, CustomerAccount currentAccount, out string message)
        {
            return this.StatusChange(new List<int> { fileResultID },
                currentAccount,
                new List<FileStatus> { FileStatus.Upload },
                (o => {
                    o.Status = FileStatus.ApplyAudit;
                    o.ApplyAuditTime = DateTime.Now;
                    if (!o.FileRequire.FileSchedule.AuditorID.HasValue)
                    {
                        o.Status = FileStatus.Audit;
                        o.AuditTime = DateTime.Now;
                    }
                }),
                out message);
        }

        public bool SetAudit(int fileResultID, CustomerAccount currentAccount, out string message)
        {
            return this.StatusChange(new List<int> { fileResultID },
                currentAccount,
                new List<FileStatus> { FileStatus.ApplyAudit },
                (o => { o.Status = FileStatus.Audit; o.AuditTime = DateTime.Now; }),
                out message, 
                "文件:{0},{1}",
                (a => {
                    var lastResult = this.NoTrackingQuery.Where(o => o.FileRequireID == a.FileRequireID && o.Status != FileStatus.Cancel).OrderByDescending(o => o.CreateTime).FirstOrDefault();
                    if (lastResult == null || lastResult.Id != a.Id)
                    {
                        return "有新审核数据,请刷新界面";
                    }
                    if (a.FileRequire.FileSchedule.AuditorID != currentAccount.Id)
                    {
                        return "只有审核人才可进行审核操作";
                    }
                    return "";
                }),(a,b,c)=> {
                    return string.Join(",",a.Select(o=>o.Status).Select(o => GetFileStatusDescription(o)));
                });
        }

        public bool SetReject(int fileResultID, string rejectReason, CustomerAccount currentAccount, out string message)
        {
            return this.StatusChange(new List<int> { fileResultID },
                currentAccount,
                new List<FileStatus> { FileStatus.ApplyAudit },
                (o => { o.Status = FileStatus.Reject; o.ApplyAuditTime = DateTime.Now; }),
                out message, "文件:{0},{1}" + (string.IsNullOrEmpty(rejectReason) ? "" : string.Format(",拒绝原因:{0}", rejectReason)),
                (a => {
                    if (a.FileRequire.FileSchedule.AuditorID != currentAccount.Id)
                    {
                        return "只有审核人才可进行审核操作";
                    }
                    return "";
                }));
        }

        public bool SetCancel(int fileResultID, CustomerAccount currentAccount, out string message)
        {
            return this.StatusChange(new List<int> { fileResultID },
                currentAccount,
                new List<FileStatus> { FileStatus.Upload },
                (o => { o.Status = FileStatus.Cancel; o.AuditTime = DateTime.Now; }),
                out message);
        }

        public IEnumerable<FileSituation> GetFileSituation(int companyId)
        {
            return this.SqlQuery<FileSituation>(@"select
oc.Id as ClassId,
oc.OutlineClassName as ClassName,
c.Id as ClausesId,
c.ClausesName,
i.Id as ItemId,
i.ItemName,
fi.Id as FineItemId,
fi.FineItemName,
frq.Id as FileRequireId,
tb.[FileName],
tb.PhysicalFullPath
from dbo.CustomerCompany cc
inner join dbo.CustomsAuthentication ca on cc.CustomsAuthenticationID = ca.Id 
inner join dbo.OutlineClass oc on ca.Id = oc.CustomsAuthenticationID
inner join dbo.Clauses c on oc.Id = c.OutlineClassID
inner join dbo.Item i on c.Id = i.ClausesID
inner join dbo.FineItem fi on i.Id = fi.ItemID
inner join dbo.FileRequire frq on fi.Id = frq.FineItemID
inner join
(
	select
	fr.FileRequireId,
	fr.[Status],
	fr.[FileName],
	fr.PhysicalFullPath,
	rn=row_number()over(partition by fr.FileRequireID order by fr.CreateTime desc,fr.id desc)
	from FileResult fr inner join dbo.FileRequire frq on fr.FileRequireId = frq.Id
	where fr.[Status]!= @cancel and frq.CustomerCompanyID = @companyId
) tb on frq.Id = tb.FileRequireId and tb.rn = 1 and tb.[Status] = @audit
where cc.Id = @companyId;", new SqlParameter { ParameterName = "@companyId", Value = companyId },
new SqlParameter { ParameterName = "@audit", Value = FileStatus.Audit },
new SqlParameter { ParameterName = "@cancel", Value = FileStatus.Cancel }).ToList();
        }

        private string GetIndexContext(List<FileSituation> data)
        {
            StringBuilder sb = new StringBuilder();
            List<string> rows = new List<string>();
            int classIndex = 0;
            int clausesIndex = 0;
            int itemIndex = 0;
            int fineItemIndex = 0;
            for(int i = data.Count - 1; i >= 0; i--)
            {
                int nextClassId = 0;
                int nextClausesId = 0;
                int nextItemId = 0;
                int nextFineItemId = 0;
                classIndex++;
                clausesIndex++;
                itemIndex++;
                fineItemIndex++;
                if (i > 0)
                {
                    nextClassId = data[i - 1].ClassId;
                    nextClausesId = data[i - 1].ClausesId;
                    nextItemId = data[i - 1].ItemId;
                    nextFineItemId = data[i - 1].FineItemId;
                }
                string classRow = string.Empty;
                string clausesRow = string.Empty;
                string itemRow = string.Empty;
                string fineItemRow = string.Empty;
                string row = "<tr>";
                if (data[i].FineItemId != nextFineItemId && fineItemIndex > 0)
                {
                    fineItemRow = string.Format("<td rowspan='{0}'>{1}</td>", fineItemIndex, data[i].FineItemName);
                    fineItemIndex = 0;
                }
                if (data[i].ItemId != nextItemId && itemIndex > 0)
                {
                    itemRow = string.Format("<td rowspan='{0}'>{1}</td>", itemIndex, data[i].ItemName);
                    itemIndex = 0;
                    fineItemIndex = 0;
                }
                if (data[i].ClausesId != nextClausesId && clausesIndex > 0)
                {
                    clausesRow = string.Format("<td rowspan='{0}'>{1}</td>", clausesIndex, data[i].ClausesName);
                    clausesIndex = 0;
                    itemIndex = 0;
                    fineItemIndex = 0;
                }
                if (data[i].ClassId != nextClassId && classIndex > 0)
                {
                    classRow = string.Format("<td rowspan='{0}'>{1}</td>", classIndex, data[i].ClassName);
                    classIndex = 0;
                    clausesIndex = 0;
                    itemIndex = 0;
                    fineItemIndex = 0;
                }
                row += classRow + clausesRow + itemRow + fineItemRow;
                row += string.Format("<td><a href='{0}'>{1}</a></td>", data[i].PhysicalFullPath, data[i].FileName);
                row += "</tr>";
                rows.Add(row);
            }
            sb.Append("<!DOCTYPE html>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<meta charset = \"utf-8\" />");
            sb.Append("<title></title>");
            sb.Append("<style>");
            sb.Append("body {font-family: \"Helvetica Neue\",Helvetica,Arial,sans-serif;");
            sb.Append("text-align:center;}");
            sb.Append("table {white-space: normal;");
            sb.Append("line-height: normal;");
            sb.Append("font-weight: normal;");
            sb.Append("font-size: medium;");
            sb.Append("font-style: normal;");
            sb.Append("text-align: start;");
            sb.Append("font-variant: normal normal;");
            sb.Append("border: 1px solid #ddd;");
            sb.Append("width: 80%;");
            sb.Append("max-width: 80%;");
            sb.Append("margin-bottom: 20px;");
            sb.Append("background-color: transparent;");
            sb.Append("margin: 0px auto;}");
            sb.Append("tr>td {border: 1px solid #ddd;}");
            sb.Append("tr>th {padding: 2px;");
            sb.Append("vertical-align: inherit;");
            sb.Append("word -break: break-all;");
            sb.Append("word -break: break-word;");
            sb.Append("word-wrap: break-word;");
            sb.Append("border: 1px solid #ddd;}");
            sb.Append("</style>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<h1>打包压缩索引</h1>");
            sb.Append("<p>解压后才可进行下载</p>");
            sb.Append("<table>");
            sb.Append("<tr><td>类</td><td>条</td><td>项</td><td>细项</td><td>文件要求</td></tr>");
            for (int ii = rows.Count -1; ii >= 0; ii--)
            {
                sb.Append(rows[ii]);
            }
            sb.Append("</table>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        public Stream PackFile(int companyId, string physicalBasePath)
        {
            Dictionary<string, string> packfiles = new Dictionary<string, string>();
            List<FileSituation> indexData = new List<FileSituation>();
            var files = this.GetFileSituation(companyId).OrderBy(o=>o.ClassId).ThenBy(o=>o.ClausesId).ThenBy(o=>o.ItemId).ThenBy(o=>o.FineItemId).ThenBy(o=>o.FileRequireId);
            Func<string, string> format = i => {
                return System.Text.RegularExpressions.Regex.Replace(i, "[\\|/|:|*|?|\"|<|>|||]", "");
            };
            foreach (var item in files)
            {
                if (!string.IsNullOrEmpty(item.FileName))
                {
                    string key = string.Format("{0}/{1}/{2}/{3}", format(item.ClassName), format(item.ClausesName), format(item.ItemName), format(item.FileName));
                    if (!packfiles.ContainsKey(key))
                    {
                        packfiles.Add(key, item.PhysicalFullPath);
                        item.PhysicalFullPath = key;
                        indexData.Add(item);
                    }
                }
            }
            byte[] buffer = new byte[5120];
            MemoryStream returnStream = new MemoryStream();
            var zipMs = new MemoryStream();
            using (ZipOutputStream zipStream = new ZipOutputStream(zipMs))
            {
                zipStream.SetLevel(9);//设置压缩等级，等级越高文件越小
                foreach (var kv in packfiles)
                {
                    string fileName = kv.Key;
                    var file = _fileManager.Down(kv.Value);
                    if (file.Success)
                    {
                        using (var streamInput = file.FileStream)
                        {
                            zipStream.PutNextEntry(new ZipEntry(fileName));
                            while (true)
                            {
                                var readCount = streamInput.Read(buffer, 0, buffer.Length);
                                if (readCount>0)
                                {
                                    zipStream.Write(buffer, 0, readCount);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            zipStream.Flush();
                        }
                    }
                }
                byte[] indexbuffer = System.Text.Encoding.UTF8.GetBytes(GetIndexContext(indexData));
                zipStream.PutNextEntry(new ZipEntry("index.html"));
                zipStream.Write(indexbuffer,0, indexbuffer.Length);
                zipStream.Finish();
                zipMs.Position = 0;
                zipMs.CopyTo(returnStream, 5120);
            }
            returnStream.Position = 0;
            return returnStream;
        }
    }
}
