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
        public Vector J_Wrist { get; set; }
        public Vector J_Metacarpal { get; set; }
        public Vector J_Proximal { get; set; }
        public Vector J_Intermediate { get; set; }
        public Vector J_Distal { get; set; }
        public Bone B_Metacarpal { get; set; }
        public Bone B_Proximal { get; set; }
        public Bone B_Intermediate { get; set; }
        public Bone B_Distal { get; set; }

        public Finger() {}

        public Finger(Leap.Finger finger, Frame frame, Hand hand)
            : base(finger, frame, hand)
        {
            SetType(finger);
            SetJoints(finger);
            B_Metacarpal = new Bone(finger.Bone(Leap.Bone.BoneType.TYPE_METACARPAL));
            B_Proximal = new Bone(finger.Bone(Leap.Bone.BoneType.TYPE_PROXIMAL));
            B_Intermediate = new Bone(finger.Bone(Leap.Bone.BoneType.TYPE_INTERMEDIATE));
            B_Distal = new Bone(finger.Bone(Leap.Bone.BoneType.TYPE_DISTAL));
        }

        public Bone Bone(Bone.BoneType type)
        {
            switch (type)
            {
                case GR.StructV2.Bone.BoneType.TYPE_DISTAL:
                    return B_Distal;
                case GR.StructV2.Bone.BoneType.TYPE_INTERMEDIATE:
                    return B_Intermediate;
                case GR.StructV2.Bone.BoneType.TYPE_METACARPAL:
                    return B_Metacarpal;
                case GR.StructV2.Bone.BoneType.TYPE_PROXIMAL:
                    return B_Proximal;
                default:
                    throw new Exception("Unknow bone");
            }
        }

        public Bone Bone(int type)
        {
            switch (type)
            {
                case (int)GR.StructV2.Bone.BoneType.TYPE_METACARPAL:
                    return B_Metacarpal;
                case (int)GR.StructV2.Bone.BoneType.TYPE_PROXIMAL:
                    return B_Proximal;
                case (int)GR.StructV2.Bone.BoneType.TYPE_INTERMEDIATE:
                    return B_Intermediate;
                case (int)GR.StructV2.Bone.BoneType.TYPE_DISTAL:
                    return B_Distal;
                default:
                    throw new Exception("Unknow bone");
            }
        }

        public Finger(Finger finger, Frame frame, Hand hand)
            : base(finger, frame, hand)
        {
            Type = finger.Type;
            J_Distal = finger.J_Distal;
            J_Intermediate = finger.J_Intermediate;
            J_Proximal = finger.J_Proximal;
            J_Metacarpal = finger.J_Metacarpal;
            J_Wrist = finger.J_Wrist;
            B_Metacarpal = finger.B_Metacarpal;
            B_Proximal = finger.B_Proximal;
            B_Intermediate = finger.B_Intermediate;
            B_Distal = finger.B_Distal;
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
            //J_Distal = new Vector(finger.JointPosition(Leap.Finger.FingerJoint.JOINT_DIP));
            //J_Intermediate = new Vector(finger.JointPosition(Leap.Finger.FingerJoint.JOINT_TIP));
            //J_Proximal = new Vector(finger.JointPosition(Leap.Finger.FingerJoint.JOINT_PIP));
            //J_Metacarpal = new Vector(finger.JointPosition(Leap.Finger.FingerJoint.JOINT_MCP));
            //J_Wrist = new Vector(finger.Bone(Leap.Bone.BoneType.TYPE_METACARPAL).PrevJoint);

            J_Distal = new Vector(finger.Bone(Leap.Bone.BoneType.TYPE_DISTAL).NextJoint);
            J_Intermediate = new Vector(finger.Bone(Leap.Bone.BoneType.TYPE_INTERMEDIATE).NextJoint);
            J_Proximal = new Vector(finger.Bone(Leap.Bone.BoneType.TYPE_PROXIMAL).NextJoint);
            J_Metacarpal = new Vector(finger.Bone(Leap.Bone.BoneType.TYPE_METACARPAL).NextJoint);
            J_Wrist = new Vector(finger.Bone(Leap.Bone.BoneType.TYPE_METACARPAL).PrevJoint);
        }
    }
}
