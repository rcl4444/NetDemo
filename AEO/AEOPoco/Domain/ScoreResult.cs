using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 评分结果
	/// </summary>
	public class ScoreResult : CreateTimeEntity
	{
		/// <summary>
		/// 评分任务ID
		/// </summary>
		public virtual int ScoreTaskID
		{
			get;
			set;
		}

		/// <summary>
		/// 评分
		/// </summary>
		public virtual ScoreLevel Score
		{
			get;
			set;
		}

		/// <summary>
		/// 评分时间
		/// </summary>
		public virtual DateTime ScoreTime
		{
			get;
			set;
		}

		public virtual ScoreTask ScoreTask
		{
			get;
			set;
		}

	}
}

