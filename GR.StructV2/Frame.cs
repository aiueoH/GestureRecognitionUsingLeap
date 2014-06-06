using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
{
    [Serializable]
    public class Frame
    {
        public long Id { get; set; }
        public long Timestamp { get; set; }
        public Hand LeftHand { get; set; }
        public Hand RightHand { get; set; }
        public List<Hand> Hands { get; set; }
        public List<Pointable> Pointables { get; set; }
        public List<Finger> Fingers { get; set; }
        public List<Tool> Tools{ get; set; }
        public List<Gesture> Gestures { get; set; }

        //public Frame(long id, long timestamp, List<Hand> hands, List<Pointable> pointables, List<Finger> fingers, List<Tool> tools)
        //{
        //    Id = id;
        //    Timestamp = timestamp;
        //    Hands = hands;
        //    Pointables = pointables;
        //    Fingers = fingers;
        //    Tools = tools;
        //    foreach (Hand h in Hands)
        //        if (h.IsLeft)
        //            LeftHand = h;
        //        else if (h.IsRight)
        //            RightHand = h;
        //        else
        //            throw new Exception("Unknow hand");
        //}

        public Frame(Leap.Frame frame)
        {
            this.Id = frame.Id;
            this.Timestamp = frame.Timestamp;
            this.Hands = new List<Hand>();
            this.Pointables = new List<Pointable>();
            this.Fingers = new List<Finger>();
            this.Tools = new List<Tool>();
            foreach (Leap.Hand lh in frame.Hands)
            {
                Hand h = new Hand(lh, this);
                this.Hands.Add(h);
                this.Pointables.AddRange(h.Pointables);
                this.Fingers.AddRange(h.Fingers);
                this.Tools.AddRange(h.Tools);
                if (h.IsLeft)
                    LeftHand = h;
                else if (h.IsRight)
                    RightHand = h;
                else
                    throw new Exception("Unknow hand");
            }
                
            this.Gestures = new List<Gesture>();
            foreach (Leap.Gesture g in frame.Gestures())
                Gestures.Add(new Gesture(g, this, Hands, Pointables));
        }

        public Frame(Frame frame)
        {
            this.Id = frame.Id;
            this.Timestamp = frame.Timestamp;
            this.Hands = new List<Hand>();
            this.Pointables = new List<Pointable>();
            this.Fingers = new List<Finger>();
            this.Tools = new List<Tool>();
            foreach (Hand h in frame.Hands)
            {
                Hand nh = new Hand(h, this);
                this.Hands.Add(nh);
                this.Pointables.AddRange(nh.Pointables);
                this.Fingers.AddRange(nh.Fingers);
                this.Tools.AddRange(nh.Tools);
                if (h.IsLeft)
                    LeftHand = h;
                else if (h.IsRight)
                    RightHand = h;
                else
                    throw new Exception("Unknow hand");
            }
            this.Gestures = new List<Gesture>();
            foreach (Gesture g in frame.Gestures)
                Gestures.Add(new Gesture(g, this, Hands, Pointables));
        }

        public Frame Clone()
        {
            return new Frame(this);
        }

        public Hand Hand(int id)
        {
            foreach (Hand h in Hands)
                if (h.Id == id)
                    return h;
            return null;
        }

        public Pointable Pointable(int id)
        {
            foreach (Pointable p in Pointables)
                if (p.Id == id)
                    return p;
            return null;
        }

        public void Remove(Pointable pointable)
        {
            Pointables.Remove(pointable);
            Finger f = pointable as Finger;
            if (f != null)
                Fingers.Remove(f);
            else
                Tools.Remove(pointable as Tool);
        }
    }
}