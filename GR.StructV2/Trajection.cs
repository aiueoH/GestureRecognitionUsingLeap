using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
{
    public class MTrajection<T>
    {
        public int Begin { get; set; }
        public List<T> Elements { get; set; }

        public MTrajection()
        {
        }

        public MTrajection(int begin, List<T> elements)
        {
            Begin = begin;
            Elements = elements;
        }
    }
}
