using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ZeroBase
{
    public partial class XSettingGrid : UserControl
    {
        private int id = 1;
        private bool locked = false;
        public EventHandler UpdateAction;
        private const string LogPath = "D:\\Record\\SettingLog\\";
        public XSettingGrid()
        {
            InitializeComponent();
        }
        public bool Locked
        {
            get { return this.locked; }
            set { this.locked = value; }
        }
        public int Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                this.propertyGrid1.SelectedObject = XSettingManager.Instance.FindSettingById(this.id);
                if (XSettingManager.Instance.FindSettingById(this.id) != null)
                {
                    this.toolStripLabel1.Text = XSettingManager.Instance.FindSettingById(this.id).Name;
                }
            }
        }

        private void RefreshUi()
        {
            this.BeginInvoke(new Action(() =>
            {
                Id = id;
            }));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            if (XMachine.Instance.MachineMode != MachineModeType.Engineering)
            {
                // return;
            }
            if (MessageBox.Show(this, "确认保存[" + XSettingManager.Instance.FindSettingById(this.id).Name + "]？", "询问",
                  MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                XSettingManager.Instance.FindSettingById(this.id).SaveSetting();
                if (UpdateAction != null)
                    UpdateAction(this, null);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = XSettingManager.Instance.FindSettingById(this.id);
            if (XSettingManager.Instance.FindSettingById(this.id) != null)
            {
                this.toolStripLabel1.Text = XSettingManager.Instance.FindSettingById(this.id).Name;
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (Directory.Exists(LogPath) == false)
            {
                Directory.CreateDirectory(LogPath);
            }
            string path = LogPath + XSettingManager.Instance.FindSettingById(this.id).Name + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            string currenttype = e.OldValue.GetType().ToString();
            string message = "";
            if (currenttype.Contains("[]"))
            {

            }
            else
            {

                message = DateTime.Now.ToString("HH:mm:ss:fff") + " => " + e.ChangedItem.Parent.Label + e.ChangedItem.Label + ":" + e.OldValue.ToString() + " => " + e.ChangedItem.Value.ToString();
            }
            StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
            sw.WriteLine(message);
            sw.Dispose();
        }
    }
}
