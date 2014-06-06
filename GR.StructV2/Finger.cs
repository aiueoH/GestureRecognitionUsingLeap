using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
{
    [Serializable]
    public class Finger : Pointable
    {
        public enum FingerType
        {
            THUMB, INDEX, MIDDLE, RING, PINKY
        }
        public FingerType Type { get; set; }
        public Vector Distal { get; set; }
        public Vector Intermediate { get; set; }
        public Vector Proximal { get; set; }
        public Vector Metacarpals { get; set; }

        public Finger() {}

        public Finger(Leap.Finger finger, Frame frame, Hand hand)
            : base(finger, frame, hand)
        {
            SetType(finger);
            SetJoints(finger);
        }

        public Finger(Finger finger, Frame frame, Hand hand)
            : base(finger, frame, hand)
        {
            Type = finger.Type;
            Distal = finger.Distal;
            Intermediate = finger.Intermediate;
            Proximal = finger.Proximal;
            Metacarpals = finger.Metacarpals;
        }

        private void SetType(Leap.Finger finger)
        {
            switch (finger.Type())
            {
                case Leap.Finger.FingerType.TYPE_THUMB:
                    Type = FingerType.THUMB;
                    break;
                case Leap.Finger.FingerType.TYPE_INDEX:
                    Type = FingerType.INDEX;
                    break;
                case Leap.Finger.FingerType.TYPE_MIDDLE:
                    Type = FingerType.MIDDLE;
                    break;
                case Leap.Finger.FingerType.TYPE_RING:
                    Type = FingerType.RING;
                    break;
                case Leap.Finger.FingerType.TYPE_PINKY:
                    Type = FingerType.PINKY;
                    break;
                default:
                    throw new Exception("Unknow finger");
            }
        }

        private void SetJoints(Leap.Finger finger)
        {
            Distal = new Vector(finger.JointPosition(Leap.Finger.FingerJoint.JOINT_DIP));
            Intermediate = new Vector(finger.JointPosition(Leap.Finger.FingerJoint.JOINT_TIP));
            Proximal = new Vector(finger.JointPosition(Leap.Finger.FingerJoint.JOINT_PIP));
            Metacarpals = new Vector(finger.JointPosition(Leap.Finger.FingerJoint.JOINT_MCP));
        }
    }
}
