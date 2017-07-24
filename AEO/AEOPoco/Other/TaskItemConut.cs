using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Other
{
    public class TaskItemConut
    {
        //类ID
        public string OutlineClassName
        {
            get;
            set;
        }
        /// <summary>
        /// 条ID
        /// </summary>
        public int ClausesID
        {
            get;
            set;
        }

        /// <summary>
        /// 条名称
        /// </summary>
        public string ClausesName
        {
            get;
            set;
        }

        /// <summary>
        /// 项数
        /// </summary>
        public int ItemCount
        {
            get;
            set;
        }

        /// <summary>
        /// 细项数
        /// </summary>
        public int FineItemCount
        {
            get;
            set;
        }

        /// <summary>
        /// 文件数
        /// </summary>
        public int FileRequireCount
        {
            get;
            set;
        }
    }
}
