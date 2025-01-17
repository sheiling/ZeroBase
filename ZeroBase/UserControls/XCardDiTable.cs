﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ZeroBase
{
    public partial class XCardDiTable : UserControl
    {
        public bool EnableTimer = false;
        private Timer m_Timer;
        private string cardname = "cardname";
        private int cardid = 0;
        private string[] colHead = new string[] { "", "通道位置", "通道号", "输入点名称" };
        private Dictionary<int, bool> stsMap = new Dictionary<int, bool>();
        private Dictionary<int, bool> lastStsMap = new Dictionary<int, bool>();
        private Dictionary<int, int> SetId = new Dictionary<int, int>();

        public XCardDiTable()
        {
            InitializeComponent();
            InitialDgv();
            InitialTimer();
        }
        public string CardName
        {
            get { return cardname; }
            set
            {
                cardname = value;
                try
                {
                    if (XDevice.Instance.CardMap[cardid] != null)
                    {
                        if (XDevice.Instance.CardMap[cardid].Name == cardname)
                        {
                            this.groupBox1.Text = cardname;
                            this.toolStripStatusLabel1.Text = cardname;
                            dataGridView1.Rows.Clear();
                            stsMap.Clear();
                            lastStsMap.Clear();
                            foreach (KeyValuePair<int, XDi> kvp in XDevice.Instance.DiMap)
                            {
                                if (kvp.Value.CardName == cardname)
                                {
                                    DataGridViewRow dr = new DataGridViewRow();
                                    dr.Cells.Add(new DataGridViewImageCell());
                                    dr.Cells.Add(new DataGridViewTextBoxCell());
                                    dr.Cells.Add(new DataGridViewTextBoxCell());
                                    dr.Cells.Add(new DataGridViewTextBoxCell());

                                    dr.Cells[0].Value = Properties.Resources._lampGray20;
                                    if (kvp.Value.ActId < 8)
                                    {
                                        dr.Cells[1].Value = "扩展通道_EDI";
                                        dr.Cells[2].Value = kvp.Value.ActId + 1;

                                    }
                                    else
                                    {
                                        dr.Cells[1].Value = "常规通道_DI";
                                        dr.Cells[2].Value = kvp.Value.ActId - 7;
                                    }
                                    if (SetId.ContainsKey(kvp.Value.ActId) == false)
                                    {
                                        SetId.Add(kvp.Value.ActId, kvp.Value.SetId);
                                    }
                                    dr.Cells[3].Value = kvp.Value.Name;

                                    dataGridView1.Rows.Add(dr);
                                    stsMap.Add(kvp.Key, false);
                                    lastStsMap.Add(kvp.Key, false);
                                }
                            }
                        }
                        else
                        {
                            this.toolStripStatusLabel1.Text = "卡号与卡名不对应";
                        }
                    }

                }
                catch
                {
                }
            
  



            }
        }

        public int CardID
        {
            get { return cardid; }
            set { cardid = value; }
        }
        private void InitialTimer()
        {
            m_Timer = new Timer();
            m_Timer.Interval = 300;
            m_Timer.Tick += new EventHandler(m_Timer_Tick);
            //EnableTimer = true;
            m_Timer.Start();
        }
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
            dataGridView1.Columns[1].Width = 70;
            dataGridView1.Columns[2].Width = 50;

        }
        private void m_Timer_Tick(object sender, EventArgs e)
        {
            if (EnableTimer)
            {
                try
                {
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        if (dr.Cells[1].Value.ToString().Contains("扩展通道_EDI"))
                        {
                            int diId = SetId[int.Parse(dr.Cells[2].Value.ToString()) - 1];
                            stsMap[diId] = XDevice.Instance.FindDiById(diId).STS;
                            if (stsMap[diId] == true && lastStsMap[diId] == false)
                            {
                                dr.Cells[0].Value = Properties.Resources._lampGreen20;
                            }
                            else if (stsMap[diId] == false && lastStsMap[diId] == true)
                            {
                                dr.Cells[0].Value = Properties.Resources._lampGray20;
                            }
                            bool temp = stsMap[diId];
                            lastStsMap[diId] = temp;
                        }
                        else
                        {
                            int diId = SetId[int.Parse(dr.Cells[2].Value.ToString()) + 7];
                            stsMap[diId] = XDevice.Instance.FindDiById(diId).STS;
                            if (stsMap[diId] == true && lastStsMap[diId] == false)
                            {
                                dr.Cells[0].Value = Properties.Resources._lampGreen20;
                            }
                            else if (stsMap[diId] == false && lastStsMap[diId] == true)
                            {
                                dr.Cells[0].Value = Properties.Resources._lampGray20;
                            }
                            bool temp = stsMap[diId];
                            lastStsMap[diId] = temp;
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
