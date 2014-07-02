using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
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

        /// <summary>
        /// 1, 0, 0
        /// </summary>
        public static Vector Right { get { return new Vector(1f, 0f, 0f); } }
        /// <summary>
        /// 0, 1, 0
        /// </summary>
        public static Vector Up { get { return new Vector(0f, 1f, 0f); } }
        /// <summary>
        /// 0, 0, -1
        /// </summary>
        public static Vector Forward { get { return new Vector(0f, 0f, -1f); } }

        private Vector() {}

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            Magnitude = ComputMagnitude();
        }

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
        
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.x, -v.y, -v.z);
        }

        public float ComputMagnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector Normalize()
        {
            return new Vector(x / Magnitude, y / Magnitude, z / Magnitude);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns>degree</returns>
        public float AngleTo(Vector v)
        {
            float dot = Normalize().Dot(v.Normalize());
            if (dot > 1f &&
                dot - 1f < 0.00001f)
                dot = 1f;
            return (float)Math.Acos(dot);
        }

        public float Dot(Vector v)
        {
            return x * v.x + y * v.y + z * v.z;
        }

        public float DistanceTo(Vector v)
        {
            return (float)Math.Sqrt(Math.Pow(x - v.x, 2) + Math.Pow(y - v.y, 2) + Math.Pow(z - v.z, 2)) / 100f;
        }

        public float XDistanceTo(Vector v)
        {
            return (float)Math.Abs(x - v.x);
        }
    }
}
