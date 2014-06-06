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
        public delegate void OnUpdateDelegate(GestureState state, Point3D handPos, float dX, float dY, float dZ, float distance);
        private enum DragState
        {
            INVALID,
            CLENCH,
            FREE,
        }

        private DragState _cDS; // current Drag State
        private DragState _pDS; // pre  Drag State
        private Point3D _bHP; // begine Hand Position
        private Point3D _cHP; // current Hand Position
        private float _dX, _dY, _dZ, _distance;

        public event OnUpdateDelegate OnUpdateEvent;

        public DragDetector()
        {
        }

        public override void Init()
        {
            base.Init();
            _cDS = DragState.INVALID;
            _pDS = DragState.INVALID;
            _bHP = new Point3D(0, 0, 0);
            _cHP = new Point3D(0, 0, 0);
            _dX = 0;
            _dY = 0;
            _dZ = 0;
            _distance = 0;
        }

        public override void Detect(Frame frame)
        {
            Frame = frame;
            UpdateDragState();
            UpdateGestureState();
            UpdateData();
            if (OnUpdateEvent != null)
                OnUpdateEvent(State, _cHP, _dX, _dY, _dZ, _distance);
        }

        private void UpdateGestureState()
        {
            State = StateMachine(_pDS, _cDS);
        }

        private void UpdateDragState()
        {
            _pDS = _cDS;
            if (Frame.Hands.Count != 1)
                _cDS = DragState.INVALID;
            else
                if (Frame.Hands[0].IsAllFingerNotExtended())
                    _cDS = DragState.CLENCH;
                else
                    _cDS = DragState.FREE;
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

        private GestureState StateMachine(DragState from, DragState to)
        {
            switch (from)
            {
                case DragState.FREE:
                    switch (to)
                    {
                        case DragState.FREE:
                            return GestureState.NULL;
                        case DragState.CLENCH:
                            return GestureState.START;
                        case DragState.INVALID:
                            return GestureState.NULL;
                    }
                    break;
                case DragState.CLENCH:
                    switch (to)
                    {
                        case DragState.FREE:
                            return GestureState.STOP;
                        case DragState.CLENCH:
                            return GestureState.UPDATE;
                        case DragState.INVALID:
                            return GestureState.STOP;
                    }
                    break;
                case DragState.INVALID:
                    switch (to)
                    {
                        case DragState.FREE:
                            return GestureState.NULL;
                        case DragState.CLENCH:
                            return GestureState.NULL;
                        case DragState.INVALID:
                            return GestureState.NULL;
                    }
                    break;
            }
            throw new Exception("Unknow drag state");
        }
    }
}
