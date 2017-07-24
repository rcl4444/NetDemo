using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 条
	/// </summary>
	public class Clauses : CreateTimeEntity
	{
		public virtual string ClausesName
		{
			get;
			set;
		}

		/// <summary>
		/// 类ID
		/// </summary>
		public virtual int OutlineClassID
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

		public virtual ICollection<Item> Items
		{
			get;
			set;
		}

		public virtual OutlineClass OutlineClass
		{
			get;
			set;
		}

	}
}

