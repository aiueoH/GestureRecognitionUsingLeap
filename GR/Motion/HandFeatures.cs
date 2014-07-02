using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR
{
    [Serializable]
    public class HandFeatures
    {
        // 五支手指頭
        private const int LENGTH_0 = 5;
        // 指節骨 
        private const int LENGTH_1 = 2;
        /// <summary>
        /// direction [finger, bone]
        /// finger : thumb, index, middle, ring, pinky
        /// join : pro, int, dis
        /// </summary>
        private Vector[][] _fs;
        public Vector this[int finger, int joint] { get { return _fs[finger][joint]; } }
        public int StateCode { get; private set; }

        private HandFeatures()
        {
        }

        public float CosSimilarity(HandFeatures hf)
        {
            float sum = 0;
            for (int i = 0; i < LENGTH_0; i++)
                for (int j = 0; j < LENGTH_1; j++)
                    sum += _fs[i][j].AngleTo(hf._fs[i][j]);
            return sum;
        }

        // 只有兩個都為0才不比對
        public static float CosSimilarity(HandFeatures a, HandFeatures b)
        {
            string code = Convert.ToString(a.StateCode | b.StateCode, 2).PadLeft(LENGTH_0, '0');
            code = new string(code.ToCharArray().Reverse().ToArray());
            float sum = 0;
            int count = 0;
            for (int i = 0; i < LENGTH_0; i++)
                if (code[i] == '1')
                {
                    sum += CosSimilarity(a._fs[i], b._fs[i]);
                    count++;
                }
            return count == 0 ? sum : sum / count / LENGTH_1;
        }

        // 只在相同 statecode 時比對
        //public static float CosSimilarity(HandFeatures a, HandFeatures b)
        //{
        //    if (a.StateCode != b.StateCode)
        //        return float.PositiveInfinity;
        //    string code = Convert.ToString(a.StateCode, 2).PadLeft(LENGTH_0, '0');
        //    code = new string(code.ToCharArray().Reverse().ToArray());
        //    float sum = 0;
        //    int count = 0;
        //    for (int i = 0; i < LENGTH_0; i++)
        //        if (code[i] == '1')
        //        {
        //            sum += CosSimilarity(a._fs[i], b._fs[i]);
        //            count++;
        //        }
        //    return count == 0 ? sum : sum / count / LENGTH_1;
        //}

        public static float CosSimilarity(Hand a, Hand b, int stateCode)
        {
            Matrix mA = a.GetAlignAxisRotationMatrix();
            Matrix mB = b.GetAlignAxisRotationMatrix();
            string code = Convert.ToString(stateCode, 2).PadLeft(5, '0');
            float sum = 0;
            int count = 0;
            if (code[4] == '1')
            {
                sum += CosSimilarity(a.Thumb, b.Thumb, mA, mB);
                count++;
            }
            if (code[3] == '1')
            {
                sum += CosSimilarity(a.Index, b.Index, mA, mB);
                count++;
            }
            if (code[2] == '1')
            {
                sum += CosSimilarity(a.Middle, b.Middle, mA, mB);
                count++;
            }
            if (code[1] == '1')
            {
                sum += CosSimilarity(a.Ring, b.Ring, mA, mB);
                count++;
            }
            if (code[0] == '1')
            {
                sum += CosSimilarity(a.Pinky, b.Pinky, mA, mB);
                count++;
            }
            return count == 0 ? sum : sum / count / LENGTH_1;
        }

        private static float CosSimilarity(Finger a, Finger b, Matrix mA, Matrix mB)
        {
            Vector[] vA = ExtractFeatures(a, mA);
            Vector[] vB = ExtractFeatures(b, mB);
            return CosSimilarity(vA, vB);
        }

        private static float CosSimilarity(Vector[] a, Vector[] b)
        {
            float sum = 0;
            for (int i = 0; i < a.Length; i++)
                sum += a[i].AngleTo(b[i]);
            return sum;
        }

        public static HandFeatures ExtractFeatures(Frame frame)
        {
            if (frame.Hands.Count == 1)
                return ExtractFeatures(frame.Hands[0]);
            else
                return null;
        }

        public static HandFeatures ExtractFeatures(Hand hand)
        {
            Vector[][] fs = new Vector[LENGTH_0][];
            Matrix m = hand.GetAlignAxisRotationMatrix();
            fs[0] = ExtractFeatures(hand.Thumb, m);
            fs[1] = ExtractFeatures(hand.Index, m);
            fs[2] = ExtractFeatures(hand.Middle, m);
            fs[3] = ExtractFeatures(hand.Ring, m);
            fs[4] = ExtractFeatures(hand.Middle, m);
            HandFeatures hf = new HandFeatures();
            hf._fs = fs;
            hf.StateCode = ExtractHandStateCode(hand);
            return hf;
        }

        private static Vector[] ExtractFeatures(Finger f, Matrix matrix)
        {
            Vector[] vs = new Vector[LENGTH_1];
            Vector p = matrix * new Vector(f.J_Proximal);
            Vector m = matrix * new Vector(f.J_Metacarpal);
            Vector i = matrix * new Vector(f.J_Intermediate);
            Vector d = matrix * new Vector(f.J_Distal);            
            vs[0] = (p - m).Normalize();
            vs[1] = (i - p).Normalize();
            //vs[2] = (d- i).Normalize();
            return vs;
        }

        public static int ExtractHandStateCode(Hand hand)
        {
            int code = 0;
            if (hand.Thumb.IsStretch())
                code += 1;
            if (hand.Index.IsStretch())
                code += 2;
            if (hand.Middle.IsStretch())
                code += 4;
            if (hand.Ring.IsStretch())
                code += 8;
            if (hand.Pinky.IsStretch())
                code += 16;
            return code;
        }
    }
}
