using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 客户公司
	/// </summary>
	public class CustomerCompany : CreateTimeEntity
	{
		/// <summary>
		/// 公司名称
		/// </summary>
		public virtual string CompanyName
		{
			get;
			set;
		}

		/// <summary>
		/// 唯一标识
		/// </summary>
		public virtual string UniqueFlag
		{
			get;
			set;
		}

		/// <summary>
		/// 海关认证ID
		/// </summary>
		public virtual int? CustomsAuthenticationID
		{
			get;
			set;
		}

		/// <summary>
		/// 导出设置
		/// </summary>
		public virtual bool? ExportSetting
		{
			get;
			set;
		}

		public virtual CustomsAuthentication CustomsAuthentication
		{
			get;
			set;
		}

		public virtual ICollection<CustomerDeparement> CustomerDeparements
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

