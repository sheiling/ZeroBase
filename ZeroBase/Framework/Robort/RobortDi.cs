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
    public partial class RobortDi : UserControl
    {
        private int robortid = 1;
        public RobortDi()
        {
            InitializeComponent();
            InitialDgv();
        }
        public int RobortId
        {
            get { return this.robortid; }
            set
            {
                robortid = value;
                if (RobotManger.Instance.FindRobotCtrById(robortid) != null)
                {
                    this.toolStripStatusLabel1.Text = RobotManger.Instance.FindRobotCtrById(robortid).Name;
                    dataGridView1.Rows.Clear();
                    foreach (KeyValuePair<int, XRobortDi> kvp in RobotManger.Instance.FindRobotCtrById(robortid).RobortDiMap)
                    {
                        DataGridViewRow dr = new DataGridViewRow();
                        dr.Cells.Add(new DataGridViewImageCell());
                        dr.Cells.Add(new DataGridViewTextBoxCell());
                        dr.Cells.Add(new DataGridViewTextBoxCell());
                        dr.Cells.Add(new DataGridViewTextBoxCell());

                        dr.Cells[0].Value = Properties.Resources._lampGray20;
                        dr.Cells[1].Value = kvp.Key;
                        dr.Cells[2].Value = kvp.Value.ActId;
                        dr.Cells[3].Value = kvp.Value.Name;

                        dataGridView1.Rows.Add(dr);
                    }
                }
            }
        }
        private string[] colHead = new string[] { "", "SetId", "ActId", "Name" };
        private void InitialDgv()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Columns.Add(new DataGridViewImageColumn());
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());

            for (int i = 0; i < 4; i++)
            {
                dataGridView1.Columns[i].HeaderText = colHead[i];
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.Columns[0].Width = 20;
            dataGridView1.Columns[1].Width = 20;
            dataGridView1.Columns[2].Width = 20;
            dataGridView1.Columns[3].Width = 20;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    int diId = int.Parse(dr.Cells[1].Value.ToString());
                    RobotManger.Instance.FindRobotCtrById(robortid).RobortDiMap[diId].Update();
                    if (RobotManger.Instance.FindRobotCtrById(robortid).RobortDiMap[diId].STS)
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
