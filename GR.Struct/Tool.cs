using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.Struct
{
    [Serializable]
    public class Tool : Pointable
    {
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
