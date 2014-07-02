using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR
{
    public static class Utility
    {
        public static float RadianToDegree(this float radian)
        {
            return radian * (float)(180f / Math.PI);
        }
        public static float DegreeToRadian(this float degree)
        {
            return degree * (float)(Math.PI / 180f);
        }

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
            if (!hand.Thumb.IsExtended &&
                hand.Index.IsExtended &&
                !hand.Middle.IsExtended &&
                !hand.Ring.IsExtended &&
                !hand.Pinky.IsExtended)
                return true;
            else
                return false;
        }

        public static float T_STRETCH = (490f).DegreeToRadian();
        public static bool IsStretch(this Finger finger)
        {
            if (finger.Type != Finger.FingerType.THUMB)
            {
                Vector MW = finger.J_Wrist - finger.J_Metacarpal;
                Vector MP = finger.J_Proximal - finger.J_Metacarpal;
                float M = MW.AngleTo(MP);
                Vector PM = finger.J_Metacarpal - finger.J_Proximal;
                Vector PI = finger.J_Intermediate - finger.J_Proximal;
                float P = PM.AngleTo(PI);
                Vector IP = finger.J_Proximal - finger.J_Intermediate;
                Vector ID = finger.J_Distal - finger.J_Intermediate;
                float I = IP.AngleTo(ID);
                float sum = M + P + I;
                if (sum > T_STRETCH)
                    return true;
                else
                    return false;
            }
            else
            {
                Vector PM = finger.J_Metacarpal - finger.J_Proximal;
                Vector PI = finger.J_Intermediate - finger.J_Proximal;
                float P = PM.AngleTo(PI);
                Vector IP = finger.J_Proximal - finger.J_Intermediate;
                Vector ID = finger.J_Distal - finger.J_Intermediate;
                float I = IP.AngleTo(ID);
                float sum = P + I;
                if (sum > T_STRETCH * 2f / 3f)
                    return true;
                else
                    return false;
            }
        }

        public static float T_CURL = (370f).DegreeToRadian();
        public static float T_CURL_THUMB = (300f).DegreeToRadian();
        public static bool IsCurl(this Finger finger)
        {
            if (finger.Type != Finger.FingerType.THUMB)
            {
                Vector MW = finger.J_Wrist - finger.J_Metacarpal;
                Vector MP = finger.J_Proximal - finger.J_Metacarpal;
                float M = MW.AngleTo(MP);
                Vector PM = finger.J_Metacarpal - finger.J_Proximal;
                Vector PI = finger.J_Intermediate - finger.J_Proximal;
                float P = PM.AngleTo(PI);
                Vector IP = finger.J_Proximal - finger.J_Intermediate;
                Vector ID = finger.J_Distal - finger.J_Intermediate;
                float I = IP.AngleTo(ID);
                float sum = M + P + I;
                if (sum < T_CURL)
                    return true;
                else
                    return false;
            }
            else
            {
                Vector PM = finger.J_Metacarpal - finger.J_Proximal;
                Vector PI = finger.J_Intermediate - finger.J_Proximal;
                float P = PM.AngleTo(PI);
                Vector IP = finger.J_Proximal - finger.J_Intermediate;
                Vector ID = finger.J_Distal - finger.J_Intermediate;
                float I = IP.AngleTo(ID);
                float sum = P + I;
                if (sum < T_CURL_THUMB)
                    return true;
                else
                    return false;
            }
        }
    }
}
