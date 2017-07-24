using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOPoco.Domain;

namespace AEOPoco.Other
{
    public class FileRequireStatus
    {
        /// <summary>
        /// 文件要求ID
        /// </summary>
        public int FileRequireId
        {
            get;
            set;
        }

        /// <summary>
        /// 文件状态
        /// </summary>
        public FileStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime
        {
            get;
            set;
        }
    }
}
