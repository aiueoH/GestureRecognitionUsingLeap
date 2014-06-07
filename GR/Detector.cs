using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public delegate void TTT(object sender, EventArgs info);
        public event TTT OnUpdate;

        public Detector(int bufferSize = FRAME_BUFFER_SIZE)
        {
            _frameBuffer = new FrameBuffer(bufferSize);

            _gestureDetector.Add(_dragD);
            _gestureDetector.Add(_twoIndexD);
            _gestureDetector.Add(_oneIndexD);

            _updateThread = new Thread(QueryFrame);
            _updateThread.Start();
        }

        public void Close()
        {
            _updateThread.Abort();
        }

        private void QueryFrame()
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

        public void AddListener(String detectorName, GestureDetector.OnUpdateDelegate listener)
        {
            foreach (GestureDetector gd in _gestureDetector)
                if (gd.GetType().Name == detectorName)
                {
                    gd.OnUpdate += listener;
                    return;
                }
            throw new Exception(String.Format("Can't find detector : {0}", detectorName));
        }

        private void Detect()
        {
            if (Frame != null)
                foreach (GestureDetector gd in _gestureDetector)
                    gd.Detect(Frame);
        }
    }
}