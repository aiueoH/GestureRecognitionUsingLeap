using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public class UHSTException : Exception
    {
        public UHSTException() : base ("Unknow hand state translate exception") {}
    }

    public abstract class GestureDetector
    {
        public delegate void OnUpdateDelegate(object sender, EventArgs info);
        public event OnUpdateDelegate OnUpdate;

        public GestureState State { get; set; }
        public GestureInfo Info { get; set; }
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

        public void Detect(Frame frame)
        {
            Frame = frame;
            UpdateHandState();
            UpdateGestureState();
            UpdateInfo();
            Notify();
        }

        protected abstract void UpdateHandState();
        protected abstract void UpdateInfo();
        private void UpdateGestureState()
        {
            switch (State)
            {
                case GestureState.NULL:
                    OnNullState();
                    break;
                case GestureState.START:
                    OnStartState();
                    break;
                case GestureState.UPDATE:
                    OnUpdateState();
                    break;
                case GestureState.STOP:
                    OnStopState();
                    break;
                default:
                    throw new Exception("Unknow gesture state");
            }
        }
        
        private void Notify()
        {
            if (Info.State != GestureState.NULL)
                if (OnUpdate != null)
                    OnUpdate(State, Info);
        }

        protected abstract void OnNullState();
        protected abstract void OnStartState();
        protected abstract void OnUpdateState();
        protected abstract void OnStopState();
    }
}
