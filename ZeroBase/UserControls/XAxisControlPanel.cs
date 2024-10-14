using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ZeroBase
{
    public partial class XAxisControlPanel : UserControl
    {
        private int m_AxisId;
        private int m_JogStart;
        private IAxis m_Axis;
        private double m_Distance;
        private double m_Vel;
        private double m_Acc = 1000;
        private Timer m_Timer;
        private int taskId = 1;
        private Dictionary<int, PictureBox> pictureBoxMap = new Dictionary<int, PictureBox>();
        public bool EnableTimer = false;
        public XAxisControlPanel()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            pictureBoxMap.Add(0, PB_SVON);
            pictureBoxMap.Add(1, PB_MEL);
            pictureBoxMap.Add(2, PB_ORG);
            pictureBoxMap.Add(3, PB_PEL);
            pictureBoxMap.Add(4, PB_ALM);
            pictureBoxMap.Add(5, PB_ASTP);
            foreach (PictureBox pb in pictureBoxMap.Values)
            {
                pb.BackgroundImageLayout = ImageLayout.Center;
                pb.BackgroundImage = Properties.Resources._lampGray20;
            }
            LB_Home.Text = "未初始化";
            LB_Home.ForeColor = Color.Red;
            InitialDistance();
            InitialVel();
            InitialTimer();
            this.textBox_Acc.Text = m_Acc.ToString();
        }
        
        public int TaskId
        {
            get { return this.taskId; }
            set
            {
                this.taskId = value;
                if (XTaskManager.Instance.FindTaskById(taskId) != null)
                {
                    this.toolStripStatusLabel1.Text = XTaskManager.Instance.FindTaskById(taskId).Name;
                    Comb_AxisNo.Items.Clear();
                    foreach (KeyValuePair<int, IAxis> kvp in XTaskManager.Instance.FindTaskById(taskId).AxisMap)
                    {
                        Comb_AxisNo.Items.Add(kvp.Value.Name);
                    }
                    if (Comb_AxisNo.Items.Count > 0)
                    {
                        Comb_AxisNo.SelectedIndex = 0;
                    }

                }
            }
        }

        private void InitialTimer()
        {
            m_Timer = new Timer();
            m_Timer.Interval = 200;
            m_Timer.Tick += new EventHandler(m_Timer_Tick);
            //EnableTimer = true;
            m_Timer.Start();
        }

        private void InitialDistance()
        {
            Comb_Distance.Items.Add(0.001);
            Comb_Distance.Items.Add(0.002);
            Comb_Distance.Items.Add(0.005);
            Comb_Distance.Items.Add(0.01);
            Comb_Distance.Items.Add(0.02);
            Comb_Distance.Items.Add(0.05);
            Comb_Distance.Items.Add(0.1);
            Comb_Distance.Items.Add(0.2);
            Comb_Distance.Items.Add(0.5);
            Comb_Distance.Items.Add(1);
            Comb_Distance.Items.Add(2);
            Comb_Distance.Items.Add(5);
            Comb_Distance.Items.Add(10);
            Comb_Distance.Items.Add(20);
            Comb_Distance.Items.Add(30);
            Comb_Distance.Items.Add(45);
            Comb_Distance.Items.Add(50);
            Comb_Distance.Items.Add(60);
            Comb_Distance.Items.Add(90);
            Comb_Distance.Items.Add(100);
            Comb_Distance.Items.Add(120);
            Comb_Distance.Items.Add(135);
            Comb_Distance.Items.Add(150);
            Comb_Distance.Items.Add(180);
            Comb_Distance.Items.Add(200);
            //Comb_Distance.Items.Add(270);
            //Comb_Distance.Items.Add(300);
            //Comb_Distance.Items.Add(360);
            //Comb_Distance.Items.Add(400);
            //Comb_Distance.Items.Add(500);
            Comb_Distance.SelectedIndex = 9;
        }

        private void InitialVel()
        {
            this.Bar_Vel.SetRange(0, 200);
            this.Bar_Vel.Value = 10;
            this.label1.Text = "10mm/s";
            m_Vel = 10;
        }

        private void Bar_Vel_Scroll(object sender, EventArgs e)
        {
            label1.Text = Bar_Vel.Value.ToString() + "mm/s";
            m_Vel = Bar_Vel.Value;
        }

        private void Comb_Distance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Comb_Distance.Items.Count > 0)
            {
                m_Distance = double.Parse(Comb_Distance.SelectedItem.ToString());
            }
        }

        private void Comb_AxisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Comb_AxisNo.Items.Count > 0)
            {
                foreach (KeyValuePair<int, IAxis> kvp in XTaskManager.Instance.FindTaskById(taskId).AxisMap)
                {
                    if (kvp.Value.Name == Comb_AxisNo.SelectedItem.ToString())
                    {
                        m_AxisId = kvp.Key;
                    }
                }
                m_Axis = XDevice.Instance.FindAxisById(m_AxisId);
                ChangeButton_Image(m_Axis);
            }
        }
        private void ChangeButton_Image(IAxis axisNow)
        {
            switch (axisNow.AxisDirection)
            {
                case XAxisDirection.Front_Back:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._up;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._down;
                    break;
                case XAxisDirection.Back_Front:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._down;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._up;
                    break;
                case XAxisDirection.Left_Right:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._right;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._left;
                    break;
                case XAxisDirection.Right_Left:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._left;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._right;
                    break;
                case XAxisDirection.Rotate:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._rotate_clock;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._rotate_antiClock;
                    break;
                case XAxisDirection.Rotate_antiClock:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._rotate_antiClock;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._rotate_clock;
                    break;
                case XAxisDirection.Up_Down:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._up;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._down;
                    break;
                case XAxisDirection.Down_Up:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._down;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._up;
                    break;
                default:
                    this.Btn_Forward.Image = ZeroBase.Properties.Resources._minus;
                    this.Btn_Back.Image = ZeroBase.Properties.Resources._minus;
                    break;
            }
        }
        private void Btn_Forward_Click(object sender, EventArgs e)
        {
            if (Comb_AxisNo.Items.Count > 0)
            {
                if (m_Axis.IsHomeOk == false)
                {
                    //return;
                }
                if (XDevice.Instance.FindAxisById(m_AxisId).SetAxisAccAndDec(m_Acc, m_Acc) != 0)
                {
                    return;
                }
                XController.Instance._MoveRel(m_AxisId, m_Distance, m_Vel);
            }
        }

        private void Btn_Back_Click(object sender, EventArgs e)
        {
            if (Comb_AxisNo.Items.Count > 0)
            {
                if (m_Axis.IsHomeOk == false)
                {
                    //return;
                }
                if (XDevice.Instance.FindAxisById(m_AxisId).SetAxisAccAndDec(m_Acc, m_Acc) != 0)
                {
                    return;
                }
                XController.Instance._MoveRel(m_AxisId, -m_Distance, m_Vel);
            }
        }

        private void Btn_Home_Click(object sender, EventArgs e)
        {
            if (Comb_AxisNo.Items.Count > 0)
            {
                XController.Instance._MoveHome(m_AxisId);
            }
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (Comb_AxisNo.Items.Count > 0)
            {
                XController.Instance._MoveStop();
            }
        }

        private void PB_SVON_Click(object sender, EventArgs e)
        {
            if (m_Axis.IsSVON)
            {
                XDevice.Instance.FindAxisById(m_AxisId).SetServo(false);
            }
            else
            {
                XDevice.Instance.FindAxisById(m_AxisId).SetServo(true);
            }
        }

        private void m_Timer_Tick(object sender, EventArgs e)
        {
            if (EnableTimer)
            {
                try
                {
                    if (Comb_AxisNo.Items.Count > 0)
                    {
                        LB_AxisNo.Text = m_AxisId.ToString() + ":";
                        if (m_Axis.IsFeedback)
                        {
                            LB_Pos.Text = m_Axis.POS.ToString("f3");
                        }
                        else
                        {
                            LB_Pos.Text = m_Axis.CommandPOS.ToString("f3");
                        }

                        bool[] axisSts = new bool[] { m_Axis.IsSVON, m_Axis.IsMEL, m_Axis.IsORG, m_Axis.IsPEL, 
                            m_Axis.IsALM, m_Axis.IsASTP, m_Axis.IsHomeOk };
                        for (int i = 0; i < 7; i++)
                        {
                            if (i < 6)
                            {
                                if (axisSts[i] == true)
                                {
                                    if (i == 0)
                                    {
                                        pictureBoxMap[i].BackgroundImage = Properties.Resources._lampGreen20;
                                    }
                                    else
                                    {
                                        pictureBoxMap[i].BackgroundImage = Properties.Resources._lampRed20;
                                    }
                                }
                                else
                                {
                                    pictureBoxMap[i].BackgroundImage = Properties.Resources._lampGray20;
                                }
                            }
                            else
                            {
                                if (axisSts[i] == true)
                                {
                                    LB_Home.Text = "已初始化";
                                    LB_Home.ForeColor = Color.Black;
                                }
                                else
                                {
                                    LB_Home.Text = "未初始化";
                                    LB_Home.ForeColor = Color.Red;
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void textBox_Acc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                m_Acc = double.Parse(this.textBox_Acc.Text.Trim());
            }
            catch
            {
                m_Acc = 1000;
                this.textBox_Acc.Text = "1000";
            }
        }


        private void bt_JOG_P_MouseDown(object sender, MouseEventArgs e)
        {
            if (Comb_AxisNo.Items.Count > 0)
            {
                if (m_Axis.IsHomeOk == false)
                {
                    return;
                }
                if (XDevice.Instance.FindAxisById(m_AxisId).APS_SetAxisJogParam(1, 0, m_Acc, 
                        m_Acc, (int)m_Vel) != 0)
                {
                    return;
                }
                m_JogStart = 1;
                XController.Instance._MoveJog(m_AxisId, m_JogStart);
            }
        }

        private void bt_JOG_P_MouseUp(object sender, MouseEventArgs e)
        {
            m_JogStart = 0;
            XController.Instance._MoveJog(m_AxisId, m_JogStart);
        }

        private void bt_JOG_N_MouseDown(object sender, MouseEventArgs e)
        {
            if (Comb_AxisNo.Items.Count > 0)
            {
                if (m_Axis.IsHomeOk == false)
                {
                    return;
                }
                if (XDevice.Instance.FindAxisById(m_AxisId).APS_SetAxisJogParam(1, 1, m_Acc, m_Acc, (int)m_Vel) != 0)
                {
                    return;
                }
                m_JogStart = 1;
                XController.Instance._MoveJog(m_AxisId, m_JogStart);
            }
        }

        private void bt_JOG_N_MouseUp(object sender, MouseEventArgs e)
        {
            m_JogStart = 0;
            XController.Instance._MoveJog(m_AxisId, m_JogStart);
        }


        private void uxEvent(object sender, EventArgs e)
        {
            //do something(including send message to other user controls)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XController.Instance._setPosition((ushort)m_AxisId, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XController.Instance._CleanALM((ushort) m_AxisId);
        }
       










    }
}
