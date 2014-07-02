using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GR.StructV2;

namespace GR
{
    [Serializable]
    public class MotionDetector
    {
        // sim
        public static float T1 = 0.5f;
        // time 
        public static float T2 = 2f;

        public string Class { get; private set; }
        public string Name { get; private set; }
        private List<Frame> _frames;
        private List<int> _keys;
        private List<HandFeatures> _keyHFs;
        private HandFeatures _currentHF;
        private int _step;
        private long _timeStamp;

        public MotionDetector(string className, string name, List<Frame> frames)
        {
            Class = className;
            Name = name;
            _frames = frames;
            _keys = MotionMatching.ExtractKeyFrame(frames, out _keyHFs);
            Reset();
        }

        public void Reset()
        {
            _step = 0;
            _currentHF = _keyHFs[_step];
        }

        public bool Detect(Frame frame, HandFeatures hf)
        {
            // 檢查 time out
            if (IsTimeOut(frame))
                Reset();
            // 檢查 key frame
            if (hf.StateCode == _currentHF.StateCode)
            {
                float sim = HandFeatures.CosSimilarity(hf, _currentHF);
                if (sim < MotionMatching.T2 * T1)
                {
                    Debug.WriteLine(String.Format("[{0}] [{1}] 第{2}個 key frame 完成", Class, Name, _step));
                    _timeStamp = frame.Timestamp;
                    _step++;
                    if (_step == _keys.Count)
                        return true;
                    else
                        _currentHF = _keyHFs[_step];
                }
            }
            return false;
        }

        public bool IsTimeOut(Frame frame)
        {
            if (_step == 0)
                return false;
            long sampleTime = _frames[_keys[_step]].Timestamp - _frames[_keys[_step - 1]].Timestamp;
            long testTime = frame.Timestamp - _timeStamp;
            if (testTime * T2 > sampleTime)
                return true;
            else
                return false;
        }

    }
}
