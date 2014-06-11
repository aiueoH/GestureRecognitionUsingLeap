using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR
{
    public class TwoIndexInfo : GestureInfo
    {
        public Point3D LeftPos { get; set; }
        public Point3D RightPos { get; set; }
        public Point3D DeltaDistanceXYZ { get; set; }
        public float DeltaDistance { get; set; }
    }
}
