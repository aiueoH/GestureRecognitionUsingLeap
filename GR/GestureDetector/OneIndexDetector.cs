using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR
{
    public class OneIndexDetector : GestureDetector
    {
        public enum HandState
        {
            STRETCH,
            INVALID,
        }
        private HandState _cHS;
        private HandState _pHS;
        private Point3D _indexPos;
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

        protected override void UpdateHandState()
        {
            _pHS = _cHS;
            if (Frame.Hands.Count == 1 && Frame.Hands[0].IsOnlyIndexFingerStrait())
                _cHS = HandState.STRETCH;
            else
                _cHS = HandState.INVALID;
        }

        protected override void UpdateInfo()
        {
            UpdateData();
            OneIndexInfo info = new OneIndexInfo();
            info.State = State;
            info.IndexPos = new Point3D(_indexPos);
            Info = info;
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

        #region Gesture state translate
        protected override void OnNullState()
        {
            if (
                (_pHS == HandState.INVALID && _cHS == HandState.INVALID) ||
                (_pHS == HandState.STRETCH && _cHS == HandState.INVALID)
                )
                State = GestureState.NULL;
            else if (
                (_pHS == HandState.INVALID && _cHS == HandState.STRETCH) ||
                (_pHS == HandState.STRETCH && _cHS == HandState.STRETCH)
                )
            {
                State = GestureState.START;
            }
            else
                throw new UnknowHandStateTranslateException();
        }

        protected override void OnStartState()
        {
            if (_pHS == HandState.STRETCH && _cHS == HandState.STRETCH)
                State = GestureState.UPDATE;
            else if (_pHS == HandState.STRETCH && _cHS == HandState.INVALID)
            {
                State = GestureState.STOP;
            }
            else
                throw new UnknowHandStateTranslateException();
        }

        protected override void OnUpdateState()
        {
            if (_pHS == HandState.STRETCH && _cHS == HandState.STRETCH)
                State = GestureState.UPDATE;
            else if (_pHS == HandState.STRETCH && _cHS == HandState.INVALID)
            {
                State = GestureState.STOP;
            }
            else
                throw new UnknowHandStateTranslateException();
        }

        protected override void OnStopState()
        {
            State = GestureState.NULL;
        }
        #endregion
    }
}
