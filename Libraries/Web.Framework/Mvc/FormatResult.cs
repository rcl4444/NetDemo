using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Framework.Mvc
{
    public class FormatResult: JsonResult
    {
        protected JsonSerializerSettings setting = new JsonSerializerSettings();

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            //response.ContentType = "application/json";
            if (this.Data != null)
            {
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                response.Write(JsonConvert.SerializeObject(this.Data, setting));
            }
        }
    }
}
