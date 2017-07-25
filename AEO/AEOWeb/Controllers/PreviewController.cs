using AEOPoco.Domain;
using AEOService.Interface;
using AEOWeb.Controllers;
using Core;
using Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.Controllers;

namespace AEOWeb.Controllers
{
    public class PreviewController : BasePublicController
    {
        private readonly IFileResultService _fileResultService;
        private readonly MyConfig _myConfig;
        private readonly IPreviewTokenService _previewTokenService;

        public PreviewController(IFileResultService fileResultService,
            MyConfig myConfig,
            IPreviewTokenService previewTokenService,
            IWorkContext workContext)
        {
            this._fileResultService = fileResultService;
            this._myConfig = myConfig;
            this._previewTokenService = previewTokenService;
        }

        //文件预览方法测试
        [AllowAnonymous]
        public ActionResult File(string id)
        {
            var preview = _previewTokenService.NoTrackingQuery.Where(o => o.Token == id).FirstOrDefault();

            if (preview != null)
            {
                TimeSpan ts = DateTime.Now - preview.CreateTime;
                if (ts.Seconds > _myConfig.PriviewTimeOut)
                {
                    return Content("<div style='text-align:center; '><img src='/Content/image/timeout.jpg'/></div>");
                }
                else
                {
                    //Response.AppendHeader("Content-Disposition", "inline; filename=" + Server.UrlEncode(fileinfo.FileName));
                    //return File(fileinfo.FileStream, fileinfo.ContentType);
                }
            }
            return Content("文件不存在");
        }
    }
}