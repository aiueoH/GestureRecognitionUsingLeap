using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.Struct
{
    [Serializable]
    public class Pointable
    {
        //public Vector Direction { get; }

        // 用來取得 tracjection 用, 判斷是否撈取過
        internal bool IsTraveled { get; set; }
        // Leap 原有屬性
        public Frame Frame { get; set; }
        public Hand Hand { get; set; }
        public int Id { get; set; }
        public bool IsFinger { get; set; }
        public bool IsTool { get; set; }
        public bool IsValid { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float TimeVisible { get; set; }
        public Vector TipPosition { get; set; }
        public Vector TipVelocity { get; set; }
        public Vector StabilizedTipPosition { get; set; }
        //public float TouchDistance { get; }
        //public Pointable.Zone TouchZone { get; }

        public Pointable(Leap.Pointable pointable, Frame frame, Hand hand)
        {
            IsTraveled = false;

            this.Frame = frame;
            this.Hand = hand;
            this.Id = pointable.Id;
            this.IsFinger = pointable.IsFinger;
            this.IsTool = pointable.IsTool;
            this.IsValid = pointable.IsValid;
            this.Length = pointable.Length;
            this.Width = pointable.Width;
            this.TimeVisible = pointable.TimeVisible;
            this.TipPosition = new Vector(pointable.TipPosition);
            this.TipVelocity = new Vector(pointable.TipVelocity);
            this.StabilizedTipPosition = new Vector(pointable.StabilizedTipPosition);
        }

        public Pointable(Pointable pointable, Frame frame, Hand hand)
        {
            IsTraveled = false;

            this.Frame = frame;
            this.Hand = hand;
            this.Id = pointable.Id;
            this.IsFinger = pointable.IsFinger;
            this.IsTool = pointable.IsTool;
            this.IsValid = pointable.IsValid;
            this.Length = pointable.Length;
            this.Width = pointable.Width;
            this.TimeVisible = pointable.TimeVisible;
            this.TipPosition = new Vector(pointable.TipPosition);
            this.TipVelocity = new Vector(pointable.TipVelocity);
            this.StabilizedTipPosition = new Vector(pointable.StabilizedTipPosition);
        }
    }
}