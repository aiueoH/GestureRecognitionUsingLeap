using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GR;
using GR.StructV2;


namespace tmpMotionRecognition
{
    public partial class Form1 : Form
    {
        private Clip _a, _b;

        public Form1()
        {
            InitializeComponent();
        }

        private void button_loadA_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.IO.Stream s = ofd.OpenFile();
                _a = Clip.Load(s);
            }
        }

        private void button_loadB_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.IO.Stream s = ofd.OpenFile();
                _b = Clip.Load(s);
            }
        }

        private void button_similarity_Click(object sender, EventArgs e)
        {
            float[,] dtwTable;
            float score = MotionMatching.DTW(_a.Frames, _b.Frames, out dtwTable);
            Console.WriteLine(String.Format("[{0}][{1}] v.s. [{2}][{3}] ===> {4}", _a.Class, _a.Name, _b.Class, _b.Name, score));
            //DTWVisualize(dtwTable);
        }

        private void DTWVisualize(float[,] table)
        {
            List<int[]> path = FindDTWPath(table);
            ShowDTWInExcel(table, path);
        }

        private void ShowDTWInExcel(float[,] table, List<int[]> path)
        {
            Microsoft.Office.Interop.Excel.Application app;
            Microsoft.Office.Interop.Excel._Workbook wb;
            Microsoft.Office.Interop.Excel._Worksheet ws;
            Microsoft.Office.Interop.Excel.Range wr;

            app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            app.DisplayAlerts = false;
            app.Workbooks.Add(Type.Missing);
            wb = app.Workbooks[1];
            wb.Activate();
            try
            {
                // 引用第一個工作表
                ws = (Microsoft.Office.Interop.Excel._Worksheet)wb.Worksheets[1];
                // 命名工作表的名稱
                ws.Name = "工作表測試";
                // 設定工作表焦點
                ws.Activate();
                for (int i = 0; i < table.GetLength(0); i++)
                    for (int j = 0; j < table.GetLength(1); j++)
                        if (table[i, j] == float.PositiveInfinity)
                            app.Cells[i + 2, j + 2] = "PositiveInfinity";
                        else
                            app.Cells[i + 2, j + 2] = table[i, j];
                foreach (int[] p in path)
                {
                    wr = ws.Range[ws.Cells[p[0] + 2, p[1] + 2], ws.Cells[p[0] + 2, p[1] + 2]];
                    wr.Select();
                    wr.Interior.Color = ColorTranslator.ToOle(Color.GreenYellow);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("產生報表時出錯！" + Environment.NewLine + ex.Message);
            }
        }

        private List<int[]> FindDTWPath(float[,] table)
        {
            List<int[]> path = new List<int[]>();
            int x = table.GetLength(0) - 1;
            int y = table.GetLength(1) - 1;
            path.Add(new int[2] { x, y });
            while (true)
            {
                float lu = table[x - 1, y - 1];
                float l = table[x - 1, y];
                float u = table[x, y - 1];
                if (
                    (lu <= l && l <= u) ||
                    (lu <= u && u <= l))
                {
                    x -= 1;
                    y -= 1;
                }
                else if (
                    (l <= lu && lu <= u) ||
                    (l <= u && u <= lu))
                    x -= 1;
                else if (
                    (u <= lu && lu <= l) ||
                    (u <= l && l <= lu))
                    y -= 1;
                else
                    throw new Exception("dtw path min error");
                if (x == 0 && y == 0)
                    break;
                path.Add(new int[2] { x, y });
            }
            path.Reverse();
            return path;
        }

        private void button_keyframe_Click(object sender, EventArgs e)
        {
            List<int> keyframeIndexs = MotionMatching.ExtractKeyFrame(_a.Frames);
            foreach (int index in keyframeIndexs)
            {
                HandFeatures hf = HandFeatures.ExtractFeatures(_a.Frames[index]);
                Console.WriteLine(Convert.ToString(hf.StateCode, 2));
            }
        }
    }
}
