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
    public partial class XDoTable : UserControl
    {
        public bool EnableTimer = false;
        private Timer m_Timer;
        private int taskId = 1;
        private Dictionary<int, bool> stsMap = new Dictionary<int, bool>();
        private Dictionary<int, bool> lastStsMap = new Dictionary<int, bool>();
        public XDoTable()
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
                    foreach (KeyValuePair<int, XDo> kvp in XTaskManager.Instance.FindTaskById(taskId).DoMap)
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
                        stsMap.Add(kvp.Key, false);
                        lastStsMap.Add(kvp.Key, false);
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

        private string[] colHead = new string[] { "", "", "SetId", "ActId", "Name"};
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
            dataGridView1.Columns[2].Width = 30;
            dataGridView1.Columns[3].Width = 30;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int doId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                XDo currentDo = XDevice.Instance.FindDoById(doId);
                if (currentDo.STS == true)
                {
                    XDevice.Instance.FindDoById(doId).SetDo(0);
                }
                else
                {
                    XDevice.Instance.FindDoById(doId).SetDo(1);
                }
            }
           
        }

        private void m_Timer_Tick(object sender, EventArgs e)
        {
            if (EnableTimer)
            {
                try
                {
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        int doId = int.Parse(dr.Cells[2].Value.ToString());
                        stsMap[doId] = XDevice.Instance.FindDoById(doId).STS;
                        if (stsMap[doId] == true && lastStsMap[doId] == false)
                        {
                            dr.Cells[0].Value = Properties.Resources._lampBlue20;
                        }
                        else if (stsMap[doId] == false && lastStsMap[doId] == true)
                        {
                            dr.Cells[0].Value = Properties.Resources._lampGray20;
                        }
                        bool temp = stsMap[doId];
                        lastStsMap[doId] = temp;
                    }
                }
                catch
                {

                }
            }
        }

    }
}
