using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR
{
    public class DragInfo : GestureInfo
    {
        public GR.StructV2.Vector HandPos { get; set; }
        public GR.StructV2.Vector DeltaPos { get; set; }
        public float Distance {get; set; }
    }
}
