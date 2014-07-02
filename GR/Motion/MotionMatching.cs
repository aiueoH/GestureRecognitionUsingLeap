using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR
{
    public class MotionMatching
    {
        public static int T1 = 20; // 手指數目不同時 需持續的畫面數
        public static float T2 = (15f).DegreeToRadian(); // 餘弦相似度閥值

        public static List<int> ExtractKeyFrame(List<Frame> frames, out List<HandFeatures> keyFrameHFs)
        {
            keyFrameHFs = new List<HandFeatures>();
            List<int> keys = new List<int>();
            int lastKey;
            Hand currentHand;
            Hand lastKeyHand;
            HandFeatures currentHF;
            HandFeatures lastKeyHF;
            if (frames.Count < 1)
                return keys;
            // 第一張一定是key frame
            lastKey = 0;
            lastKeyHF = HandFeatures.ExtractFeatures(frames[lastKey]);
            lastKeyHand = frames[lastKey].Hands[0];
            keys.Add(lastKey);
            keyFrameHFs.Add(lastKeyHF);
            //
            for (int i = 1; i < frames.Count; i++)
            {
                currentHand = frames[i].Hands[0];
                currentHF = HandFeatures.ExtractFeatures(frames[i]);

                //// 檢查手指數目 (state code)
                //if (lastKeyHF.StateCode != currentHF.StateCode)
                //{
                //    // 往後檢查
                //    int count = 0;
                //    for (int j = i; j < i + T1 && j < frames.Count; j++)
                //    {
                //        HandFeatures hf = HandFeatures.ExtractFeatures(frames[j]);
                //        if (currentHF.StateCode != hf.StateCode)
                //            break;
                //        count++;
                //    }
                //    if (count == T1)
                //    {
                //        // 確定是 key frame
                //        lastKey = i;
                //        lastKeyHF = currentHF;
                //        lastKeyHand = currentHand;
                //        keys.Add(lastKey);
                //        keyFrameHFs.Add(lastKeyHF);
                //        continue;
                //    }
                //}

                // 檢查變異量
                // ↓↓↓↓↓↓↓↓ S和T其中一邊有伸直手指就比較變異輛的方法
                float sim = HandFeatures.CosSimilarity(currentHF, lastKeyHF);
                if (sim > T2)
                {
                    Console.WriteLine("差異度 " + sim.RadianToDegree());
                    lastKey = i;
                    lastKeyHF = currentHF;
                    lastKeyHand = currentHand;
                    keys.Add(lastKey);
                    keyFrameHFs.Add(lastKeyHF);
                    continue;
                }
                //// 檢查變異量
                //// ↓↓↓↓↓↓↓↓ 伸直手指的變異量方法
                //if (currentHF.StateCode == lastKeyHF.StateCode)
                //{
                //    //float sim = HandFeatures.CosSimilarity(currentHand, lastKeyHand, currentHF.StateCode);
                //    float sim = HandFeatures.CosSimilarity(currentHand, lastKeyHand);
                //    if (sim > T2)
                //    {
                //        Console.WriteLine("差異度 " + sim.RadianToDegree());
                //        lastKey = i;
                //        lastKeyHF = currentHF;
                //        lastKeyHand = currentHand;
                //        keys.Add(lastKey);
                //        keyFrameHFs.Add(lastKeyHF);
                //        continue;
                //    }
                //}
                // ↓↓↓↓↓↓↓↓ 全部手指的變異量方法
                //if (currentFHF.CosSimilarity(lastKFHF) > T2)
                //{
                //    lastKeyFrame = i;
                //    lastKFHF = currentFHF;
                //    keyFrames.Add(lastKeyFrame);
                //    continue;
                //}
            }
            return keys;
        }

        public static List<int> ExtractKeyFrame_B(List<Frame> frames, out List<HandFeatures> keyFrameHFs)
        {
            keyFrameHFs = new List<HandFeatures>();
            List<int> keys = new List<int>();
            int lastKey;
            Hand currentHand;
            Hand lastKeyHand;
            HandFeatures currentHF;
            HandFeatures lastKeyHF;
            if (frames.Count < 1)
                return keys;
            // 第一張一定是key frame
            lastKey = 0;
            lastKeyHF = HandFeatures.ExtractFeatures(frames[lastKey]);
            lastKeyHand = frames[lastKey].Hands[0];
            keys.Add(lastKey);
            keyFrameHFs.Add(lastKeyHF);
            //
            for (int i = 1; i < frames.Count; i++)
            {
                currentHand = frames[i].Hands[0];
                currentHF = HandFeatures.ExtractFeatures(frames[i]);

                // 檢查手指數目 (state code)
                if (lastKeyHF.StateCode != currentHF.StateCode)
                {
                    // 往後檢查
                    int count = 0;
                    for (int j = i; j < i + T1 && j < frames.Count; j++)
                    {
                        HandFeatures hf = HandFeatures.ExtractFeatures(frames[j]);
                        if (currentHF.StateCode != hf.StateCode)
                            break;
                        count++;
                    }
                    if (count == T1)
                    {
                        // 確定是 key frame
                        lastKey = i;
                        lastKeyHF = currentHF;
                        lastKeyHand = currentHand;
                        keys.Add(lastKey);
                        keyFrameHFs.Add(lastKeyHF);
                        continue;
                    }
                }

                // 檢查變異量
                // ↓↓↓↓↓↓↓↓ 伸直手指的變異量方法
                if (currentHF.StateCode == lastKeyHF.StateCode)
                {
                    float sim = HandFeatures.CosSimilarity(currentHand, lastKeyHand, currentHF.StateCode);
                    if (sim > T2)
                    {
                        Console.WriteLine("差異度 " + sim.RadianToDegree());
                        lastKey = i;
                        lastKeyHF = currentHF;
                        lastKeyHand = currentHand;
                        keys.Add(lastKey);
                        keyFrameHFs.Add(lastKeyHF);
                        continue;
                    }
                }
            }
            return keys;
        }

        public static List<int> ExtractKeyFrame(List<Frame> frames)
        {
            List<HandFeatures> tmp;
            return ExtractKeyFrame(frames, out tmp);
        }

        public static float DTW(List<Frame> fsA, List<Frame> fsB)
        {
            float[,] dtwTable;
            return DTW(fsA, fsB, out dtwTable);
        }

        public static float DTW(List<Frame> fsA, List<Frame> fsB, out float[,] dtwTable)
        {
            List<HandFeatures> hfsA = new List<HandFeatures>();
            List<HandFeatures> hfsB = new List<HandFeatures>();
            foreach (Frame frame in fsA)
            {
                if (frame.Hands.Count != 1)
                    continue;
                hfsA.Add(HandFeatures.ExtractFeatures(frame.Hands[0]));
            }
            foreach (Frame frame in fsB)
            {
                if (frame.Hands.Count != 1)
                    continue;
                hfsB.Add(HandFeatures.ExtractFeatures(frame.Hands[0]));
            }
            float[,] table = DTW(hfsA, hfsB);
            dtwTable = table;
            return table[table.GetLength(0) - 1, table.GetLength(1) - 1];
        }

        private static float[,] DTW(List<HandFeatures> hfsA, List<HandFeatures> hfsB)
        {
            int lA = hfsA.Count;
            int lB = hfsB.Count;
            float[,] table = CreateDTWTable(lA, lB);
            for (int i = 1; i < lA; i++)
            {
                for (int j = 1; j < lB; j++)
                {
                    float upper = table[i - 1, j];
                    float left = table[i, j - 1];
                    float upperleft = table[i - 1, j - 1];
                    float current = hfsA[i - 1].CosSimilarity(hfsB[j - 1]);
                    table[i, j] = current + Math.Min(Math.Min(upper, left), Math.Min(left, upperleft));
                }
            }
            return table;
        }

        private static float[,] CreateDTWTable(int lengthA, int lengthB)
        {
            float[,] table = new float[lengthA, lengthB];
            for (int i = 1; i < lengthA; i++)
                table.SetValue(float.PositiveInfinity, i, 0);
            for (int i = 1; i < lengthB; i++)
                table.SetValue(float.PositiveInfinity, 0, i);
            table.SetValue(0, 0, 0);
            return table;
        }
    }
}
