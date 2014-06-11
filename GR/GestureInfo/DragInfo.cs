using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR
{
    public class DragInfo : GestureInfo
    {
        public Point3D HandPos { get; set; }
        public Point3D DeltaPos {get; set; }
        public float Distance {get; set; }
    }
}
