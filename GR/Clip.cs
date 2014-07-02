using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GR.StructV2;

namespace GR
{
    [Serializable]
    public class Clip
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public List<Frame> Frames { get; set; }

        public Clip Clone()
        {
            Clip c = new Clip();
            c.Name = Name;
            c.Class = Class;
            c.Frames = new List<Frame>();
            foreach (Frame f in Frames)
                c.Frames.Add(new Frame(f));
            return c;
        }

        public void Save(System.IO.Stream stream)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, this);
        }

        public static Clip Load(System.IO.Stream stream)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return formatter.Deserialize(stream) as Clip;
        }
    }
}
