using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR
{
    public class GestureInfo : EventArgs
    {
        public GestureState State { get; set; }
    }
}