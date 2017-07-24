using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 评分记录
	/// </summary>
	public class ScoreOperationNote : BaseOperationNote
	{
		public virtual int ScoreTaskID
		{
			get;
			set;
		}

		public virtual ScoreTask ScoreTask
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

	}
}

