using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	public class UserRole : CreateTimeEntity
	{
		public virtual int CustomerAccountID
		{
			get;
			set;
		}

		public virtual Role RoleID
		{
			get;
			set;
		}

		public virtual CustomerAccount CustomerAccount
		{
			get;
			set;
		}

	}
}

