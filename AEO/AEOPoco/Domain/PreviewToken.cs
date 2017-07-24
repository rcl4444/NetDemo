using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	public class PreviewToken : CreateTimeEntity
	{
		public virtual string Token
		{
			get;
			set;
		}

		public virtual string Path
		{
			get;
			set;
		}

		public virtual string ContentType
		{
			get;
			set;
		}

	}
}

