using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.Struct
{
    [Serializable]
    public class Finger : Pointable
    {
        public Finger(Leap.Finger finger, Frame frame, Hand hand)
            : base(finger, frame, hand)
        {
        }

        public Finger(Finger finger, Frame frame, Hand hand)
            : base(finger, frame, hand)
        {
        }
    }
}