using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR
{
    public static class Utility
    {
        public static bool IsAllFingerNotExtended(this Hand hand)
        {
            bool b = true;
            foreach (Finger finger in hand.Fingers)
                if (finger.IsExtended)
                    b = false;
            return b;
        }

        public static bool IsOnlyIndexFingerStrait(this Hand hand)
        {
            if (hand.Thumb.IsCurl() &&
                !hand.Index.IsCurl() &&
                hand.Middle.IsCurl() &&
                hand.Ring.IsCurl() &&
                hand.Pinky.IsCurl())
                return true;
            else
                return false;
        }

        public static bool IsCurl(this Finger finger)
        {
            bool b = true;
            if (finger.IsExtended)
                b = false;
            return b;
        }
    }
}
