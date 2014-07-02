using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR.FramePlayer
{
    public class Player
    {
        public enum Mode
        {
            Live,
            Clip,
        }

        private Leap.Controller _controller = new Leap.Controller();
        private Mode _mode = Mode.Live;
        private Frame _currentFrame = null;
        private bool _isRecording = false;
        private List<Frame> _clipFrames;
        private List<Frame> _recordFrames;
        private int _clipFrameIndex;

        public bool IsRecording { get { return _isRecording; } }
        public bool IsNormalize { get; set; }
        public Frame Frame { get { return _currentFrame; } }
        
        public void LiveMode()
        {
            _mode = Mode.Live;
        }

        public void ClipMode()
        {
            _mode = Mode.Clip;
        }

        public void StartRecord()
        {
            if (_isRecording)
                throw new Exception("Is recording now.");
            _recordFrames = new List<Frame>();
            _isRecording = true;
        }

        public List<Frame> StopRecord()
        {
            if (!_isRecording)
                throw new Exception("Is not recording now.");
            _isRecording = false;
            return _recordFrames;
        }

        public List<Frame> ClipFrames
        {
            get { return _clipFrames; }
            set 
            {
                if (value.Count < 1)
                    throw new Exception("Empty clip.");
                _clipFrames = value;
                _clipFrameIndex = 0;
            }
        }

        public int ClipFrameIndex
        {
            get { return _clipFrameIndex; }
            set { _clipFrameIndex = value; }
        }

        public void First()
        {
            if (_mode != Mode.Clip)
                return;
            _clipFrameIndex = 0;
            SetCurrentFrame(_clipFrames[_clipFrameIndex]);
        }

        public void Last()
        {
            if (_mode != Mode.Clip)
                return;
            _clipFrameIndex = _clipFrames.Count - 1;
            SetCurrentFrame(_clipFrames[_clipFrameIndex]);
        }

        public void Previous()
        {
            if (_mode != Mode.Clip)
                return;
            _clipFrameIndex--;
            if (_clipFrameIndex < 0)
                _clipFrameIndex = 0;
            SetCurrentFrame(_clipFrames[_clipFrameIndex]);
        }

        public void Next()
        {
            if (_mode == Mode.Live)
            {
                SetCurrentFrame(new Frame(_controller.Frame()));
            }
            else if (_mode == Mode.Clip)
            {
                if (_clipFrames == null)
                    return;
                _clipFrameIndex++;
                if (_clipFrameIndex >= _clipFrames.Count)
                    _clipFrameIndex = _clipFrames.Count - 1;
                SetCurrentFrame(_clipFrames[_clipFrameIndex]);
            }
            if (_isRecording)
                _recordFrames.Add(_currentFrame);
        }

        private void SetCurrentFrame(Frame frame)
        {
            _currentFrame = frame;
        }
    }
}
