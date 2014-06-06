using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR
{
    public class OneIndexDetector : GestureDetector
    {
        public delegate void OnUpdateDelegate(GestureState state, Point3D indexPos);
        public enum HandState
        {
            STRETCH,
            INVALID,
        }
        private HandState _cHS;
        private HandState _pHS;
        private Point3D _indexPos;

        public event OnUpdateDelegate OnUpdateEvent;

        public OneIndexDetector()
        {
        }

        public override void Init()
        {
            base.Init();
            _cHS = HandState.INVALID;
            _pHS = HandState.STRETCH;
            _indexPos = new Point3D(0, 0, 0);
        }

        public override void Detect(StructV2.Frame frame)
        {
            Frame = frame;
            UpdateHandState();
            UpdateGestureState();
            UpdateData();
            if (OnUpdateEvent != null)
                OnUpdateEvent(State, _indexPos);
        }

        public void UpdateHandState()
        {
            _pHS = _cHS;
            if (Frame.Hands.Count == 1 && Frame.Hands[0].IsOnlyIndexFingerStrait())
                _cHS = HandState.STRETCH;
            else
                _cHS = HandState.INVALID;
        }

        public void UpdateGestureState()
        {
            State = StateMachine(_pHS, _cHS);
        }

        public void UpdateData()
        {
            if (State == GestureState.START || State == GestureState.UPDATE)
            {
                _indexPos.X = Frame.Hands[0].Index.TipPosition.x;
                _indexPos.Y = Frame.Hands[0].Index.TipPosition.y;
                _indexPos.Z = Frame.Hands[0].Index.TipPosition.z;
            }
        }

        private GestureState StateMachine(HandState from, HandState to)
        {
            switch (State)
            {
                case GestureState.NULL:
                    switch (from)
                    {
                        case HandState.INVALID:
                            switch (to)
                            {
                                case HandState.INVALID:
                                    return GestureState.NULL;
                                case HandState.STRETCH:
                                    return GestureState.START;
                            }
                            break;
                        case HandState.STRETCH:
                            switch (to)
                            {
                                case HandState.INVALID:
                                    return GestureState.NULL;
                                case HandState.STRETCH:
                                    return GestureState.START;
                            }
                            break;
                    }
                    break;
                case GestureState.START:
                    switch (from)
                    {
                        case HandState.STRETCH:
                            switch (to)
                            {
                                case HandState.INVALID:
                                    return GestureState.STOP;
                                case HandState.STRETCH:
                                    return GestureState.UPDATE;
                            }
                            break;
                    }
                    break;
                case GestureState.UPDATE:
                    switch (from)
                    {
                        case HandState.STRETCH:
                            switch (to)
                            {
                                case HandState.INVALID:
                                    return GestureState.STOP;
                                case HandState.STRETCH:
                                    return GestureState.UPDATE;
                            }
                            break;
                    }
                    break;
                case GestureState.STOP:
                    return GestureState.NULL;
            }
            throw new Exception("Unknow hand state");
        }

    }
}
