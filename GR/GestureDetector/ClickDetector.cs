using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GR.StructV2;

namespace GR
{
    public class ClickDetector : GestureDetector
    {
        public enum HandState
        {
            STABLE,
            UP,
            DOWN,
            INVALID,
        }
        
        private const float STABLE_DIST = 0.005f;
        private const float DOWN_Y_DIST = -2f;
        private const float UP_Y_DIST = 2f;
        private const long AMBIGOUS_DURATION = 500 * 1000; // ms * 1000 
        private const long STABLE_DURATION = 100 * 1000; // ms * 1000

        private bool _isOneStrait = false;

        private HandState _cHS = HandState.INVALID;
        private HandState _pHS = HandState.INVALID;

        private long _ambiguousTimeStamp = long.MinValue;
        private bool _isAmbiguos = false;

        private long _goingToStableTimeStamp = long.MinValue;
        private bool _isGoingToStable = false;

        private Vector _preIndexPos;
        private Vector _stablePos = new Vector(0, 0, 0);
        private Vector _stableDir = new Vector(0, 0, 0);

        public override void Init()
        {
            base.Init();
        }

        protected override void UpdateHandState()
        {
            _pHS = _cHS;
            _cHS = HandState.INVALID;
            if (Frame.Hands.Count == 1 && Frame.Hands[0].IsOnlyIndexFingerStrait())
            {
                Vector currentIndexPos = Frame.Hands[0].Index.StabilizedTipPosition;
                if (_preIndexPos != null)
                {
                    // STABLE
                    if (_isOneStrait && currentIndexPos.DistanceTo(_preIndexPos) < STABLE_DIST)
                    {
                        if (!_isGoingToStable)
                        {
                            _isGoingToStable = true;
                            _goingToStableTimeStamp = Frame.Timestamp;
                        }
                        else if (Frame.Timestamp - _goingToStableTimeStamp > STABLE_DURATION)
                            _cHS = HandState.STABLE;
                    }
                    else
                        _isGoingToStable = false;
                    // DOWN
                    if (currentIndexPos.y - _preIndexPos.y < DOWN_Y_DIST)
                        _cHS = HandState.DOWN;
                    // UP
                    if (currentIndexPos.y - _preIndexPos.y > UP_Y_DIST)
                        _cHS = HandState.UP;
                }
                _preIndexPos = currentIndexPos;
                _isOneStrait = true;
            }
            else
                _isOneStrait = false;
        }

        protected override void UpdateInfo()
        {
            ClickInfo info = new ClickInfo();
            info.State = State;
            info.StablePos = new Vector(_stablePos);
            info.StableDir = new Vector(_stableDir);
            Info = info;
        }

        protected override void OnNullState()
        {
            if (_cHS == HandState.STABLE)
                State = GestureState.START;
            else
                State = GestureState.NULL;
        }

        protected override void OnStartState()
        {
            _stablePos = new Vector(Frame.Hands[0].Index.TipPosition);
            _stableDir = new Vector(Frame.Hands[0].Index.Direction);

            bool isTimeOut = _isAmbiguos && (Frame.Timestamp - _ambiguousTimeStamp) > AMBIGOUS_DURATION;
            _isAmbiguos = false;

            if (_cHS == HandState.DOWN)
                State = GestureState.UPDATE;
            else if (
                (_pHS == HandState.STABLE && _cHS == HandState.STABLE) ||
                (_pHS == HandState.INVALID && _cHS == HandState.STABLE)
                )
                State = GestureState.START;
            else if (_pHS == HandState.INVALID && _cHS == HandState.INVALID && !isTimeOut)
            {
                State = GestureState.START;
                _isAmbiguos = true;
            }
            else if (_pHS == HandState.STABLE && _cHS == HandState.INVALID)
            {
                _ambiguousTimeStamp = Frame.Timestamp;
                _isAmbiguos = true;
            }
            else if (
                (_pHS == HandState.STABLE && _cHS == HandState.UP) ||
                (_pHS == HandState.INVALID && _cHS == HandState.UP) ||
                (_pHS == HandState.INVALID && _cHS == HandState.INVALID && isTimeOut)
                )
                State = GestureState.NULL;
            else
                throw new UnknowHandStateTranslateException();
        }

        protected override void OnUpdateState()
        {
            bool isTimeOut = _isAmbiguos && (Frame.Timestamp - _ambiguousTimeStamp) > AMBIGOUS_DURATION;
            _isAmbiguos = false;

            if (_pHS == HandState.DOWN && _cHS == HandState.DOWN)
                State = GestureState.UPDATE;
            else if (
                (_pHS == HandState.INVALID && _cHS == HandState.INVALID && !isTimeOut) ||
                (_pHS == HandState.INVALID && _cHS == HandState.STABLE && !isTimeOut) ||
                (_pHS == HandState.INVALID && _cHS == HandState.DOWN && !isTimeOut) ||
                (_pHS == HandState.STABLE && _cHS == HandState.INVALID && !isTimeOut) ||
                (_pHS == HandState.STABLE && _cHS == HandState.STABLE && !isTimeOut) ||
                (_pHS == HandState.STABLE && _cHS == HandState.DOWN && !isTimeOut)
                )
            {
                State = GestureState.UPDATE;
                _isAmbiguos = true;
            }
            else if (
                (_pHS == HandState.DOWN && _cHS == HandState.INVALID) ||
                (_pHS == HandState.DOWN && _cHS == HandState.STABLE)
                )
            {
                State = GestureState.UPDATE;
                _ambiguousTimeStamp = Frame.Timestamp;
                _isAmbiguos = true;
            }
            else if (
                (_pHS == HandState.INVALID && _cHS == HandState.INVALID && isTimeOut) ||
                (_pHS == HandState.INVALID && _cHS == HandState.STABLE && isTimeOut) ||
                (_pHS == HandState.STABLE && _cHS == HandState.INVALID && isTimeOut) ||
                (_pHS == HandState.STABLE && _cHS == HandState.STABLE && isTimeOut)
                )
                State = GestureState.NULL;
            else if (
                (_pHS == HandState.DOWN && _cHS == HandState.UP) ||
                (_pHS == HandState.INVALID && _cHS == HandState.UP) ||
                (_pHS == HandState.STABLE && _cHS == HandState.UP)
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
