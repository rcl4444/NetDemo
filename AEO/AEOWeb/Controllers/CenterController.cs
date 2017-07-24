using AEOPoco.Domain;
using AEOService.Interface;
using AEOWeb.Controllers;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEOWeb.Controllers
{
    public class CenterController : AuthorizeController
    {
        public CenterController(IWorkContext workContext)
            : base(workContext)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}