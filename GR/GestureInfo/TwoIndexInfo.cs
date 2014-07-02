using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR
{
    public class TwoIndexInfo : GestureInfo
    {
        public GR.StructV2.Vector LeftPos { get; set; }
        public GR.StructV2.Vector RightPos { get; set; }
        public GR.StructV2.Vector DeltaDistanceXYZ { get; set; }
        public float DeltaDistance { get; set; }
    }
}
