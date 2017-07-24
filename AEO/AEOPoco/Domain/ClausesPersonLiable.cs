using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 条负责人
	/// </summary>
	public class ClausesPersonLiable : CreateTimeEntity
	{
		public virtual int ClausesID
		{
			get;
			set;
		}

		public virtual int CustomerAccountID
		{
			get;
			set;
		}

		public virtual int CustomerCompanyID
		{
			get;
			set;
		}

		public virtual Clauses Clauses
		{
			get;
			set;
		}

		public virtual CustomerAccount CustomerAccount
		{
			get;
			set;
		}

		public virtual CustomerCompany CustomerCompany
		{
			get;
			set;
		}

	}
}

