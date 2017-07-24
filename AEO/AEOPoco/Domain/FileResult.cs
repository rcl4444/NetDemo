using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AEOPoco.Domain
{
	/// <summary>
	/// 文件结果
	/// </summary>
	public class FileResult : CreateTimeEntity
	{
		/// <summary>
		/// 文件要求ID
		/// </summary>
		public virtual int FileRequireID
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
		/// 物理路径
		/// </summary>
		public virtual string PhysicalFullPath
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
		/// 文件状态
		/// </summary>
		public virtual FileStatus Status
		{
			get;
			set;
		}

		/// <summary>
		/// 审核时间
		/// </summary>
		public virtual DateTime? AuditTime
		{
			get;
			set;
		}

		/// <summary>
		/// 上传用户ID
		/// </summary>
		public virtual int UploadPersonID
		{
			get;
			set;
		}

		/// <summary>
		/// 上传文件类型
		/// </summary>
		public virtual string ContentType
		{
			get;
			set;
		}

		/// <summary>
		/// 提审日期
		/// </summary>
		public virtual DateTime? ApplyAuditTime
		{
			get;
			set;
		}

		public virtual FileRequire FileRequire
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

