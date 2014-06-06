using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
{
    [Serializable]
    public class Tool : Pointable
    {
        public Tool() {}

        public Tool(Leap.Tool tool, Frame frame, Hand hand)
            : base(tool, frame, hand)
        {

        }

        public Tool(Tool tool, Frame frame, Hand hand)
            : base(tool, frame, hand)
        {
        }
    }
}
