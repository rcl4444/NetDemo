using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 文件要求
	/// </summary>
	public class FileRequire : CreateTimeEntity
	{
		/// <summary>
		/// 细项ID
		/// </summary>
		public virtual int FineItemID
		{
			get;
			set;
		}

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
		/// 客户公司ID
		/// </summary>
		public virtual int CustomerCompanyID
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

		public virtual FileSchedule FileSchedule
		{
			get;
			set;
		}

		public virtual CustomerCompany CustomerCompany
		{
			get;
			set;
		}

		public virtual FineItem FineItem
		{
			get;
			set;
		}

	}
}

