using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 海关认证
	/// </summary>
	public class CustomsAuthentication : CreateTimeEntity
	{
		/// <summary>
		/// 认证标题名
		/// </summary>
		public virtual string TitleName
		{
			get;
			set;
		}

		/// <summary>
		/// 海关版本标识
		/// </summary>
		public virtual string CustomsVersion
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

		public virtual ICollection<OutlineClass> OutlineClasses
		{
			get;
			set;
		}

	}
}

