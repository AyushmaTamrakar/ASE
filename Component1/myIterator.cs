using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal class myIterator : Iterator
    {
        Collection collection = null;
        int currentIndex = 0;

        public myIterator(Collection collection)
        {
            this.collection = collection;

        }
        public string FirstItem
        {
            get
            {
                currentIndex = 0;
                return collection[currentIndex];
            }
        }
        public string NextItem
        {
            get
            {
                currentIndex++;
                if (IsDone == false)
                {
                    return collection[currentIndex];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CurrentItem
        {
            get
            {
                return collection[currentIndex];
            }
        }
        public bool IsDone
        {
            get
            {
                if (currentIndex < collection.Count)
                {
                    return false;
                }
                return true;
            }
        }

    }
}
