using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GR.StructV2;

namespace GR
{
    public class Detector
    {
        private const int FRAME_BUFFER_SIZE = 5;

        private Leap.Controller _controller = new Leap.Controller();
        private Thread _updateThread = null;
        private FrameBuffer _frameBuffer = null;
        public Frame Frame { get { return _frameBuffer.Frame; } }
        // gesture detector
        private List<GestureDetector> _gestureDetector = new List<GestureDetector>();
        private DragDetector _dragD = new DragDetector();
        private TwoIndexDetector _twoIndexD = new TwoIndexDetector();
        private OneIndexDetector _oneIndexD = new OneIndexDetector();

        public Detector(int bufferSize = FRAME_BUFFER_SIZE)
        {
            _frameBuffer = new FrameBuffer(bufferSize);

            _updateThread = new Thread(UpdateFrame);
            _updateThread.Start();

            //-----
            _gestureDetector.Add(_dragD);
            _gestureDetector.Add(_twoIndexD);
            _gestureDetector.Add(_oneIndexD);
            //AddDragListener(OnDrag);
            _dragD.OnUpdate += OnDrag;
            _oneIndexD.OnUpdate += OnOneIndex;
            _twoIndexD.OnUpdate += OnTwoIndex;
            //AddTwoIndexListener(OnTwoIndex);
            //AddOneIndexListener(OnOneIndex);
        }

        private void UpdateFrame()
        {
            long id = long.MinValue;
            while (true)
            {
                Leap.Frame f = _controller.Frame();
                if (f.Id == id) continue;
                _frameBuffer.Add(new Frame(f));
                Detect();
                id = f.Id;
            }
        }

        private void Detect()
        {
            if (Frame == null)
                return;
            foreach (GestureDetector gd in _gestureDetector)
                gd.Detect(Frame);
        }

        private void OnDrag(object sender, EventArgs args)
        {
            DragInfo info = args as DragInfo;
            if (info != null)
            {
                Console.WriteLine(String.Format("Drag :: state={0} x={1} y={2} z={3} dx={4} dy={5} dz={6} dis={7}",
                    info.State, info.HandPos.X, info.HandPos.Y, info.HandPos.Z,
                    info.DeltaPos.X, info.DeltaPos.Y, info.DeltaPos.Z,
                    info.Distance));
            }
        }

        private void OnOneIndex(object sender, EventArgs args)
        {
            OneIndexInfo info = args as OneIndexInfo;
            if (info != null)
            {
                Console.WriteLine(String.Format("OneIndex :: state={0} x={1} y={2} z={3}", info.State, info.IndexPos.X, info.IndexPos.Y, info.IndexPos.Z));
            }
        }

        private void OnTwoIndex(object sender, EventArgs args)
        {
            TwoIndexInfo info = args as TwoIndexInfo;
            if (info != null)
            {
                if (info.State != GestureState.NULL)
                    Console.WriteLine(String.Format("TwoIndex :: state={0} dDX={1} dDY={2} dDZ={3} dD={4}",
                        info.State,
                        info.DeltaDistanceXYZ.X, info.DeltaDistanceXYZ.Y, info.DeltaDistanceXYZ.Z, info.DeltaDistance));
            }
        }

        private void OnDrag(GestureState state, Point3D handPos, float dX, float dY, float dZ, float distance)
        {
            if (state != GestureState.NULL)
                Console.WriteLine(String.Format("Drag :: state={0} x={1} y={2} z={3} dx={4} dy={5} dz={6} dis={7}", 
                    state, handPos.X, handPos.Y, handPos.Z,
                    dX, dY, dZ, distance));
        }

        private void OnTwoIndex(GestureState state, Point3D leftIndexPos, Point3D rightIndexPos, float dDX, float dDY, float dDZ, float dD)
        {
            if (state != GestureState.NULL)
                Console.WriteLine(String.Format("TwoIndex :: state={0} dDX={1} dDY={2} dDZ={3} dD={4}",
                    state,
                    dDX, dDY, dDZ, dD));
        }

        private void OnOneIndex(GestureState state, Point3D indexPos)
        {
            if (state != GestureState.NULL)
                Console.WriteLine(String.Format("OneIndex :: state={0} x={1} y={2} z={3}", state, indexPos.X, indexPos.Y, indexPos.Z));
        }

        public void AddTwoIndexListener(TwoIndexDetector.OnUpdateDelegate listener)
        {
            _twoIndexD.OnUpdateEvent += listener;
        }
    }
}