using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
{
    [Serializable]
    public class Bone
    {
        public enum BoneType
        {
            TYPE_METACARPAL,
            TYPE_PROXIMAL,
            TYPE_INTERMEDIATE,
            TYPE_DISTAL,
        }
        public BoneType Type { get; set; }
        //public Vector Center { get; set; }
        public Vector Direction { get; set; }
        public bool IsValid { get; set; }
        public float Length { get; set; }
        public Vector NextJoint { get; set; }
        public Vector PrevJoint { get; set; }
        public float Width { get; set; }

        public Bone(Leap.Bone bone)
        {
            SetType(bone.Type);
            //Center = new Vector(bone.Center);
            Direction = new Vector(bone.Direction);
            IsValid = bone.IsValid;
            Length = bone.Length;
            NextJoint = new Vector(bone.NextJoint);
            PrevJoint = new Vector(bone.PrevJoint);
            Width = bone.Width;
        }

        public Bone(Bone bone)
        {
            Type = bone.Type;
            //Center = new Vector(bone.Center);
            Direction = new Vector(bone.Direction);
            IsValid = bone.IsValid;
            Length = bone.Length;
            NextJoint = new Vector(bone.NextJoint);
            PrevJoint = new Vector(bone.PrevJoint);
            Width = bone.Width;
        }

        private void SetType(Leap.Bone.BoneType t)
        {
            switch (t)
            {
                case Leap.Bone.BoneType.TYPE_DISTAL:
                    Type = BoneType.TYPE_DISTAL;
                    break;
                case Leap.Bone.BoneType.TYPE_INTERMEDIATE:
                    Type = BoneType.TYPE_INTERMEDIATE;
                    break;
                case Leap.Bone.BoneType.TYPE_METACARPAL:
                    Type = BoneType.TYPE_METACARPAL;
                    break;
                case Leap.Bone.BoneType.TYPE_PROXIMAL:
                    Type = BoneType.TYPE_PROXIMAL;
                    break;
                default:
                    throw new Exception("Unknow bone");
            }
        }

    }
}
