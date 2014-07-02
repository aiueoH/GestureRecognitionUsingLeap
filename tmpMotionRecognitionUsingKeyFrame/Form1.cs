using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GR;
using GR.StructV2;

namespace tmpMotionRecognitionUsingKeyFrame
{
    public partial class Form1 : Form
    {
        private Leap.Controller _controller = new Leap.Controller();
        private MDManager _mdManager;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_loadclips_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<Clip> clips = new List<Clip>();
                foreach (string filename in ofd.FileNames)
                {
                    System.IO.Stream stream = File.Open(filename, FileMode.Open);
                    Clip clip = Clip.Load(stream);
                    clips.Add(clip);
                }
                _mdManager = new MDManager(clips);
            }
        }

        System.Timers.Timer _timer;
        private void button_start_Click(object sender, EventArgs e)
        {
            if (_mdManager == null)
                return;
            _timer = new System.Timers.Timer();
            _timer.Interval = 33;
            _timer.AutoReset = false;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(Process);
            _timer.Start();
        }

        private void Process(object sender, System.Timers.ElapsedEventArgs e)
        {
            Frame frame = new Frame(_controller.Frame());
            _mdManager.Detect(frame);
            _timer.Start();
        }
    }
}
