using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.StructV2
{
    [Serializable]
    public class Gesture
    {
        public enum GestureType
        {
            INVALID,
            SWIPE,
            CIRCLE,
            SCREEN_TAP,
            KEY_TAP,
        }

        public enum GestureState
        {
            INVALID,
            START,
            UPDATE,
            STOP,
        }

        public Frame Frame { get; set; }
        public float Duration { get; set; }
        public float DurationSeconds { get; set; }
        public GestureType Type { get; set; }
        public GestureState State { get; set; }
        public List<Hand> Hands { get; set; }
        public List<Pointable> Pointables { get; set; }

        public Gesture(Leap.Gesture gesture, Frame frame, List<Hand> hands, List<Pointable> pointables)
        {
            Frame = frame;
            Duration = gesture.Duration;
            DurationSeconds = gesture.DurationSeconds;
            SetType(gesture);
            SetState(gesture);
            SetHandsFromLeap(gesture, hands);
            SetPointablesFromLeap(gesture, pointables);
        }

        public Gesture(Gesture gesture, Frame frame, List<Hand> hands, List<Pointable> pointables)
        {
            Frame = frame;
            Duration = gesture.Duration;
            DurationSeconds = gesture.DurationSeconds;
            Type = gesture.Type;
            State = gesture.State;
            Hands = new List<Hand>();
            foreach (Hand gh in gesture.Hands)
                foreach (Hand h in hands)
                    if (gh.Id == h.Id)
                        Hands.Add(h);
            Pointables = new List<Pointable>();
            foreach (Pointable gp in gesture.Pointables)
                foreach (Pointable p in pointables)
                    if (gp.Id == p.Id)
                        Pointables.Add(p);
        }

        private void SetPointablesFromLeap(Leap.Gesture gesture, List<Pointable> pointables)
        {
            Pointables = new List<Pointable>();
            foreach (Leap.Pointable gp in gesture.Pointables)
                foreach (Pointable p in pointables)
                    if (gp.Id == p.Id)
                        Pointables.Add(p);
        }

        private void SetHandsFromLeap(Leap.Gesture gesture, List<Hand> hands)
        {
            Hands = new List<Hand>();
            foreach (Leap.Hand lp in gesture.Hands)
                foreach (Hand h in hands)
                    if (lp.Id == h.Id)
                        Hands.Add(h);
        }

        private void SetState(Leap.Gesture gesture)
        {
            switch (gesture.State)
            {
                case Leap.Gesture.GestureState.STATE_INVALID:
                    State = GestureState.INVALID;
                    break;
                case Leap.Gesture.GestureState.STATE_START:
                    State = GestureState.START;
                    break;
                case Leap.Gesture.GestureState.STATE_UPDATE:
                    State = GestureState.UPDATE;
                    break;
                case Leap.Gesture.GestureState.STATE_STOP:
                    State = GestureState.STOP;
                    break;
                default:
                    throw new Exception("Unknow gesture state");
            }
        }

        private void SetType(Leap.Gesture gesture)
        {
            switch (gesture.Type)
            {
                case Leap.Gesture.GestureType.TYPE_INVALID:
                    Type = GestureType.INVALID;
                    break;
                case Leap.Gesture.GestureType.TYPE_SWIPE:
                    Type = GestureType.SWIPE;
                    break;
                case Leap.Gesture.GestureType.TYPE_CIRCLE:
                    Type = GestureType.CIRCLE;
                    break;
                case Leap.Gesture.GestureType.TYPE_SCREEN_TAP:
                    Type = GestureType.SCREEN_TAP;
                    break;
                case Leap.Gesture.GestureType.TYPE_KEY_TAP:
                    Type = GestureType.KEY_TAP;
                    break;
                default:
                    throw new Exception("Unknow gesture type");
            }
        }
    }
}
