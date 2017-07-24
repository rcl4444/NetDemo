using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	public class CorrectiveTaskOperationNote : BaseOperationNote
	{
		/// <summary>
		/// 整改任务ID
		/// </summary>
		public virtual int CorrectiveTaskID
		{
			get;
			set;
		}

		/// <summary>
		/// 整改任务
		/// </summary>
		public virtual CorrectiveTask CorrectiveTask
		{
			get;
			set;
		}

	}
}

