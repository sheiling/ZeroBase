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
    public partial class XAxisStatusTable : UserControl
    {
        public bool EnableTimer = false;
        private Timer m_Timer = new Timer();
        private int taskId = 1;
        private Dictionary<int, bool[]> stsMap = new Dictionary<int, bool[]>();
        private Dictionary<int, bool[]> lastStsMap = new Dictionary<int, bool[]>();
        public XAxisStatusTable()
        {
            InitializeComponent();
            InitialDgv();
            InitialTimer();
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
                    dataGridView1.Rows.Clear();
                    stsMap.Clear();
                    lastStsMap.Clear();
                    foreach (KeyValuePair<int, IAxis> kvp in XTaskManager.Instance.FindTaskById(taskId).AxisMap)
                    {
                        DataGridViewRow dr = new DataGridViewRow();
                        dr.Cells.Add(new DataGridViewTextBoxCell());
                        dr.Cells.Add(new DataGridViewTextBoxCell());
                        dr.Cells.Add(new DataGridViewTextBoxCell());
                        dr.Cells[0].Value = kvp.Key;
                        dr.Cells[1].Value = kvp.Value.Name;
                        dr.Cells[2].Value = 0;
                        for (int i = 3; i < 11; i++)
                        {
                            dr.Cells.Add(new DataGridViewImageCell());
                            dr.Cells[i].Value = Properties.Resources._lampGray20;
                        }
                        dataGridView1.Rows.Add(dr);
                        stsMap.Add(kvp.Key, new bool[8] { false, false, false, false, false, false, false, false });
                        lastStsMap.Add(kvp.Key, new bool[8] { false, false, false, false, false, false, false, false });
                    }
                }
            }
        }

        private string[] ColumnsHeaderText = new string[11] { "Id", "Name", "POS", "SVON", "HMOK", "MDN", "MEL", "ORG", "PEL", "ALM", "ASTP" };
        private int[] ColumnsWidth = new int[8] { 22, 22, 22, 22, 22, 22, 22, 30 };

        private void InitialDgv()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
            for (int i = 3; i < 11; i++)
            {
                dataGridView1.Columns.Add(new DataGridViewImageColumn());
            }
            dataGridView1.Columns[0].Width = 15;
            for (int i = 0; i < 11; i++)
            {
                dataGridView1.Columns[i].HeaderText = ColumnsHeaderText[i];
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (i > 2)
                {
                    dataGridView1.Columns[i].Width = ColumnsWidth[i - 3];
                }
            }
           
        }

        private void InitialTimer()
        {
            m_Timer.Interval = 200;
            m_Timer.Tick+=new EventHandler(m_Timer_Tick);
           // EnableTimer = true;
            m_Timer.Start();
        }
       
        private void m_Timer_Tick(object sender, EventArgs e)
        {
            if (EnableTimer)
            {
                try
                {
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        int axisId = int.Parse(dr.Cells[0].Value.ToString());
                        if (XDevice.Instance.FindAxisById(axisId).IsFeedback)
                        {
                            dr.Cells[2].Value = XDevice.Instance.FindAxisById(axisId).POS.ToString("f3");
                        }
                        else
                        {
                            dr.Cells[2].Value = XDevice.Instance.FindAxisById(axisId).CommandPOS.ToString("f3");
                        }
                        stsMap[axisId] = new bool[8]{ XDevice.Instance.FindAxisById(axisId).IsSVON,
                                                  XDevice.Instance.FindAxisById(axisId).IsHomeOk,
                                                  XDevice.Instance.FindAxisById(axisId).IsMDN,
                                                  XDevice.Instance.FindAxisById(axisId).IsMEL,
                                                  XDevice.Instance.FindAxisById(axisId).IsORG,
                                                  XDevice.Instance.FindAxisById(axisId).IsPEL,
                                                  XDevice.Instance.FindAxisById(axisId).IsALM,
                                                  XDevice.Instance.FindAxisById(axisId).IsASTP};
                        for (int i = 0; i < 8; i++)
                        {
                            if (stsMap[axisId][i] == true && lastStsMap[axisId][i] == false)
                            {
                                if (i < 3)
                                {
                                    dr.Cells[i + 3].Value = Properties.Resources._lampGreen20;
                                }
                                else
                                {
                                    dr.Cells[i + 3].Value = Properties.Resources._lampRed20;
                                }
                            }
                            else if (stsMap[axisId][i] == false && lastStsMap[axisId][i] == true)
                            {
                                dr.Cells[i + 3].Value = Properties.Resources._lampGray20;
                            }
                        }
                        lastStsMap[axisId] = new bool[8] { false, false, false, false, false, false, false, false };
                        for (int i = 0; i < 8; i++)
                        {
                            lastStsMap[axisId][i] = stsMap[axisId][i];
                        }
                    }
                }
                catch
                {

                }
            }
            
        }
    }
}
