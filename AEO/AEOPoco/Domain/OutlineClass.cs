using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 类
	/// </summary>
	public class OutlineClass : CreateTimeEntity
	{
		public virtual string OutlineClassName
		{
			get;
			set;
		}

		/// <summary>
		/// 海关认证ID
		/// </summary>
		public virtual int CustomsAuthenticationID
		{
			get;
			set;
		}

		/// <summary>
		/// 海关ID(唯一标识)
		/// </summary>
		public virtual int CustomsID
		{
			get;
			set;
		}

		public virtual ICollection<Clauses> Clauseses
		{
			get;
			set;
		}

		public virtual CustomsAuthentication CustomsAuthentication
		{
			get;
			set;
		}

	}
}

