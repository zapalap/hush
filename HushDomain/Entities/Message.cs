using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HushDomain.Entities
{
    public class Message 
    {
        public virtual long Id { get; set; }
        public virtual string Text { get; set; }
    }
}
