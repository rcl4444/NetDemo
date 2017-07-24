using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AEOPoco.Domain;

namespace AEOPoco.Other
{
    [XmlRootAttribute("CustomsAuthentication", IsNullable = false)]
    public class XmlDocument
    {
        public XmlDocument()
        {
            OutlineClasses = new XmlOutlineClass[] { };
        }

        /// <summary>
        /// 认证标题名
        /// </summary>
        public string TitleName
        {
            get;
            set;
        }

        /// <summary>
        /// 海关版本标识
        /// </summary>
        public string CustomsVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 海关ID(唯一)
        /// </summary>
        public int CustomsID
        {
            get;
            set;
        }

        /// <summary>
        /// 类集合
        /// </summary>
        [XmlArrayItem("OutlineClass")]
        public XmlOutlineClass[] OutlineClasses
        {
            get;
            set;
        }
    }

    public class XmlOutlineClass
    {
        public XmlOutlineClass()
        {
            Clauseses = new XmlClauses[] { };
        }
        /// <summary>
        /// 类名
        /// </summary>
        public string OutlineClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 海关ID(唯一标识)
        /// </summary>
        public int CustomsID { get; set; }

        /// <summary>
        /// 条集合
        /// </summary>
        [XmlArrayItem("Clauses")]
        public XmlClauses[] Clauseses
        {
            get;
            set;
        }
    }

    public class XmlClauses
    {
        public XmlClauses()
        {
            Items = new XmlItem[] { };
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
        /// 海关ID(唯一)
        /// </summary>
        public int CustomsID
        {
            get;
            set;
        }

        /// <summary>
        /// 项集合
        /// </summary>
        [XmlArrayItem("Item")]
        public XmlItem[] Items
        {
            get;
            set;
        }
    }

    public class XmlItem
    {
        public XmlItem()
        {
            FineItems = new XmlFineItem[] { };
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
        /// 海关ID(唯一)
        /// </summary>
        public int CustomsID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否重要标识
        /// </summary>
        public bool IsImportant
        {
            get;
            set;
        }

        /// <summary>
        /// 评分项
        /// </summary>
        [XmlArrayItem("ScoreLevel")]
        public ScoreLevel[] ScoreLevels 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// 细项集合
        /// </summary>
        [XmlArrayItem("FineItem")]
        public XmlFineItem[] FineItems
        {
            get;
            set;
        }
    }

    public class XmlFineItem
    {
        public XmlFineItem()
        {
            FileRequires = new XmlFileRequire[] { };
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
        /// 海关ID(唯一)
        /// </summary>
        public int CustomsID
        {
            get;
            set;
        }

        /// <summary>
        /// 文件要求集合
        /// </summary>
        [XmlArrayItem("FileRequire")]
        public XmlFileRequire[] FileRequires
        {
            get;
            set;
        }
    }

    public class XmlFileRequire
    {
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 建议文件名
        /// </summary>
        public virtual string SuggestFileName
        {
            get;
            set;
        }

        /// <summary>
        /// 海关ID(唯一)
        /// </summary>
        public virtual int CustomsID
        {
            get;
            set;
        }
    }
}
