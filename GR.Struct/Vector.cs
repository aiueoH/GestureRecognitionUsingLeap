using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.Struct
{
    [Serializable]
    public class Vector
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public float Yaw { get; set; }
        public float Magnitude { get; set; }
        public float MagnitudeSquared { get; set; }
        //public Vector Normalized { get; set; }

        private Vector() {}

        public Vector(Leap.Vector vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
            Pitch = vector.Pitch;
            Roll = vector.Roll;
            Yaw = vector.Yaw;
            Magnitude = vector.Magnitude;
            MagnitudeSquared = vector.MagnitudeSquared;
        }

        public Vector(Vector vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
            Pitch = vector.Pitch;
            Roll = vector.Roll;
            Yaw = vector.Yaw;
            Magnitude = vector.Magnitude;
            MagnitudeSquared = vector.MagnitudeSquared;
        }

        public Vector PosOffset(Vector vector)
        {
            Vector v = new Vector(this);
            v.x -= vector.x;
            v.y -= vector.y;
            v.z -= vector.z;
            return v;
        }

        public float DistanceTo(Vector vector)
        {
            return (float)Math.Sqrt(Math.Pow(x - vector.x, 2) + Math.Pow(y - vector.y, 2) + Math.Pow(z - vector.z, 2)) / 100f;
        }

        public float XDistanceTo(Vector vector)
        {
            return (float)Math.Abs(x - vector.x);
        }
    }
}
