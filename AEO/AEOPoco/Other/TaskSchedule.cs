using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Other
{
    /// <summary>
    /// 任务进度
    /// </summary>
    public class TaskSchedule
    {
        /// <summary>
        /// 条ID
        /// </summary>
        public int ClausesId
        {
            get;
            set;
        }

        /// <summary>
        /// 条名
        /// </summary>
        public string ClausesName
        {
            get;
            set;
        }

        /// <summary>
        /// 条负责人
        /// </summary>
        public string ClausesChargePerson
        {
            get;
            set;
        }

        /// <summary>
        /// 项ID
        /// </summary>
        public int ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 项名
        /// </summary>
        public string ItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 细项ID
        /// </summary>
        public int FineItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 细项ID
        /// </summary>
        public string FineItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件要求ID
        /// </summary>
        public int FileRequireID
        {
            get;
            set;
        }

        /// <summary>
        /// 建议文件名
        /// </summary>
        public string SuggestFileName
        {
            get;
            set;
        }

        /// <summary>
        /// 主办人
        /// </summary>
        public string ChargePerson
        {
            get;
            set;
        }

        /// <summary>
        /// 预期完成时间
        /// </summary>
        public DateTime? FinishTime
        {
            get;
            set;
        }
        
        /// <summary>
        /// 真实上传文件时间
        /// </summary>
        public DateTime? RealFinishTime
        {
            get;
            set;
        }

        /// <summary>
        /// 进度类型
        /// </summary>
        public ScheduleType Type
        {
            get;
            set;
        }
    }

    public enum ScheduleType
    {
        /// <summary>
        /// 文件要求
        /// </summary>
        FileRequire = 0,

        /// <summary>
        /// 整改任务
        /// </summary>
        CorrectiveTask = 1
    }
}
