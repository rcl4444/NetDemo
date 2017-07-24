using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 项
	/// </summary>
	public class Item : CreateTimeEntity
	{
		/// <summary>
		/// 项名
		/// </summary>
		public virtual string ItemName
		{
			get;
			set;
		}

		/// <summary>
		/// 条ID
		/// </summary>
		public virtual int ClausesID
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

		/// <summary>
		/// 是否重要
		/// </summary>
		public virtual bool IsImportant
		{
			get;
			set;
		}

		public virtual ICollection<FineItem> FineItems
		{
			get;
			set;
		}

		public virtual Clauses Clauses
		{
			get;
			set;
		}

		public virtual ICollection<ItemScoreConfigure> ScoreConfigure
		{
			get;
			set;
		}

	}
}

