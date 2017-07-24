using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 客户用户
	/// </summary>
	public class CustomerAccount : AbstractAccount
	{
		/// <summary>
		/// 用户真实姓名
		/// </summary>
		public virtual string PersonName
		{
			get;
			set;
		}

		/// <summary>
		/// 是否公司管理员
		/// </summary>
		public virtual bool IsManager
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
		/// 部门ID
		/// </summary>
		public virtual int? CustomerDeparementID
		{
			get;
			set;
		}

		/// <summary>
		/// 是否变更初始密码
		/// </summary>
		public virtual bool HasChange
		{
			get;
			set;
		}

		public virtual CustomerCompany CustomerCompany
		{
			get;
			set;
		}

		public virtual CustomerDeparement CustomerDeparement
		{
			get;
			set;
		}

		public virtual ICollection<UserRole> UserRoles
		{
			get;
			set;
		}

		public virtual ICollection<Feedback> Feedback
		{
			get;
			set;
		}

	}
}

