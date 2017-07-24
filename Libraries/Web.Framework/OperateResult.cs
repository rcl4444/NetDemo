using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Web.Models
{
    public class OperateResult
    {
        public int Success { get; set; }

        public object Obj { get; set; }

        public string Message { get; set; }

        public string ToJson()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }
}