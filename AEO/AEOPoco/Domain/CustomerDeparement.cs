using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 客户部门
	/// </summary>
	public class CustomerDeparement : CreateTimeEntity
	{
		/// <summary>
		/// 部门名称
		/// </summary>
		public virtual string DeparementName
		{
			get;
			set;
		}

		/// <summary>
		/// 公司ID
		/// </summary>
		public virtual int CustomerCompanyID
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

		public virtual CustomerCompany CustomerCompany
		{
			get;
			set;
		}

		public virtual ICollection<CustomerAccount> CustomerAccounts
		{
			get;
			set;
		}

	}
}

