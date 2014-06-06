using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GR.StructV2;

namespace GR
{
    public class TwoIndexDetector : GestureDetector
    {
        public delegate void OnUpdateDelegate(
            GestureState state, 
            Point3D leftIndexPos, 
            Point3D rightIndexPos, 
            float dDX, 
            float dDY, 
            float dDZ, 
            float dD);
        private enum PointState
        {
            INVALID,
            YES,
            NO,
        }
        private PointState _cPS;
        private PointState _pPS;
        private Point3D _bLeft, _bRight;
        private Point3D _cLeft, _cRight;
        private float _bDX, _bDY, _bDZ, _bD;
        private float _dDX, _dDY, _dDZ, _dD;

        public event OnUpdateDelegate OnUpdateEvent;

        public TwoIndexDetector()
        {
        }

        public override void Init()
        {
            base.Init();
            _cPS = PointState.INVALID;
            _pPS = PointState.INVALID;
            _bLeft = new Point3D(0, 0, 0);
            _bRight = new Point3D(0, 0, 0);
            _cLeft = new Point3D(0, 0, 0);
            _cRight = new Point3D(0, 0, 0);
            _bDX = 0;
            _bDY = 0;
            _bDZ = 0;
            _bDZ = 0;
            _dDX = 0;
            _dDY = 0;
            _dDZ = 0;
            _dD = 0;
        }

        public override void Detect(Frame frame)
        {
            Frame = frame;
            UpdatePointState();
            UpdateGestureState();
            UpdateData();
            if (OnUpdateEvent != null)
                OnUpdateEvent(State, _cLeft, _cRight, _dDX, _dDY, _dDZ, _dD);
        }

        private void UpdatePointState()
        {
            _pPS = _cPS;
            if (Frame.Hands.Count != 2 && (Frame.LeftHand == null || Frame.RightHand == null))
                _cPS = PointState.INVALID;
            else
            {
                Hand lH = Frame.LeftHand;
                Hand rH = Frame.RightHand;
                if (lH.IsOnlyIndexFingerStrait() && rH.IsOnlyIndexFingerStrait())
                    _cPS = PointState.YES;
                else
                    _cPS = PointState.NO;
            }
        }

        private void UpdateGestureState()
        {
            State = StateMachine(_pPS, _cPS);
        }

        private void UpdateData()
        {
            if (State == GestureState.START)
            {
                SetIndexPos(Frame.LeftHand, _bLeft);
                SetIndexPos(Frame.RightHand, _bRight);
                _bDX = Math.Abs(_bLeft.X - _bRight.X);
                _bDY = Math.Abs(_bLeft.Y - _bRight.Y);
                _bDZ = Math.Abs(_bLeft.Z - _bRight.Z);
                _bD = _bLeft.DistanceTo(_bRight);
            }
            if (State == GestureState.START || State == GestureState.UPDATE)
            {
                SetIndexPos(Frame.LeftHand, _cLeft);
                SetIndexPos(Frame.RightHand, _cRight);
            }
            _dDX = Math.Abs(_cLeft.X - _cRight.X) - _bDX;
            _dDY = Math.Abs(_cLeft.Y - _cRight.Y) - _bDY;
            _dDZ = Math.Abs(_cLeft.Z - _cRight.Z) - _bDZ;
            _dD = _cLeft.DistanceTo(_cRight) - _bD;
        }

        private void SetIndexPos(Hand hand, Point3D index)
        {
            Vector v = hand.Index.TipPosition;
            index.X = v.x;
            index.Y = v.y;
            index.Z = v.z;
        }

        private GestureState StateMachine(PointState from, PointState to)
        {
            switch (from)
            {
                case PointState.NO:
                    switch (to)
                    {
                        case PointState.NO:
                            return GestureState.NULL;
                        case PointState.YES:
                            return GestureState.START;
                        case PointState.INVALID:
                            return GestureState.NULL;
                    }
                    break;
                case PointState.YES:
                    switch (to)
                    {
                        case PointState.NO:
                            return GestureState.STOP;
                        case PointState.YES:
                            return GestureState.UPDATE;
                        case PointState.INVALID:
                            return GestureState.STOP;
                    }
                    break;
                case PointState.INVALID:
                    switch (to)
                    {
                        case PointState.NO:
                            return GestureState.NULL;
                        case PointState.YES:
                            return GestureState.NULL;
                        case PointState.INVALID:
                            return GestureState.NULL;
                    }
                    break;
            }
            throw new Exception("Unknow point state");
        }
    }
}
