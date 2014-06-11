using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using GR.StructV2;

namespace GR
{
    public class Detector
    {
        private const int FRAME_BUFFER_SIZE = 5;

        private Leap.Controller _controller = new Leap.Controller();
        private bool _isRunning = false;
        private Thread _updateThread = null;
        private FrameBuffer _frameBuffer = null;
        public Frame Frame { get { return _frameBuffer.Frame; } }
        // gesture detector
        private List<GestureDetector> _gestureDetector = new List<GestureDetector>();
        private DragDetector _dragD = new DragDetector();
        private TwoIndexDetector _twoIndexD = new TwoIndexDetector();
        private OneIndexDetector _oneIndexD = new OneIndexDetector();
        private ClickDetector _clickD = new ClickDetector();

        public delegate void OnExceptionDelegate(Exception ex);
        public event OnExceptionDelegate OnException;

        public Detector(int bufferSize = FRAME_BUFFER_SIZE)
        {
            _frameBuffer = new FrameBuffer(bufferSize);

            _gestureDetector.Add(_dragD);
            _gestureDetector.Add(_twoIndexD);
            _gestureDetector.Add(_oneIndexD);
            _gestureDetector.Add(_clickD);
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _updateThread = new Thread(QueryFrame);
                _updateThread.Start();
            }
            else
                throw new Exception("Detector is running!");
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void QueryFrame()
        {
            long id = long.MinValue;
            while (_isRunning)
            {
                Leap.Frame f = _controller.Frame();
                if (f.Id == id) continue;
                _frameBuffer.Add(new Frame(f));
                Detect();
                id = f.Id;
            }
            try
            {

            }
            catch (Exception ex)
            {
                if (OnException != null)
                    OnException(ex);
                else
                    throw ex;
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

        public void Detect(Leap.Frame frame)
        {
            _frameBuffer.Add(new Frame(frame));
            Detect();
        }

        private void Detect()
        {
            if (Frame != null)
                foreach (GestureDetector gd in _gestureDetector)
                    gd.Detect(Frame);
        }
    }
}