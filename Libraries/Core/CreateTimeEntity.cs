using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class CreateTimeEntity : BaseEntity
    {
        public DateTime CreateTime { get; set; }
    }
}
