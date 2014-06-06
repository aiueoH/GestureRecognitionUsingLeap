using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GR.StructV2;

namespace GR
{
    public enum GestureState
    {
        NULL,
        START,
        UPDATE,
        STOP,
    }

    public abstract class GestureDetector
    {
        public GestureState State { get; set; }
        protected Frame Frame { get; set; }

        public GestureDetector()
        {
            Init();
        }

        public virtual void Init()
        {
            Frame = null;
            State = GestureState.NULL;
        }
        public abstract void Detect(Frame frame);
    }
}
