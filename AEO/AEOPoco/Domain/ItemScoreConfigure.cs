using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 项评分项配置
	/// </summary>
	public class ItemScoreConfigure : CreateTimeEntity
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
		/// 对应评分项
		/// </summary>
		public virtual ScoreLevel ScoreValue
		{
			get;
			set;
		}

		public virtual Item Item
		{
			get;
			set;
		}

	}
}

