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
    public class FeedbackController : AuthorizeController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService, IWorkContext workContext)
            : base(workContext)
        {
            this._feedbackService = feedbackService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveFeedback(string description)
        {
            string message="";
            var success = _feedbackService.SaveFeedback(description, currentAccount.Id, out message) == true ? 1 : 0;
            return StandardJson("",success,message);
        }
    }
}