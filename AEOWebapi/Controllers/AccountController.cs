using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AEOWebapi.Controllers
{
    //[Authorize]
    public class AccountController: ApiController
    {
        public HttpResponseMessage Get()
        {
            //JSON默认序列化方式会导致文本输出""包含,这里需要自己移除headers
            return new HttpResponseMessage()
            {
                Content = new StringContent("<xml></xml>", System.Text.Encoding.UTF8, "text/plain")
            };
        }
    }
}
