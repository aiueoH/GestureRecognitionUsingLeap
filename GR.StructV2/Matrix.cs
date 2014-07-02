using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
{
    public class Matrix
    {
        private const int SIZE = 3;
        private float[,] _es;

        public Matrix(float e)
        {
            _es = new float[SIZE, SIZE];
            for (int i = 0; i < SIZE; i++)
                for (int j = 0; j < SIZE; j++)
                    _es[i, j] = e;
        }

        public float this[int r, int c]
        {
            get
            {
                return _es[r, c];
            }
            set
            {
                _es[r, c] = value;
            }
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            float x = v.x * m[0, 0] + v.y * m[1, 0] + v.z * m[2, 0];
            float y = v.x * m[0, 1] + v.y * m[1, 1] + v.z * m[2, 1];
            float z = v.x * m[0, 2] + v.y * m[1, 2] + v.z * m[2, 2];
            return new Vector(x, y, z);
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            Matrix m = new Matrix(0f);
            for (int i = 0; i < SIZE; i++)
                for (int j = 0; j < SIZE; j++)
                    for (int k = 0; k < SIZE; k++)
                        m[i, j] += a[i, k] * b[k, j];
            return m;
        }

        public static Matrix RotationX(float radian)
        {
            Matrix m = new Matrix(0f);
            m[0, 0] = 1f;
            m[1, 1] = (float)Math.Cos(radian);
            m[1, 2] = -(float)Math.Sin(radian);
            m[2, 1] = (float)Math.Sin(radian);
            m[2, 2] = (float)Math.Cos(radian);
            return m;
        }

        public static Matrix RotationY(float radian)
        {
            Matrix m = new Matrix(0f);
            m[0, 0] = (float)Math.Cos(radian);
            m[0, 2] = (float)Math.Sin(radian);
            m[1, 1] = 1f;
            m[2, 0] = -(float)Math.Sin(radian);
            m[2, 2] = (float)Math.Cos(radian);
            return m;
        }

        public static Matrix RotationZ(float radian)
        {
            Matrix m = new Matrix(0f);
            m[0, 0] = (float)Math.Cos(radian);
            m[0, 1] = -(float)Math.Sin(radian);
            m[1, 0] = (float)Math.Sin(radian);
            m[1, 1] = (float)Math.Cos(radian);
            m[2, 2] = 1f;
            return m;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < SIZE; i++)
                    result += String.Format("[{0}, {1}, {2}]", _es[i, 0], _es[i, 1], _es[i, 2]);
            return result;
        }
    }
}
