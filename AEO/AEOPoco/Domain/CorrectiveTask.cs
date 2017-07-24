using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 整改任务
	/// </summary>
	public class CorrectiveTask : CreateTimeEntity
	{
		/// <summary>
		/// 整改内容
		/// </summary>
		public virtual string CorrectiveContent
		{
			get;
			set;
		}

		/// <summary>
		/// 预计完成时间
		/// </summary>
		public virtual DateTime FinishTime
		{
			get;
			set;
		}

		/// <summary>
		/// 主办人
		/// </summary>
		public virtual int ChargePersonID
		{
			get;
			set;
		}

		/// <summary>
		/// 审核人
		/// </summary>
		public virtual int AuditorID
		{
			get;
			set;
		}

		/// <summary>
		/// 创建者
		/// </summary>
		public virtual int CreaterID
		{
			get;
			set;
		}

		/// <summary>
		/// 公司ID
		/// </summary>
		public virtual int CompanyID
		{
			get;
			set;
		}

		/// <summary>
		/// 整改对象ID
		/// </summary>
		public virtual int CorrectiveObjectID
		{
			get;
			set;
		}

		/// <summary>
		/// 状态
		/// </summary>
		public virtual CorrectiveTaskStatus Status
		{
			get;
			set;
		}

		/// <summary>
		/// 是否删除
		/// </summary>
		public virtual bool IsDelete
		{
			get;
			set;
		}

		/// <summary>
		/// 完成时间
		/// </summary>
		public virtual DateTime? CompleteTime
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

		public virtual CustomerAccount CreatePerson
		{
			get;
			set;
		}

		public virtual CustomerCompany CustomerCompany
		{
			get;
			set;
		}

		public virtual Clauses CorrectiveObject
		{
			get;
			set;
		}

		public virtual ICollection<CorrectiveTaskResult> CorrectiveTaskResults
		{
			get;
			set;
		}

	}
}

