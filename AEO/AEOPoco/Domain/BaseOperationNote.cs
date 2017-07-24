using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 日志记录基类
	/// </summary>
	public abstract class BaseOperationNote : CreateTimeEntity
	{
		/// <summary>
		/// 操作者ID
		/// </summary>
		public virtual int OperationerID
		{
			get;
			set;
		}

		/// <summary>
		/// 操作者
		/// </summary>
		public virtual CustomerAccount Operationer
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

	}
}

