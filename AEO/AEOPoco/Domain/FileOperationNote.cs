using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 文件操作记录
	/// </summary>
	public class FileOperationNote : BaseOperationNote
	{
		public virtual int FileRequireID
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

