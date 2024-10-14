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
    public partial class RobortDo : UserControl
    {
        private int robortid = 1;
        public RobortDo()
        {
            InitializeComponent();
            InitialDgv();
        }
        public int RobortId
        {
            get
            {
                return this.robortid;
            }
            set
            {
                this.robortid = value;
                if (RobotManger.Instance.FindRobotCtrById(robortid) != null)
                {
                    this.toolStripStatusLabel1.Text = RobotManger.Instance.FindRobotCtrById(robortid).Name;
                    dataGridView1.Rows.Clear();
                    foreach (KeyValuePair<int, XRobortDo> kvp in RobotManger.Instance.FindRobotCtrById(robortid).RobortDoMap)
                    {
                        DataGridViewRow dr = new DataGridViewRow();
                        dr.Cells.Add(new DataGridViewImageCell());
                        dr.Cells.Add(new DataGridViewButtonCell());
                        dr.Cells.Add(new DataGridViewTextBoxCell());
                        dr.Cells.Add(new DataGridViewTextBoxCell());
                        dr.Cells.Add(new DataGridViewTextBoxCell());

                        dr.Cells[0].Value = Properties.Resources._lampGray20;
                        dr.Cells[2].Value = kvp.Key;
                        dr.Cells[3].Value = kvp.Value.ActId;
                        dr.Cells[4].Value = kvp.Value.Name;

                        dataGridView1.Rows.Add(dr);
                    }
                }
            }
        }
        private string[] colHead = new string[] { "", "", "SetId", "ActId", "Name" };
        private void InitialDgv()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Columns.Add(new DataGridViewImageColumn());
            dataGridView1.Columns.Add(new DataGridViewButtonColumn());
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());

            for (int i = 0; i < 5; i++)
            {
                dataGridView1.Columns[i].HeaderText = colHead[i];
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.Columns[0].Width = 20;
            dataGridView1.Columns[1].Width = 20;
            dataGridView1.Columns[2].Width = 20;
            dataGridView1.Columns[3].Width = 20;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                System.Threading.Tasks.Task.Run(new Action(() =>
                {
                    foreach (var station in XStationManager.Instance.Stations.Values)
                    {
                        if (station.State == XStationState.RUNNING || station.State == XStationState.RESETING || station.State == XStationState.PAUSE)
                            station.Stop();
                        else if (station.State == XStationState.WAITRUN)
                            ;
                            //station.SetState(XStationState.WAITRESET);
                    }
                }));
                int doId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                XRobortDo currentDo = RobotManger.Instance.FindRobotDoById(doId);
                currentDo.Update();
                if (currentDo.STS == true)
                {
                    RobotManger.Instance.FindRobotDoById(doId).SetDo(0);
                }
                else
                {
                    RobotManger.Instance.FindRobotDoById(doId).SetDo(1);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    int doId = int.Parse(dr.Cells[2].Value.ToString());
                    RobotManger.Instance.FindRobotCtrById(robortid).RobortDoMap[doId].Update();
                    if (RobotManger.Instance.FindRobotCtrById(robortid).RobortDoMap[doId].STS)
                    {
                        dr.Cells[0].Value = Properties.Resources._lampGreen20;
                    }
                    else
                    {
                        dr.Cells[0].Value = Properties.Resources._lampGray20;
                    }
                }
            }
            catch
            {

            }
        }

    }
}
