using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Other
{
    public class FileAuditResult
    {
        /// <summary>
        /// 文件要求Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 预计完成
        /// </summary>
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// 条
        /// </summary>
        public string ClausesName { get; set; }
        /// <summary>
        /// 项
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 细项内容
        /// </summary>
        public string FineItemName { get; set; }
        /// <summary>
        /// 文件要求
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 主办人
        /// </summary>
        public string PersonName { get; set; }
    }
}
