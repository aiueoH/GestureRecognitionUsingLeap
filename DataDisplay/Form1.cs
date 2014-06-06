﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GR;
using GR.StructV2;

namespace DataDisplay
{
    public partial class Form1 : Form
    {
        private Detector _detector = new Detector();

        public Form1()
        {
            Leap.Vector v = new Leap.Vector();

            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            new Thread(delegate()
                {
                    while (true)
                    {
                        Frame frame = _detector.Frame;
                        if (frame == null) continue;
                        Hand hand = frame.LeftHand;
                        if (hand == null) continue;
                        textBox01.Text = hand.Fingers[0].Direction.x.ToString();
                        textBox02.Text = hand.Fingers[0].Direction.y.ToString();
                        textBox03.Text = hand.Fingers[0].Direction.z.ToString();

                        textBox11.Text = hand.Fingers[1].Direction.x.ToString();
                        textBox12.Text = hand.Fingers[1].Direction.y.ToString();
                        textBox13.Text = hand.Fingers[1].Direction.z.ToString();

                        textBox21.Text = hand.Fingers[2].Direction.x.ToString();
                        textBox22.Text = hand.Fingers[2].Direction.y.ToString();
                        textBox23.Text = hand.Fingers[2].Direction.z.ToString();

                        textBox31.Text = hand.Fingers[3].Direction.x.ToString();
                        textBox32.Text = hand.Fingers[3].Direction.y.ToString();
                        textBox33.Text = hand.Fingers[3].Direction.z.ToString();

                        textBox41.Text = hand.Fingers[4].Direction.x.ToString();
                        textBox42.Text = hand.Fingers[4].Direction.y.ToString();
                        textBox43.Text = hand.Fingers[4].Direction.z.ToString();

                        textBox1.Text = hand.GrabStrength.ToString();
                    }
                }).Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Frame frame = _detector.Frame;
            if (frame == null) return;
            Hand hand = frame.LeftHand;
            if (hand == null) return;

            string s = String.Format(
                @"
                    {0}, {1}, {2},
                    {3}, {4}, {5},
                    {6}, {7}, {8},
                    {9}, {10}, {11},
                    {12}, {13}, {14},

                    {15}, {16}, {17},

                    {18},
                    {19},
                    {20},
                    {21},
                    {22},
                ",
                 hand.Fingers[0].Direction.x, hand.Fingers[0].Direction.y, hand.Fingers[0].Direction.z,
                 hand.Fingers[1].Direction.x, hand.Fingers[1].Direction.y, hand.Fingers[1].Direction.z,
                 hand.Fingers[2].Direction.x, hand.Fingers[2].Direction.y, hand.Fingers[2].Direction.z,
                 hand.Fingers[3].Direction.x, hand.Fingers[3].Direction.y, hand.Fingers[3].Direction.z,
                 hand.Fingers[4].Direction.x, hand.Fingers[4].Direction.y, hand.Fingers[4].Direction.z,
                 hand.Direction.Pitch, hand.Direction.Roll, hand.Direction.Yaw,
                 hand.Fingers[0].Direction.DistanceTo(hand.Direction),
                 hand.Fingers[1].Direction.DistanceTo(hand.Direction),
                 hand.Fingers[2].Direction.DistanceTo(hand.Direction),
                 hand.Fingers[3].Direction.DistanceTo(hand.Direction),
                 hand.Fingers[4].Direction.DistanceTo(hand.Direction)
                 );
            Console.WriteLine(s);
        }
    }
}