using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GR.StructV2;
using GR;

namespace tmpFeaturesTest
{
    public partial class Form1 : Form
    {
        private Leap.Controller _c = new Leap.Controller();

        public Form1()
        {
            InitializeComponent();
        }

        private void button_catch_Click(object sender, EventArgs e)
        {
            Frame frame = new Frame(_c.Frame());
            foreach (Hand hand in frame.Hands)
            {
                HandFeatures hf = HandFeatures.ExtractFeatures(hand);
                Console.WriteLine("state code = " + hf.StateCode + "," + Convert.ToString(hf.StateCode, 2));
            }
        }
    }
}
