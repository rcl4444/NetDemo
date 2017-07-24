using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AEOWebapi.Controllers
{
    public class AccountController: ApiController
    {
        public DateTime Get()
        {
            return DateTime.Now;
        }
    }
}
