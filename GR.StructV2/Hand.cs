using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
{
    [Serializable]
    public class Hand
    {

        // 用來取得 tracjection 用, 判斷是否撈取過
        internal bool IsTraveled { get; set; }
        // Leap 原有屬性
        public Frame Frame { get; set; }
        public List<Pointable> Pointables { get; set; }
        public List<Finger> Fingers { get; set; }
        public Finger GetFinger(int index)
        {
            foreach (Finger f in Fingers)
                if ((int)f.Type == index)
                    return f;
            return null;
        }
        public List<Tool> Tools { get; set; }
        public int Id { get; set; }
        public float PalmWidth { get; set; }
        public Vector PalmPosition { get; set; }
        public Vector PalmVelocity { get; set; }
        public Vector SphereCenter { get; set; }
        public float SphereRadius { get; set; }
        public Vector StabilizedPalmPosition { get; set; }
        public float TimeVisible { get; set; }
        public Vector Direction { get; set; }
        public Vector PalmNormal { get; set; }
        public bool IsLeft { get; set; }
        public bool IsRight { get; set; }
        public float GrabStrength { get; set; }
        public float PinchStrength { get; set; }

        public Finger Thumb { get; set; }
        public Finger Index { get; set; }
        public Finger Middle { get; set; }
        public Finger Ring { get; set; }
        public Finger Pinky { get; set; }

        public Hand() {}

        public Hand(Leap.Hand hand, Frame frame)
        {
            IsTraveled = false;
            Frame = frame;
            Id = hand.Id;
            PalmWidth = hand.PalmWidth;
            IsLeft = hand.IsLeft;
            IsRight = hand.IsRight;
            Pointables = new List<Pointable>();
            Fingers = new List<Finger>();
            Tools = new List<Tool>();
            foreach (Leap.Finger lf in hand.Fingers)
            {
                Finger f = new Finger(lf, frame, this);
                Fingers.Add(f);
                Pointables.Add(f);
                switch (f.Type)
                {
                    case Finger.FingerType.THUMB:
                        Thumb = f;
                        break;
                    case Finger.FingerType.INDEX:
                        Index = f;
                        break;
                    case Finger.FingerType.MIDDLE:
                        Middle = f;
                        break;
                    case Finger.FingerType.RING:
                        Ring = f;
                        break;
                    case Finger.FingerType.PINKY:
                        Pinky = f;
                        break;
                    default:
                        throw new Exception("Unknow finger");
                }
            }

            foreach (Leap.Tool lt in hand.Tools)
            {
                Tool t = new Tool(lt, frame, this);
                Tools.Add(t);
                Pointables.Add(t);
            }
            PalmPosition = new Vector(hand.PalmPosition);
            PalmVelocity = new Vector(hand.PalmVelocity);
            PalmNormal = new Vector(hand.PalmNormal);
            SphereCenter = new Vector(hand.SphereCenter);
            Direction = new Vector(hand.Direction);
            SphereRadius = hand.SphereRadius;
            StabilizedPalmPosition = new Vector(hand.StabilizedPalmPosition);
            TimeVisible = hand.TimeVisible;
            GrabStrength = hand.GrabStrength;
            PinchStrength = hand.PinchStrength;
        }

        public Hand(Hand hand, Frame frame)
        {
            IsTraveled = false;

            Frame = frame;
            Id = hand.Id;
            PalmWidth = hand.PalmWidth;
            Pointables = new List<Pointable>();
            Fingers = new List<Finger>();
            Tools = new List<Tool>();
            foreach (Finger f in hand.Fingers)
            {
                Finger nf = new Finger(f, frame, this);
                Fingers.Add(nf);
                Pointables.Add(nf);
                switch (f.Type)
                {
                    case Finger.FingerType.THUMB:
                        Thumb = f;
                        break;
                    case Finger.FingerType.INDEX:
                        Index = f;
                        break;
                    case Finger.FingerType.MIDDLE:
                        Middle = f;
                        break;
                    case Finger.FingerType.RING:
                        Ring = f;
                        break;
                    case Finger.FingerType.PINKY:
                        Pinky = f;
                        break;
                    default:
                        throw new Exception("Unknow finger");
                }
            }

            foreach (Tool t in hand.Tools)
            {
                Tool nt = new Tool(t, frame, this);
                Tools.Add(nt);
                Pointables.Add(nt);
            }
            PalmPosition = new Vector(hand.PalmPosition);
            PalmVelocity = new Vector(hand.PalmVelocity);
            PalmNormal = new Vector(hand.PalmNormal);
            SphereCenter = new Vector(hand.SphereCenter);
            Direction = new Vector(hand.Direction);
            SphereRadius = hand.SphereRadius;
            StabilizedPalmPosition = new Vector(hand.StabilizedPalmPosition);
            TimeVisible = hand.TimeVisible;
            GrabStrength = hand.GrabStrength;
            PinchStrength = hand.PinchStrength;
        }

        public Pointable Pointable(int id)
        {
            foreach (Pointable p in Pointables)
                if (p.Id == id)
                    return p;
            return null;
        }

        public Matrix GetAlignAxisRotationMatrix()
        {
            Matrix m = new Matrix(0f);
            Vector forward = new Vector(Direction);
            Vector up = -new Vector(PalmNormal);
            float angle;
            // 找出手掌forward與世界forwar的兩夾角，並產生旋轉矩陣
            angle = new Vector(0f, forward.y, forward.z).AngleTo(Vector.Forward);
            if (forward.y < 0)
                angle = -angle;
            m = Matrix.RotationX(angle);
            forward = m * forward;
            angle = new Vector(forward.x, 0f, forward.z).AngleTo(Vector.Forward);
            if (forward.x > 0)
                angle = -angle;
            m = m * Matrix.RotationY(angle);
            // 找出手掌up與世界up的夾角，並產生旋轉矩陣
            up = m * up;
            angle = new Vector(up.x, up.y, 0f).AngleTo(Vector.Up);
            if (up.x > 0)
                angle = -angle;
            m = m * Matrix.RotationZ(angle);
            return m;
        }
    }
}
