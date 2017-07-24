using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 文件排程
	/// </summary>
	public class FileSchedule : CreateTimeEntity
	{
		/// <summary>
		/// 完成时间
		/// </summary>
		public virtual DateTime? FinishTime
		{
			get;
			set;
		}

		/// <summary>
		/// 审核者ID
		/// </summary>
		public virtual int? AuditorID
		{
			get;
			set;
		}

		/// <summary>
		/// 主办人ID
		/// </summary>
		public virtual int ChargePersonID
		{
			get;
			set;
		}

		public virtual CustomerAccount ChargePerson
		{
			get;
			set;
		}

		public virtual CustomerAccount Auditor
		{
			get;
			set;
		}

		public virtual FileRequire FileRequire
		{
			get;
			set;
		}

	}
}

