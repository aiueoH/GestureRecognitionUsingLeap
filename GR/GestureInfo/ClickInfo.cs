using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR
{
    public class ClickInfo : GestureInfo
    {
        public GR.StructV2.Vector StablePos { get; set; }
        public GR.StructV2.Vector StableDir { get; set; }
    }
}
