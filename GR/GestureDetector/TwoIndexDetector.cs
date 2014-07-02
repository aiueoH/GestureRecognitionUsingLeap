using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR
{
    public class TwoIndexDetector : GestureDetector
    {
        private enum HandState
        {
            INVALID,
            STRETCH,
            READY,
        }
        private HandState _cHS;
        private HandState _pHS;
        private Vector _bLeft, _bRight;
        private Vector _cLeft, _cRight;
        private float _bDX, _bDY, _bDZ, _bD;
        private float _dDX, _dDY, _dDZ, _dD;

        public TwoIndexDetector()
        {
        }

        public override void Init()
        {
            base.Init();
            _cHS = HandState.INVALID;
            _pHS = HandState.INVALID;
            _bLeft = new Vector(0, 0, 0);
            _bRight = new Vector(0, 0, 0);
            _cLeft = new Vector(0, 0, 0);
            _cRight = new Vector(0, 0, 0);
            _bDX = 0;
            _bDY = 0;
            _bDZ = 0;
            _bDZ = 0;
            _dDX = 0;
            _dDY = 0;
            _dDZ = 0;
            _dD = 0;
        }

        protected override void UpdateHandState()
        {
            _pHS = _cHS;
            if (Frame.Hands.Count != 2 || (Frame.LeftHand == null || Frame.RightHand == null))
                _cHS = HandState.INVALID;
            else
            {
                Hand lH = Frame.LeftHand;
                Hand rH = Frame.RightHand;
                if (lH.IsOnlyIndexFingerStrait() && rH.IsOnlyIndexFingerStrait())
                    _cHS = HandState.STRETCH;
                else
                    _cHS = HandState.READY;
            }
        }

        protected override void UpdateInfo()
        {
            UpdateData();
            TwoIndexInfo info = new TwoIndexInfo();
            info.State = State;
            info.LeftPos = new Vector(_cLeft);
            info.RightPos = new Vector(_cRight);
            info.DeltaDistanceXYZ = new Vector(_dDX, _dDY, _dDZ);
            info.DeltaDistance = _dD;
            Info = info;
        }

        private void UpdateData()
        {
            if (State == GestureState.START)
            {
                SetIndexPos(Frame.LeftHand, _bLeft);
                SetIndexPos(Frame.RightHand, _bRight);
                _bDX = Math.Abs(_bLeft.x - _bRight.x);
                _bDY = Math.Abs(_bLeft.y - _bRight.y);
                _bDZ = Math.Abs(_bLeft.z - _bRight.z);
                _bD = _bLeft.DistanceTo(_bRight);
            }
            if (State == GestureState.START || State == GestureState.UPDATE)
            {
                SetIndexPos(Frame.LeftHand, _cLeft);
                SetIndexPos(Frame.RightHand, _cRight);
            }
            _dDX = Math.Abs(_cLeft.x - _cRight.x) - _bDX;
            _dDY = Math.Abs(_cLeft.y - _cRight.y) - _bDY;
            _dDZ = Math.Abs(_cLeft.z - _cRight.z) - _bDZ;
            _dD = _cLeft.DistanceTo(_cRight) - _bD;
        }

        private void SetIndexPos(Hand hand, Vector index)
        {
            Vector v = hand.Index.StabilizedTipPosition;
            index.x = v.x;
            index.y = v.y;
            index.z = v.z;
        }

        protected override void OnNullState()
        {
            if (
                (_pHS == HandState.INVALID && _cHS == HandState.INVALID) ||
                (_pHS == HandState.INVALID && _cHS == HandState.READY) ||
                (_pHS == HandState.INVALID && _cHS == HandState.STRETCH) ||
                (_pHS == HandState.READY && _cHS == HandState.READY) ||
                (_pHS == HandState.READY && _cHS == HandState.INVALID) ||
                (_pHS == HandState.STRETCH && _cHS == HandState.READY) ||
                (_pHS == HandState.STRETCH && _cHS == HandState.INVALID)
                )
                State = GestureState.NULL;
            else if (_pHS == HandState.READY && _cHS == HandState.STRETCH)
                State = GestureState.START;
            else
                throw new UnknowHandStateTranslateException();
        }

        protected override void OnStartState()
        {
            if (_pHS == HandState.STRETCH && _cHS == HandState.STRETCH)
                State = GestureState.UPDATE;
            else if (
                (_pHS == HandState.STRETCH && _cHS == HandState.INVALID) ||
                (_pHS == HandState.STRETCH && _cHS == HandState.READY))
                State = GestureState.STOP;
            else
                throw new UnknowHandStateTranslateException();
        }

        protected override void OnUpdateState()
        {
            if (_pHS == HandState.STRETCH && _cHS == HandState.STRETCH)
                State = GestureState.UPDATE;
            else if (
                (_pHS == HandState.STRETCH && _cHS == HandState.READY) ||
                (_pHS == HandState.STRETCH && _cHS == HandState.INVALID)
                )
                State = GestureState.STOP;
            else
                throw new UnknowHandStateTranslateException();
        }

        protected override void OnStopState()
        {
            State = GestureState.NULL;
        }
    }
}
