using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Other
{
    public class FileSituation
    {
        /// <summary>
        /// 类ID
        /// </summary>
        public int ClassId
        {
            get;
            set;
        }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            get;
            set;
        }

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
        /// 细项名
        /// </summary>
        public string FineItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件要求Id
        /// </summary>
        public int FileRequireId
        {
            get;
            set;
        }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件物理路径
        /// </summary>
        public string PhysicalFullPath
        {
            get;
            set;
        }
    }
}
