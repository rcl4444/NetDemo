using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Framework.Controllers
{
    public class CommonController : BasePublicController
    {
        // GET: Common
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}