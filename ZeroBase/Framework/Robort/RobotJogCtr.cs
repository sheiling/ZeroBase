using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZeroBase
{
    public partial class RobotJogCtr : UserControl
    {
        public RobotJogCtr()
        {
            InitializeComponent();
        }

        private int robotId = 1;
        private int robotAxisCount = 4;

        public int RobotId
        {
            get { return this.robotId; }
            set
            {
                this.robotId = value;
            }
        }

        public int RobotAxisCount
        {
            get { return robotAxisCount; }
            set
            {
                robotAxisCount = value;
            }
        }
        private void RobotCtr_Load(object sender, EventArgs e)
        {

            if (!IsHandleCreated)
            {
                return;
            }
            timer1.Interval = 500;
            timer1.Start();
            if (RobotAxisCount == 4)
            {
                label5.Visible = false;
                label3.Visible = false;
                txtA.Visible = false;
                txtB.Visible = false;
                Badd.Visible = false;
                Bminus.Visible = false;
                Cadd.Visible = false;
                Cminus.Visible = false;
                Aadd.Text = "+U";
                Aminus.Text = "-U";
            }
            if (RobotManger.Instance.FindRobotCtrById(robotId) == null)
            {
                return;
            }

            var type = RobotManger.Instance.FindRobotCtrById(robotId).Command.GetType();

            if (type.Name == "MitsubishiRobot")
            {

            }
            if (type.Name == "ABBRobot")
            {
                checkBox1.Visible = false;
            }
            if (type.Name == "EpsonRobot")
            {

            }

        }


        private void tbVel_Scroll(object sender, EventArgs e)
        {
            label1Speed.Text = tbVel.Value + "%(1-20)";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Run(new Action(() =>
            {
                foreach (var station in XStationManager.Instance.Stations.Values)
                {
                    if (station.State == XStationState.RUNNING || station.State == XStationState.RESETING || station.State == XStationState.PAUSE || station.State == XStationState.WAITRUN)
                        station.Stop();
                }
            }));

            var btn = (Button)sender;

            var cmd = "MVS";

            var rel = Convert.ToDouble(numberBox1.Text);


            var dir = btn.Text.Substring(0, 1);
            var axisid = btn.Text.Substring(1, 1);
            if (dir == "-")
            {
                rel = -rel;
            }
            cmd = cmd + "," + axisid + "," + tbVel.Value + "," + rel;


            Task.Factory.StartNew(new Action(() => RobotManger.Instance.FindRobotCtrById(robotId).Command.TriggerAndGetData(cmd, true)));
        }



        private void radioButton1_Click(object sender, EventArgs e)
        {
            var mm = (RadioButton)sender;
            numberBox1.Text = mm.Text.Replace("mm", "");
        }



        private void btnGetPos_Click(object sender, EventArgs e)
        {

            var pos = new double[] { };
            RobotManger.Instance.FindRobotCtrById(robotId).Command.GetRobotPos(out pos);

            var txt = new TextBox[] { txtX, txtY, txtZ, txtA, txtB, txtC };
            if (robotAxisCount == 4)
            {
                txtX.Text = pos[0].ToString("0.000");
                txtY.Text = pos[1].ToString("0.000");
                txtZ.Text = pos[2].ToString("0.000");
                txtC.Text = pos[3].ToString("0.000");
            }
            if (robotAxisCount == 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    txt[i].Text = pos[i].ToString("0.000");
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Run(new Action(() =>
            {
                foreach (var station in XStationManager.Instance.Stations.Values)
                {
                    if (station.State == XStationState.RUNNING || station.State == XStationState.RESETING || station.State == XStationState.PAUSE || station.State == XStationState.WAITRUN)
                        station.Stop();
                }
            }));

            RobotManger.Instance.FindRobotCtrById(robotId).Command.ResetByIO();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            RobotManger.Instance.FindRobotCtrById(robotId).Command.StopByIO();

        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            RobotManger.Instance.FindRobotCtrById(robotId).Command.SatrtByIO();
        }

        private void btnServo_Click(object sender, EventArgs e)
        {
            RobotManger.Instance.FindRobotCtrById(robotId).Command.ServoOffByIO();

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                RobotManger.Instance.FindRobotCtrById(robotId).Command.AuthorizationByIO(1);
            }
            else
            {
                RobotManger.Instance.FindRobotCtrById(robotId).Command.AuthorizationByIO(0);
            }



        }
        public void RefreshUi()
        {
            if (!IsHandleCreated)
            {
                return;
            }
            if (Visible)
            {
                BeginInvoke(new Action(() =>
                {
                    int ioid = RobotManger.Instance.FindRobotCtrById(robotId).Command.RobortIO.DoServo;
                    btnServo.BackColor = XDevice.Instance.FindDoById(ioid).STS ? BackColor = Color.Lime : Color.Gray;
                  
                    label4.BackColor = RobotManger.Instance.FindRobotCtrById(robotId).Command.Alarm ? BackColor = Color.Red : Color.Lime;
                    this.BackColor = DefaultBackColor;


                    var type = RobotManger.Instance.FindRobotCtrById(robotId).Command.GetType();

                    if (type.Name == "MitsubishiRobot")
                    {

                    }
                    if (type.Name == "ABBRobot")
                    {
                        checkBox1.Visible = false;
                    }
                    if (type.Name == "EpsonRobot")
                    {
                        ioid = RobotManger.Instance.FindRobotCtrById(robotId).Command.RobortIO.DoAuthorization;
                        checkBox1.Checked = XDevice.Instance.FindDoById(ioid).STS;
                    }


                }));
            }
        }

        private void btnServo_CheckedChanged(object sender, EventArgs e)
        {
            RobotManger.Instance.FindRobotCtrById(robotId).Command.ServoOnByIO();
        }

        private void Bminus_Click(object sender, EventArgs e)
        {

        }

        private void Cminus_Click(object sender, EventArgs e)
        {

        }

    }
}
