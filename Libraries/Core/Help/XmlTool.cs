using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Core.Help
{
    public static class Tools
    {
        private static readonly Type[] WriteTypes = new[] {
        typeof(string),
        typeof(Enum),
        typeof(DateTime), typeof(DateTime?),
        typeof(DateTimeOffset), typeof(DateTimeOffset?),
        typeof(int), typeof(int?),
        typeof(decimal), typeof(decimal?),
        typeof(Guid), typeof(Guid?),
    };
        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || WriteTypes.Contains(type);
        }
        public static object ToXml(this object input)
        {
            return input.ToXml(null);
        }
        public static object ToXml(this object input, string element)
        {
            if (input == null)
                return null;

            if (string.IsNullOrEmpty(element))
                element = "object";
            element = XmlConvert.EncodeName(element);
            var ret = new XElement(element);

            if (input != null)
            {
                var type = input.GetType();

                if (input is IEnumerable && !type.IsSimpleType())
                {
                    var elements = (input as IEnumerable<object>)
                        .Select(m => m.ToXml(element))
                        .ToArray();

                    return elements;
                }
                else
                {
                    var props = type.GetProperties();

                    var elements = from prop in props
                                   let name = XmlConvert.EncodeName(prop.Name)
                                   let val = prop.GetValue(input, null)
                                   let value = prop.PropertyType.IsSimpleType()
                                        ? new XElement(name, val)
                                        : val.ToXml(name)
                                   where value != null
                                   select value;

                    ret.Add(elements);
                }
            }

            return ret;
        }
    }
}
