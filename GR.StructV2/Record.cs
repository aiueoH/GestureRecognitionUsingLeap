using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.StructV2
{
    [Serializable]
    public class Record
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Frame> Frames { get; set; }

        public List<MTrajection<Pointable>> PointableTracjections { get; set; }
        public List<MTrajection<Hand>> HandTracjections { get; set; }

        public Record(string name, TimeSpan duration, List<Frame> frames)
        {
            Name = name;
            Duration = duration;
            Frames = frames;
            //Frames = RemoveEmpty();
            //Frames = RemoveNoise();
        }

        public Record Clone()
        {
            return new Record(Name, Duration, CloneFrame());
        }

        public List<Frame> CloneFrame()
        {
            List<Frame> newFrames = new List<Frame>();
            foreach (Frame f in Frames)
                newFrames.Add(new Frame(f));
            return newFrames;
        }

        public void SaveFile(System.IO.Stream stream)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, this);
        }

        static public Record LoadFile(string fileName)
        {
            System.IO.Stream stream = System.IO.File.OpenRead(fileName);
            try
            {
                return Record.LoadFile(stream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }

        static public Record LoadFile(System.IO.Stream stream)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return formatter.Deserialize(stream) as Record;
        }

        private List<Frame> RemoveNoise()
        {
            List<Frame> newFrames = CloneFrame();
            for (int i = 0; i < Frames.Count; i++)
            {
                List<Pointable> ps = Frames[i].Pointables;
                foreach (Pointable p in ps)
                {
                    if (!(i + 5 < Frames.Count &&
                        Frames[i + 1].Pointable(p.Id) != null &&
                        Frames[i + 2].Pointable(p.Id) != null &&
                        Frames[i + 3].Pointable(p.Id) != null &&
                        Frames[i + 4].Pointable(p.Id) != null &&
                        Frames[i + 5].Pointable(p.Id) != null
                        ))
                    {
                        newFrames[i].Remove(p);
                        System.Diagnostics.Debug.WriteLine("Remove" + i + " " + p.Id);
                    }
                }
            }
            return newFrames;
        }

        private List<Frame> RemoveEmpty()
        {
            List<Frame> newFrames = new List<Frame>(Frames);
            foreach (Frame f in Frames)
                if (IsNoPointable(f))
                    newFrames.Remove(f);
            return newFrames;
        }

        /// <summary>
        /// 去頭去尾斷么九
        /// </summary>
        private void Trim()
        {
            if (Frames != null)
            {
                int first = 0, last = Frames.Count - 1;
                for (int i = 0; i < Frames.Count; i++)
                    if (!IsNoPointable(Frames[i]))
                    {
                        first = i;
                        break;
                    }
                for (int i = Frames.Count - 1; i >= 0; i--)
                    if (!IsNoPointable(Frames[i]))
                    {
                        last = i;
                        break;
                    }
                Frames = Frames.GetRange(first, last - first + 1);
            }
        }

        /// <summary>
        /// 檢查該 frame 是否沒有 pointable 存在
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private bool IsNoPointable(Frame frame)
        {
            return frame.Pointables.Count == 0 ? true : false; 
        }

        /// <summary>
        /// 取得 hand 軌跡
        /// </summary>
        /// <returns></returns>
        public List<MTrajection<Hand>> ExtractHandTrajections()
        {
            return ExtractHandTrajections(Frames);
        }

        /// <summary>
        /// 取得連續 frames 中所有的 pointable tracjection
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        private List<MTrajection<Hand>> ExtractHandTrajections(List<Frame> frames)
        {
            List<MTrajection<Hand>> result = new List<MTrajection<Hand>>();
            SetIsTraveld(frames, false);
            for (int i = 0; i < frames.Count; i++)
                foreach (Hand h in frames[i].Hands)
                    if (!h.IsTraveled)
                        result.Add(ExtractHandTrajection(frames, i, h));
            SetIsTraveld(frames, false);
            return result;
        }

        /// <summary>
        /// 在連續的 frames 中，從 frames[begin] 開始，找出指定的 hand 之 tracjection
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="begin"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        private MTrajection<Hand> ExtractHandTrajection(List<Frame> frames, int begin, Hand hand)
        {
            MTrajection<Hand> result = new MTrajection<Hand>();
            result.Begin = begin;
            result.Elements = new List<Hand>();
            for (int i = begin; i < frames.Count; i++)
            {
                Hand h = frames[i].Hand(hand.Id);
                if (i == begin && h != null && h != hand)
                    Console.WriteLine(String.Format("hand from frame[{0}] at frame[{1}] error", begin, i));
                if (h == null ||
                    h.IsTraveled == true)
                    break;
                h.IsTraveled = true;
                result.Elements.Add(h);
            }
            return result;
        }

        /// <summary>
        /// 取得 pointable 軌跡
        /// </summary>
        /// <returns></returns>
        public List<MTrajection<Pointable>> ExtractPointableTrajections()
        {
            return ExtractPointableTrajections(Frames);
        }

        /// <summary>
        /// 取得連續 frames 中所有的 pointable tracjection
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        private List<MTrajection<Pointable>> ExtractPointableTrajections(List<Frame> frames)
        {
            List<MTrajection<Pointable>> result = new List<MTrajection<Pointable>>();
            SetIsTraveld(frames, false);
            for (int i = 0; i < frames.Count; i++)
                foreach (Pointable p in frames[i].Pointables)
                    if (!p.IsTraveled)
                        result.Add(ExtractPointableTrajection(frames, i, p));
            SetIsTraveld(frames, false);
            // ---------------------------------------------------
            // 莫名其妙的 bug 待解
            for (int i = result.Count - 1; i >= 0; i--)
                if (result[i].Elements.Count == 0)
                    result.RemoveAt(i);
            // ---------------------------------------------------
            //foreach (MTrajection<MPointable> tt in result)
            //    Console.WriteLine("trac length=" + tt.Elements.Count);
            return result;
        }

        /// <summary>
        /// 在連續的 frames 中，從 frames[begin] 開始，找出指定的 pointable 之 tracjection
        /// </summary>
        /// <param name="frames">連續的 frames</param>
        /// <param name="begin">開始 index</param>
        /// <param name="pointable">指定的 pointable</param>
        /// <returns></returns>
        private MTrajection<Pointable> ExtractPointableTrajection(List<Frame> frames, int begin, Pointable pointable)
        {
            MTrajection<Pointable> result = new MTrajection<Pointable>();
            result.Begin = begin;
            result.Elements = new List<Pointable>();
            for (int i = begin; i < frames.Count; i++)
            {
                Pointable p = frames[i].Pointable(pointable.Id);
                if (i == begin && p != null && p != pointable)
                    Console.WriteLine(String.Format("from frame[{0}] at frame[{1}] error", begin, i));
                if (p == null || 
                    p.IsTraveled == true)
                    break;
                p.IsTraveled = true;
                result.Elements.Add(p);
            }
            return result;
        }

        public List<MTrajection<Pointable>> Filt(int length)
        {
            return Filt(ExtractPointableTrajections(), length);
        }

        public static List<MTrajection<T>> Filt<T>(List<MTrajection<T>> trajections, int length)
        {
            List<MTrajection<T>> result = new List<MTrajection<T>>(trajections);
            foreach (MTrajection<T> t in trajections)
                if (t.Elements.Count < length)
                    result.Remove(t);
            return result;
        }

        /// <summary>
        /// 設定每個 frame 中所有的 pointable、hand 的 IsTraveld 屬性
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="isTraveled"></param>
        private void SetIsTraveld(List<Frame> frames, bool isTraveled)
        {
            foreach (Frame f in frames)
            {
                foreach (Hand h in f.Hands)
                    h.IsTraveled = isTraveled;
                foreach (Pointable p in f.Pointables)
                    p.IsTraveled = isTraveled;
            }
        }

        /// <summary>
        /// 計算兩筆 record 的相似度 (DTW)
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public double CalcSimilarity(Record record)
        {
            Record ra = this, rb = record;
            Vector raPos = ra.Frames.First().Fingers.First().TipPosition;
            Vector rbPos = rb.Frames.First().Fingers.First().TipPosition;
            int raLength = ra.Frames.Count + 1;
            int rbLength = rb.Frames.Count + 1;
            double[,] dtw = new double[raLength, rbLength];
            for (int i = 1; i < raLength; i++)
                dtw.SetValue(double.PositiveInfinity, i, 0);
            for (int i = 1; i < rbLength; i++)
                dtw.SetValue(double.PositiveInfinity, 0, i);
            dtw.SetValue(0f, 0, 0);
            for (int i = 1; i < raLength; i++)
                for (int j = 1; j < rbLength; j++)
                {
                    double upper = dtw[1 - 1, j];
                    double left = dtw[i, j - 1];
                    double upperleft = dtw[i - 1, j - 1];
                    Vector va = ra.Frames[i - 1].Fingers.First().TipPosition.PosOffset(raPos);
                    Vector vb = rb.Frames[j - 1].Fingers.First().TipPosition.PosOffset(rbPos);
                    dtw[i, j] = va.DistanceTo(vb) + Math.Min(Math.Min(upper, left), Math.Min(left, upperleft));
                }
            using (System.IO.StreamWriter s = new System.IO.StreamWriter(ra.Name + "_" + rb.Name + ".csv"))
            {
                
            }
            return dtw[raLength - 1, rbLength - 1];
        }
    }
}
