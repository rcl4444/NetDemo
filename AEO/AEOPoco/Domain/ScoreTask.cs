using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 评分任务
	/// </summary>
	public class ScoreTask : CreateTimeEntity
	{
		/// <summary>
		/// 项ID
		/// </summary>
		public virtual int ItemID
		{
			get;
			set;
		}

		/// <summary>
		/// 评分人
		/// </summary>
		public virtual int ScorePersonID
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
		/// 评分
		/// </summary>
		public virtual ScoreLevel? Score
		{
			get;
			set;
		}

		/// <summary>
		/// 评分时间
		/// </summary>
		public virtual DateTime? ScoreTime
		{
			get;
			set;
		}

		public virtual Item Item
		{
			get;
			set;
		}

		public virtual CustomerAccount ScorePerson
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

