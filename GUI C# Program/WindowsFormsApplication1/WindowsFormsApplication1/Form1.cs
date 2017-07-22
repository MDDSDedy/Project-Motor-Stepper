using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.IO.Ports;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int TickStart, intMode = 1;
        private int pos = 0, step, cmd = 0, i = 0;
        private string tString = string.Empty;
        private double dtA, dtB, deg;
        private bool isCW, isCont;
 
        private void Form1_Load(object sender, EventArgs e)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "Phase Graph";
            myPane.XAxis.Title.Text = "Time (centisecond)";
            myPane.YAxis.Title.Text = "Phase";

            RollingPointPairList siji = new RollingPointPairList(60000);
            RollingPointPairList loro = new RollingPointPairList(60000);

            LineItem curve = myPane.AddCurve("Phase 1", siji, Color.Red, SymbolType.None);
            LineItem curve1 = myPane.AddCurve("Phase 2", loro, Color.Blue, SymbolType.None);

            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 50;
            myPane.XAxis.Scale.MinorStep = 1;
            myPane.XAxis.Scale.MajorStep = 5;

            myPane.YAxis.Scale.Min = -300;
            myPane.YAxis.Scale.Max = 300;

            zedGraphControl1.AxisChange();

            TickStart = Environment.TickCount;
            
        }
     
        private void button2_Click(object sender, EventArgs e)
        { 
            if (serialPort1.IsOpen)
            {
                i = 0;
                if (radioButton3.Checked)
                {
                    cmd = 1;
                    timer1.Interval = 15;
                    if (radioButton1.Checked)
                    {
                        step = 10;
                    }
                    if (radioButton2.Checked)
                    {
                        deg = Convert.ToDouble(textBox2.Text);
                        step = Convert.ToInt16(deg / 1.8);
                    }
                }

                if (radioButton4.Checked)
                {
                    cmd = 2;
                    timer1.Interval = 2;
                    if (radioButton1.Checked)
                    {
                        step = 10;
                    }
                    if (radioButton2.Checked)
                    {
                        deg = Convert.ToDouble(textBox2.Text);
                        step = Convert.ToInt16(deg / 0.9);
                    }
                }
                if (radioButton6.Checked)
                {
                    isCW = true;
                }
                if (radioButton7.Checked)
                {
                    isCW = false;
                }
                if (radioButton1.Checked)
                {
                    isCont = true;
                }
                if (radioButton2.Checked)
                {
                    isCont = false;
                }
                if (radioButton5.Checked)
                {
                    timer1.Interval = 1;
                    cmd = 3;
                    if (radioButton1.Checked)
                    {
                        step = 15;
                    }
                    if (radioButton2.Checked)
                    {
                        deg = Convert.ToDouble(textBox2.Text);
                        step = Convert.ToInt16((deg / 7.2) * 360); 
                    }
                }
                timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Serial belum tersambung!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                timer1.Enabled = false;
                serialPort1.Write("#+000*+000" + '\n');
            }
            else
            {
                MessageBox.Show("Serial belum tersambung!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (!serialPort1.IsOpen)
            {
                try
                {
                    if (serialPort1.IsOpen) serialPort1.Close();
                    serialPort1.PortName = Convert.ToString(textBox1.Text);
                    serialPort1.BaudRate = Convert.ToInt32(textBox3.Text);
                    serialPort1.Parity = Parity.None;
                    serialPort1.Handshake = Handshake.None;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    button1.Text = "Disconnect";
                }
                catch (Exception er)
                {
                    MessageBox.Show("Port tidak dapat dibuka " + er.ToString(), "Buka Port Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    serialPort1.Write("#+000*+000" + '\n');
                    timer1.Enabled = false;
                    serialPort1.Close();
                    button1.Text = "Connect";
                }
                catch (Exception er)
                {
                    MessageBox.Show("Port tidak dapat ditutup " + er.ToString(), "Tutup Port Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cmd == 1)
            {
                if (i < step)
                {
                    fStep();
                    i++;
                }
                else
                {
                    timer1.Enabled = false;
                }
            }
            else if (cmd == 2)
            {
                if (i < step)
                {
                    hStep();
                    i++;
                }
                else
                {
                    timer1.Enabled = false;
                }
            }
            else if (cmd == 3) { mStep(); i += 15; }
            else if (cmd == 4) { }

            if (isCont)
            {
                step += 15;
            }
        }

        private void hStep()
        {
            if (isCW)
            {
                if (pos == 0)
                {
                    _0();
                    pos = 1;
                }
                else if (pos == 1)
                {
                    _1();
                    pos = 2;
                }
                else if (pos == 2)
                {
                    _2();
                    pos = 3;
                }
                else if (pos == 3)
                {
                    _3();
                    pos = 4;
                }
                else if (pos == 4)
                {
                    _4();
                    pos = 5;
                }
                else if (pos == 5)
                {
                    _5();
                    pos = 6;
                }
                else if (pos == 6)
                {
                    _6();
                    pos = 7;
                }
                else if (pos == 7)
                {
                    _7();
                    pos = 0;
                }
            }
            else
            {
                if (pos == 0)
                {
                    _0();
                    pos = 7;
                }
                else if (pos == 1)
                {
                    _1();
                    pos = 0;
                }
                else if (pos == 2)
                {
                    _2();
                    pos = 1;
                }
                else if (pos == 3)
                {
                    _3();
                    pos = 2;
                }
                else if (pos == 4)
                {
                    _4();
                    pos = 3;
                }
                else if (pos == 5)
                {
                    _5();
                    pos = 4;
                }
                else if (pos == 6)
                {
                    _6();
                    pos = 5;
                }
                else if (pos == 7)
                {
                    _7();
                    pos = 6;
                }
            }
        }

        private void fStep()
        {
            if (isCW)
            {
                if (pos == 0)
                {
                    _0();
                    pos = 2;
                }
                else if (pos == 2)
                {
                    _2();
                    pos = 4;
                }
                else if (pos == 4)
                {
                    _4();
                    pos = 6;
                }
                else
                {
                    _6();
                    pos = 0;
                }
            }
            else
            {
                if (pos == 0)
                {
                    _0();
                    pos = 6;
                }
                else if (pos == 2)
                {
                    _2();
                    pos = 0;
                }
                else if (pos == 4)
                {
                    _4();
                    pos = 2;
                }
                else
                {
                    _6();
                    pos = 4;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                timer1.Enabled = false;
                serialPort1.Write("#+000*+000" + '\n');
                serialPort1.Close();
            }
        }

        private void mStep()
        {
            if (!isCW)
            {
                if (i < step)
                {
                    deg = i * (Math.PI / 180);
                    dtA = Math.Sin(deg) * 255;
                    dtB = Math.Cos(deg) * 255;
                    Draw((dtA), (dtB));
                    serialPort1.Write("#" + dtA.ToString("+000;-000") + "*" + dtB.ToString("+000;-000") + '\n');
                }
                else if (i >= step)
                {
                    deg = step * (Math.PI / 180);
                    dtA = Math.Sin(deg) * 255;
                    dtB = Math.Cos(deg) * 255;
                    Draw((dtA), (dtB));
                    timer1.Enabled = false;
                    serialPort1.Write("#" + dtA.ToString("+000;-000") + "*" + dtB.ToString("+000;-000") + '\n');
                }
            }
            else
            {
                if (i < step)
                {
                    deg = i * (Math.PI / 180);
                    dtA = Math.Cos(deg) * 255;
                    dtB = Math.Sin(deg) * 255;
                    Draw((dtA), (dtB));
                    serialPort1.Write("#" + dtA.ToString("+000;-000") + "*" + dtB.ToString("+000;-000") + '\n');
                }
                else if (i >= step)
                {
                    deg = step * (Math.PI / 180);
                    dtB = Math.Sin(deg) * 255;
                    dtA = Math.Cos(deg) * 255;
                    Draw((dtA), (dtB));
                    timer1.Enabled = false;
                    serialPort1.Write("#" + dtA.ToString("+000;-000") + "*" + dtB.ToString("+000;-000") + '\n');
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = radioButton2.Checked;
        }

        private void _0()
        {
            serialPort1.Write("#+255*+000" + '\n');
            Draw(255, 0);
           
        }
        private void _1()
        {
            serialPort1.Write("#+255*+255" + '\n');
            Draw(255, 255);

        }
        private void _2()
        {
            serialPort1.Write("#+000*+255" + '\n');
            Draw(0, 255);

        }
        private void _3()
        {
            serialPort1.Write("#-255*+255" + '\n');
            Draw(-255, 255);

        }
        private void _4()
        {
            serialPort1.Write("#-255*+000" + '\n');
            Draw(-255, 0);

        }
        private void _5()
        {
            serialPort1.Write("#-255*-255" + '\n');
            Draw(-255, -255);

        }
        private void _6()
        {
            serialPort1.Write("#+000*-255" + '\n');
            Draw(0, -255);

        }
        private void _7()
        {
            serialPort1.Write("#+255*-255" + '\n');
            Draw(255, -255);

        }

        private void Draw(double phase, double phase1)
        {
            if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                return;

            LineItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            LineItem curve1 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
            if (curve == null)
                return;
            if (curve1 == null)
                return;

            IPointListEdit siji = curve.Points as IPointListEdit;
            IPointListEdit loro = curve1.Points as IPointListEdit;
            if (siji == null)
                return;
            if (loro == null)
                return;

            double  waktu = (Environment.TickCount - TickStart) / 10.0;
            siji.Add((waktu), phase);
            loro.Add((waktu), phase1);

            Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
            if (waktu > xScale.Max - xScale.MajorStep)
            {
                if (intMode == 1)
                {
                    xScale.Max = waktu + xScale.MajorStep;
                    xScale.Min = xScale.Max - 30.0;
                }
                else
                {
                    xScale.Max = waktu + xScale.MajorStep;
                    xScale.Min = 0;
                }

                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            } 
        }
    }
}
