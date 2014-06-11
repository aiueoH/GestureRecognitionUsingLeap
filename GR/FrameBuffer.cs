using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR
{
    public class FrameBuffer
    {
        private List<Frame> _frames = new List<Frame>();
        public int Size { get; set; }

        public Frame Frame { get; set; }
        public Frame this[int index]
        {
            get
            {
                if (_frames.Count - 1 < index)
                    return null;
                return _frames[index];
            }
        }

        public FrameBuffer(int size)
        {
            Size = size;
            Frame = null;
        }

        public void Add(Frame frame)
        {
            _frames.Add(frame);
            lock (this)
            {
                while (_frames.Count > Size) _frames.RemoveAt(0);
            }
            UpdateFrame();
        }

        public void UpdateFrame()
        {
            if (_frames.Count < Size) return;
            Frame = _frames.Last();
        }

        //public Hand UpdateLeftHand()
        //{
        //    int count = CountLeftHand();
        //    if (count < Size - 1) return null;
        //    Hand avgH = new Hand();
        //    foreach (Frame f in _frames)
        //    {
        //        if (f.LeftHand == null) continue;
        //    }   
        //    return null;
        //}

        //public int CountLeftHand()
        //{
        //    int count = 0;
        //    foreach (Frame f in _frames)
        //        if (f.LeftHand != null)
        //            count++;
        //    return count;
        //}
    }
}
