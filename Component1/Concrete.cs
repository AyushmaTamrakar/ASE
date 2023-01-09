using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    public class Concrete : Collection
    {
        Dictionary<string,string> variable = null;
        public Concrete()
        {
            variable = new Dictionary<string, string>();
        }
        public Iterator getIterator()
        {
            return new myIterator(this);
        }
   
        public int Count
        {
            get { return variable.Count; }
        }

    }
}
