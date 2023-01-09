using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
     public interface Collection
    {
         Iterator getIterator();

        //string this[string index] { set; get; }
        
        int Count { get; }
    }
}
