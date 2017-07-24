using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOPoco.Domain;
using Repository.Interface;
using System.Collections.ObjectModel;
using AEOPoco.Other;

namespace AEOService.Services
{
    public class CustomerCompanyService : BaseService<CustomerCompany>, ICustomerCompanyService
    {
        public CustomerCompanyService(IRepository<CustomerCompany> customerCompanyRepository):base(customerCompanyRepository)
        {

        }

        public CustomerCompany GetCompany(string uniqueFlag)
        {
            return this.Query.Where(o => o.UniqueFlag == uniqueFlag).FirstOrDefault();
        }

        public CustomerCompany GetCompany(int companyID)
        {
            return this.GetByID(companyID);
        }

        public CustomerCompany GetDeparement(int companyID)
        {
            return this.Query.Where(o => o.Id == companyID).FirstOrDefault();
        }

        public bool ServiceInitialize(CustomerCompany company, XmlDocument obj, out string message)
        {
            if (company.CustomsAuthenticationID.HasValue)
            {
                message = "该公司已经初始化";
                return false;
            }
            else
            {
                bool b = false;
                using (var tran = this.BeginTransaction())
                {
                    try
                    {
                        DateTime createtime = DateTime.Now;
                        var document = new CustomsAuthentication() {
                            TitleName = obj.TitleName,
                            CustomsVersion = obj.CustomsVersion,
                            CustomsID = obj.CustomsID,
                            CreateTime = createtime
                        };
                        List<OutlineClass> classli = new List<OutlineClass>();
                        b = obj.OutlineClasses.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Count() > 0;
                        if (!b)
                        {
                            foreach (var xmlclass in obj.OutlineClasses)
                            {
                                var outlintclass = new OutlineClass()
                                {
                                    OutlineClassName = xmlclass.OutlineClassName,
                                    CustomsID = xmlclass.CustomsID,
                                    CreateTime = createtime,
                                    CustomsAuthentication = document
                                };
                                
                                List<Clauses> clausesli = new List<Clauses>();
                                b = xmlclass.Clauseses.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Count() > 0;
                                if (!b)
                                {
                                    foreach (var xmlclauses in xmlclass.Clauseses)
                                    {
                                        var clauses = new Clauses()
                                        {
                                            ClausesName = xmlclauses.ClausesName,
                                            CustomsID = xmlclauses.CustomsID,
                                            CreateTime = createtime,
                                            OutlineClass = outlintclass
                                        };
                                        List<Item> itemli = new List<Item>();
                                        b = xmlclauses.Items.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Count() > 0;
                                        if (!b)
                                        {
                                            foreach (var xmlitem in xmlclauses.Items)
                                            {
                                                var item = new Item()
                                                {
                                                    ItemName = xmlitem.ItemName,
                                                    CustomsID = xmlitem.CustomsID,
                                                    CreateTime = createtime,
                                                    Clauses = clauses,
                                                    IsImportant = xmlitem.IsImportant
                                                };
                                                List<ItemScoreConfigure> iscli = new List<ItemScoreConfigure>();
                                                foreach (var sl in xmlitem.ScoreLevels)
                                                {
                                                    var isc = new ItemScoreConfigure(){
                                                        Item = item,
                                                        ScoreValue = sl,
                                                        CreateTime = DateTime.Now
                                                    };
                                                    iscli.Add(isc);
                                                }
                                                item.ScoreConfigure = iscli;
                                                List<FineItem> fineitemli = new List<FineItem>();
                                                b = xmlitem.FineItems.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Count() > 0;
                                                if (!b)
                                                {
                                                    foreach (var xmlfineitem in xmlitem.FineItems)
                                                    {
                                                        var fineitem = new FineItem()
                                                        {
                                                            FineItemName = xmlfineitem.FineItemName,
                                                            CustomsID = xmlfineitem.CustomsID,
                                                            CreateTime = createtime,
                                                            Item = item
                                                        };
                                                        List<FileRequire> filerequireli = new List<FileRequire>();
                                                        b = xmlfineitem.FileRequires.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Count() > 0;
                                                        if (!b)
                                                        {
                                                            foreach (var xmlfilerequire in xmlfineitem.FileRequires)
                                                            {
                                                                var filerequire = new FileRequire()
                                                                {
                                                                    Description = xmlfilerequire.Description,
                                                                    SuggestFileName = xmlfilerequire.SuggestFileName,
                                                                    CustomsID = xmlfilerequire.CustomsID,
                                                                    CreateTime = createtime,
                                                                    CustomerCompany = company,
                                                                    FineItem = fineitem
                                                                };
                                                                filerequireli.Add(filerequire);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            message = string.Format("细项:{0}下的文件要求CustomsID{1}重复", xmlfineitem.FineItemName, string.Join(",", xmlfineitem.FileRequires.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Select(o=>o.Key)));
                                                            return false;
                                                        }
                                                        fineitem.FileRequires = filerequireli;
                                                        fineitemli.Add(fineitem);
                                                    }
                                                }
                                                else
                                                {
                                                    message = string.Format("项:{0}下的细项CustomsID{1}重复", xmlitem.ItemName, string.Join(",", xmlitem.FineItems.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Select(o => o.Key)));
                                                    return false;
                                                }
                                                item.FineItems = fineitemli;
                                                itemli.Add(item);
                                            }
                                        }
                                        else
                                        {
                                            message = string.Format("条:{0}下的项CustomsID{1}重复", xmlclauses.ClausesName, string.Join(",", xmlclauses.Items.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Select(o => o.Key)));
                                            return false;
                                        }
                                        clauses.Items = itemli;
                                        clausesli.Add(clauses);
                                    }
                                }
                                else
                                {
                                    message = string.Format("类:{0}下的条CustomsID{1}重复", xmlclass.OutlineClassName, string.Join(",", xmlclass.Clauseses.GroupBy(l => l.CustomsID).Where(g => g.Count() > 1).Select(o => o.Key)));
                                    return false;
                                }
                                outlintclass.Clauseses = clausesli;
                                classli.Add(outlintclass);
                            }
                        }
                        else
                        {
                            message = "该类模板初始化失败，请联系客服";
                            return false;
                        }
                        document.OutlineClasses = classli;
                        company.CustomsAuthentication = document;
                        this.Update(company);
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
        }
    }
}
