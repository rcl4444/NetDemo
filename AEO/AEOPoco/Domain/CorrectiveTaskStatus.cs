﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace AEOPoco.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// 整改任务状态
	/// </summary>
	public enum CorrectiveTaskStatus : int
	{
		/// <summary>
		/// 新创建
		/// </summary>
		Create = 0,
		/// <summary>
		/// 完成
		/// </summary>
		Finish = 1,
		/// <summary>
		/// 审核
		/// </summary>
		Audit = 2,
		/// <summary>
		/// 审核不通过
		/// </summary>
		Reject = -1,
	}
}
