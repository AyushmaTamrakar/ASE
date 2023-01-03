using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    public interface Iterator
    {
        public bool hasNext();
        public object next();
    }
}
