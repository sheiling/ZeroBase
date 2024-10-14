using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ZeroBase
{
    public partial class XTimeoutTip : Form
    {
        private Color Red = Color.FromArgb(0xC8, 0x25, 0x06);
        private Color Green = Color.FromArgb(0xAE, 0xDA, 0x97);
        private ManualResetEvent _mre = new ManualResetEvent(false);
        public XTimeoutTip(string message)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.button1.BackColor = Green;
            this.BackColor = Red;
            this.richTextBox1.Text = message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            _mre.Set();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            _mre.Set();
            this.Close();
        }

        public bool WaitOne()
        {
            return _mre.WaitOne();
        }

        public bool Reset()
        {
            return _mre.Reset();
        }

        public bool Set()
        {
            return _mre.Set();
        }

       



    }
}
