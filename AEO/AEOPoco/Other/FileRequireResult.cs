using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Other
{
    public class FileRequireResult
    {
        /// <summary>
        /// 文件要求ID
        /// </summary>
        public int FileRequireId { get; set; }

        /// <summary>
        /// 文件结果ID
        /// </summary>
        public int? FileResultId { get; set; }

        /// <summary>
        /// 细项名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 预计完成
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// 实际完成
        /// </summary>
        public DateTime? UploadTime { get; set; }

        /// <summary>
        /// 文件要求
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 文件状态
        /// </summary>
        public FileStatus? Status { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 文件物理路径
        /// </summary>
        public string PhysicalFullPath { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public string AuditPerson { get; set; }

        /// <summary>
        /// 建议文件名
        /// </summary>
        public string SuggestFileName { get; set; }
    }
}
