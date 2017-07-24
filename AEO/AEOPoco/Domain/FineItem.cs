using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 细项
	/// </summary>
	public class FineItem : CreateTimeEntity
	{
		/// <summary>
		/// 细项名
		/// </summary>
		public virtual string FineItemName
		{
			get;
			set;
		}

		/// <summary>
		/// 项ID
		/// </summary>
		public virtual int ItemID
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

		public virtual ICollection<FileRequire> FileRequires
		{
			get;
			set;
		}

		public virtual Item Item
		{
			get;
			set;
		}

	}
}

