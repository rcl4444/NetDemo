using AEOPoco.Domain;
using AEOService.Interface;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace AEOService.Services
{
    public class OutlineclassService : BaseService<OutlineClass>, IOutlineclassService
    {
        protected readonly IRepository<ClausesPersonLiable> _clausesPersonLiableRepository;
        protected readonly IRepository<Clauses> _clausesRepository;
        protected readonly IRepository<Item> _itemRepository;
        protected readonly IRepository<FineItem> _fineItemRepository;
        protected readonly IRepository<FileRequire> _fileRequireRepository;
        protected readonly IRepository<ItemScoreConfigure> _itemScoreConfigureRepository;
        protected readonly IRepository<CustomerCompany> _customerCompanyRepository;
        protected readonly IRepository<ScoreTask> _scoreTaskRepository;
        protected readonly IScoreTaskService _coreTaskService;
        

        public OutlineclassService(IRepository<OutlineClass> OutlineClass,
            IRepository<ClausesPersonLiable> clausesPersonLiableRepository,
            IRepository<Clauses> clausesRepository,
            IRepository<Item> itemRepository,
            IRepository<FineItem> fineItemRepository,
            IRepository<FileRequire> fileRequireRepository,
            IRepository<ItemScoreConfigure> itemScoreConfigureRepository,
            IRepository<CustomerCompany> customerCompanyRepository,
            IRepository<ScoreTask> scoreTaskRepository,
            IScoreTaskService coreTaskService)
            : base(OutlineClass)
        { 
            this._clausesPersonLiableRepository=clausesPersonLiableRepository;
            this._clausesRepository = clausesRepository;
            this._itemRepository = itemRepository;
            this._fineItemRepository = fineItemRepository;
            this._fileRequireRepository = fileRequireRepository;
            this._itemScoreConfigureRepository = itemScoreConfigureRepository;
            this._customerCompanyRepository = customerCompanyRepository;
            this._scoreTaskRepository = scoreTaskRepository;
            this._coreTaskService = coreTaskService;
        }
        public IQueryable<dynamic> OutlineclassSerach(int CompanyID)
        {
            var query = (from c in this.NoTrackingQuery
                         join cp in _clausesPersonLiableRepository.TableNoTracking on c.Id equals cp.ClausesID into temp
                         from c1 in temp.DefaultIfEmpty()
                         select new
                         {
                             c1.Clauses.ClausesName, //类
                             c1.CustomerAccount.AccountName //主办人
                         });
            return query;
        }

        public NPOI.SS.UserModel.IWorkbook GetExportData(int companyid, NPOI.SS.UserModel.IWorkbook book, string[] CellArray, string templetFileName, bool IsSenior)
        {
            var company = _customerCompanyRepository.TableNoTracking.Where(o => o.Id == companyid).FirstOrDefault();
            using (FileStream file = new FileStream(templetFileName, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = hssfworkbook.GetSheet("Sheet1");

                var query = (from oc in this.NoTrackingQuery.Where(o => o.CustomsAuthenticationID == company.CustomsAuthenticationID)
                             join c in _clausesRepository.TableNoTracking on oc.Id equals c.OutlineClassID
                             join i in _itemRepository.TableNoTracking on c.Id equals i.ClausesID
                             join st in _scoreTaskRepository.TableNoTracking on i.Id equals st.ItemID into temp
                             from tp in temp.DefaultIfEmpty()
                             join fi in _fineItemRepository.TableNoTracking on i.Id equals fi.ItemID
                             join fr in _fileRequireRepository.TableNoTracking on fi.Id equals fr.FineItemID into temp1
                             from tp1 in temp1.DefaultIfEmpty()
                             select new
                             {
                                 oc.OutlineClassName, //类名称
                                 c.ClausesName,// 条名称
                                 ItemID = i.Id, //项ID
                                 i.ItemName, //项名称
                                 tp.Score,//评分
                                 fi.FineItemName, //细项名称
                                 tp1.Id,
                                 tp1.SuggestFileName,//建议文件名
                                 scorecfg = i.ScoreConfigure.Select(o => new
                                 {
                                     o.ScoreValue, //评分项
                                 }).ToList()
                             }
                 );

                //排序项
                var itemlist = (from a in query
                                   group a by new
                                   {
                                       a.ItemName,
                                       a.ItemID
                                   }
                                       into g
                                       select new
                                       {
                                           g.Key.ItemName,
                                           g.Key.ItemID,
                                           list = g.Select(o => new { 
                                           o.SuggestFileName,
                                           o.Score
                                           }).ToList()
                                       }).OrderBy(o => o.ItemID).ToList();

                Dictionary<int, FileNameScore> value = new Dictionary<int, FileNameScore>();
                var n = 0;
                //获取该项的文件要求、分数
                foreach (var item in itemlist)
                {
                    var str = "";
                    for (int i = 0; i < item.list.Count(); i++)
                    {
                        if (!str.Contains(item.list[i].SuggestFileName))
                        {
                            if (i == item.list.Count() - 1)
                            {
                                str = str + item.list[i].SuggestFileName;
                            }
                            else
                            {
                                str = str + item.list[i].SuggestFileName + Environment.NewLine;
                            }
                            
                        }
                        if (i == item.list.Count() - 1)
                        {

                            value.Add(n, new FileNameScore{ SuggestFileName = str, Score = item.list[i].Score });
                        }
                    }
                    n++;
                }
                var Score = 100;
                //给每一行标记过的数据设置数据
                for (int i = 0; i < CellArray.Length; i++)
                {
                    var curcell = CellArray[i];
                    var b = value[i];
                    if (IsSenior ? curcell == "29" : curcell == "24")
                    {
                        IRow row = sheet1.GetRow(Convert.ToInt32(curcell));
                        IRow row1 = sheet1.GetRow(Convert.ToInt32(curcell) + 1);
                        row.GetCell(3).SetCellValue(b.SuggestFileName);
                        if (b.Score.HasValue)
                        {
                            if (b.Score.Value == ScoreLevel.ReachStandard)
                            {
                                row1.GetCell(4).SetCellValue("0");
                            }
                            if (b.Score.Value == ScoreLevel.Substandard)
                            {
                                row1.GetCell(5).SetCellValue("-2");
                                Score += -2;
                            }
                            if (b.Score.Value == ScoreLevel.NotApplicable)
                            {
                                row1.GetCell(7).SetCellValue("-");
                            }
                        }
                    }
                    else if (IsSenior ? curcell == "58" : curcell == "53")
                    {
                        IRow row = sheet1.GetRow(Convert.ToInt32(curcell));
                        row.GetCell(3).SetCellValue(b.SuggestFileName);

                        if (b.Score.HasValue)
                        {
                            if (b.Score.Value == ScoreLevel.Conform)
                            {
                                row.GetCell(4).SetCellValue("2");
                                Score += 2;
                            }
                            if (b.Score.Value == ScoreLevel.NotApplicableV2)
                            {
                                row.GetCell(6).SetCellValue("0");
                            }
                        }
                    }
                    else
                    {
                        IRow row = sheet1.GetRow(Convert.ToInt32(curcell));
                        row.GetCell(3).SetCellValue(b.SuggestFileName);

                        if (b.Score.HasValue)
                        {
                            if (b.Score.Value == ScoreLevel.ReachStandard)
                            {
                                row.GetCell(4).SetCellValue("0");
                            }
                            if (IsSenior ? i > 15 && i < 24 : i > 11 && i < 20)
                            {
                                if (b.Score.Value == ScoreLevel.Substandard)
                                {
                                    row.GetCell(5).SetCellValue("-2");
                                    Score += -2;
                                }
                            }
                            else
                            {
                                if (b.Score.Value == ScoreLevel.PartiallyCompliant)
                                {
                                    row.GetCell(5).SetCellValue("-1");
                                    Score += -1;
                                }
                                if (b.Score.Value == ScoreLevel.Substandard)
                                {
                                    row.GetCell(6).SetCellValue("-2");
                                    Score += -2;
                                }
                            }
                            if (b.Score.Value == ScoreLevel.NotApplicable)
                            {
                                row.GetCell(7).SetCellValue("-");
                            }
                        }
                    }
                    if (i == CellArray.Length - 1)
                    {
                        IRow row = sheet1.GetRow(Convert.ToInt32(curcell)+1);
                        if (Score >= 95)
                        {
                            row.GetCell(1).SetCellValue("认证通过，认证分数为：" + Score);
                        }
                        else 
                        {
                            row.GetCell(1).SetCellValue("认证不通过，认证分数为：" + Score);
                        }
                    }
                }
                return hssfworkbook;
            }
        }

        public class FileNameScore
        {
            public string SuggestFileName;
            public ScoreLevel? Score;
        }
    }
}
