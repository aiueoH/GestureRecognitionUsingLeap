using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GR.StructV2;

namespace GR
{
    public class DragDetector : GestureDetector
    {
        private enum HandState
        {
            INVALID,
            CLOSE,
            OPEN,
        }

        private HandState _cHS; // current Drag State
        private HandState _pHS; // pre  Drag State
        private Point3D _bHP; // begine Hand Position
        private Point3D _cHP; // current Hand Position
        private float _dX, _dY, _dZ, _distance;

        public DragDetector()
        {
        }

        public override void Init()
        {
            base.Init();
            _cHS = HandState.INVALID;
            _pHS = HandState.INVALID;
            _bHP = new Point3D(0, 0, 0);
            _cHP = new Point3D(0, 0, 0);
            _dX = 0;
            _dY = 0;
            _dZ = 0;
            _distance = 0;
        }

        protected override void UpdateHandState()
        {
            UpdateDragState();
        }

        protected override void UpdateInfo()
        {
            UpdateData();
            DragInfo info = new DragInfo();
            info.State = State;
            info.HandPos = new Point3D(_cHP);
            info.DeltaPos = new Point3D(_dX, _dY, _dZ);
            info.Distance = _distance;
            Info = info;
        }

        private void UpdateDragState()
        {
            _pHS = _cHS;
            if (Frame.Hands.Count != 1)
                _cHS = HandState.INVALID;
            else
                if (Frame.Hands[0].IsAllFingerNotExtended())
                    _cHS = HandState.CLOSE;
                else
                    _cHS = HandState.OPEN;
        }

        private void UpdateData()
        {
            if (State == GestureState.START)
            {
                Vector v = Frame.Hands[0].PalmPosition;
                _bHP.X = v.x;
                _bHP.Y = v.y;
                _bHP.Z = v.z;
            }
            if (State != GestureState.NULL && State != GestureState.STOP)
            {
                Vector v = Frame.Hands[0].PalmPosition;
                _cHP.X = v.x;
                _cHP.Y = v.y;
                _cHP.Z = v.z;
            }
            _dX = _cHP.X - _bHP.X;
            _dY = _cHP.Y - _bHP.Y;
            _dZ = _cHP.Z - _bHP.Z;
            _distance = _cHP.DistanceTo(_bHP);
        }

        #region Gesture State Translate
        protected override void OnNullState()
        {
            if (_pHS == HandState.OPEN && _cHS == HandState.CLOSE)
                State = GestureState.START;
            else if (
                (_pHS == HandState.INVALID && _cHS == HandState.INVALID) ||
                (_pHS == HandState.INVALID && _cHS == HandState.OPEN) ||
                (_pHS == HandState.INVALID && _cHS == HandState.CLOSE) ||
                (_pHS == HandState.OPEN && _cHS == HandState.INVALID) ||
                (_pHS == HandState.OPEN && _cHS == HandState.OPEN) ||
                (_pHS == HandState.CLOSE && _cHS == HandState.INVALID) ||
                (_pHS == HandState.CLOSE && _cHS == HandState.OPEN) ||
                (_pHS == HandState.CLOSE && _cHS == HandState.CLOSE)
                )
                State = GestureState.NULL;
            else
                throw new UHSTException();
        }

        protected override void OnStartState()
        {
            if (_pHS == HandState.CLOSE && _cHS == HandState.CLOSE)
                State = GestureState.UPDATE;
            else if (
                (_pHS == HandState.CLOSE && _cHS == HandState.INVALID) ||
                (_pHS == HandState.CLOSE && _cHS == HandState.OPEN)
                )
                State = GestureState.STOP;
            else
                throw new UHSTException();
        }

        protected override void OnUpdateState()
        {
            if (_pHS == HandState.CLOSE && _cHS == HandState.CLOSE)
                State = GestureState.UPDATE;
            else if (
                (_pHS == HandState.CLOSE && _cHS == HandState.INVALID) ||
                (_pHS == HandState.CLOSE && _cHS == HandState.OPEN)
                )
                State = GestureState.STOP;
            else
                throw new UHSTException();
        }

        protected override void OnStopState()
        {
            State = GestureState.NULL;
        }
        #endregion
    }
}
