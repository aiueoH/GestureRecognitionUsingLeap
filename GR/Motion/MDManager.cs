using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR
{
    [Serializable]
    public class MDManager
    {
        public delegate void OnMotionDetectDelegate(object sender, string className, string name);
        public event OnMotionDetectDelegate OnMotionDetect;

        Dictionary<string, List<MotionDetector>> _classMDs = new Dictionary<string, List<MotionDetector>>();

        public MDManager(List<Clip> clips)
        {
            foreach (Clip c in clips)
            {
                MotionDetector md = new MotionDetector(c.Class, c.Name, c.Frames);
                List<MotionDetector> mds;
                if (_classMDs.ContainsKey(c.Class))
                    mds = _classMDs[c.Class];
                else
                {
                    mds = new List<MotionDetector>();
                    _classMDs.Add(c.Class, mds);
                }
                mds.Add(md);
            }
        }

        public void Detect(Frame frame)
        {
            HandFeatures hf = HandFeatures.ExtractFeatures(frame);
            if (hf == null)
                return;
            foreach (string className in _classMDs.Keys.ToArray())
                foreach (MotionDetector md in _classMDs[className])
                    if (md.Detect(frame, hf))
                    {
                        Reset(md.Class);
                        if (OnMotionDetect != null)
                            OnMotionDetect(this, md.Class, md.Name);
                        break;
                    }
        }

        public void Reset()
        {
            foreach (string className in _classMDs.Keys.ToArray())
                Reset(className);
        }

        public void Reset(string className)
        {
            if (!_classMDs.ContainsKey(className))
                return;
            foreach (MotionDetector md in _classMDs[className])
                md.Reset();
        }
    }
}
