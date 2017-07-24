using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 整改任务结果
	/// </summary>
	public class CorrectiveTaskResult : CreateTimeEntity
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
		/// 物理路径
		/// </summary>
		public virtual string PhysicalFullPath
		{
			get;
			set;
		}

		/// <summary>
		/// 上传人ID
		/// </summary>
		public virtual int UploadPersonID
		{
			get;
			set;
		}

		/// <summary>
		/// 上传时间
		/// </summary>
		public virtual DateTime UploadTime
		{
			get;
			set;
		}

		/// <summary>
		/// 是否取消
		/// </summary>
		public virtual bool IsCancel
		{
			get;
			set;
		}

		/// <summary>
		/// 文件名
		/// </summary>
		public virtual string FileName
		{
			get;
			set;
		}

		/// <summary>
		/// MIME类型
		/// </summary>
		public virtual string ContentType
		{
			get;
			set;
		}

		public virtual CorrectiveTask CorrectiveTask
		{
			get;
			set;
		}

		public virtual CustomerAccount UploadPerson
		{
			get;
			set;
		}

	}
}

