using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOPoco.Domain;

namespace AEOPoco.Other
{
    public class ScoreRequire
    {
        /// <summary>
        /// 评分任务ID
        /// </summary>
        public int ScoreTaskID 
        {
            get;
            set;
        }

        /// <summary>
        /// 评分
        /// </summary>
        public ScoreLevel Score
        {
            get;
            set;
        }

        public IList<FileResultRejectDetail> Argument
        {
            get;
            set;
        }
    }

    public class FileResultRejectDetail
    {
        /// <summary>
        /// 文件结果ID
        /// </summary>
        public int FileResultID
        {
            get;
            set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public string Cause
        {
            get;
            set;
        }
    }
}
