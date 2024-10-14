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
    public partial class XTaskStepBar : UserControl
    {
        private int taskId = 1;
        private string taskName;
        public XTaskStepBar()
        {
            InitializeComponent();
        }

        public int TaskId
        {
            get { return taskId; }
            set 
            {
                taskId = value;
                if (XTaskManager.Instance.FindTaskById(taskId) == null)
                {
                    return;
                }
                taskName = XTaskManager.Instance.FindTaskById(taskId).Name;
                button1.Text = taskName + ":Step";
                XTaskManager.Instance.FindTaskById(taskId).OnStep += new Action<string, Color>(BeginShow);
            }
        }

        public void BeginShow(string step, Color color)
        {
            try
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action<string, Color>(Show), new object[] { step, color });
                }
            }
            catch
            {

            }
        }

        private void Show(string step, Color color)
        {
            button1.Text = taskName + ":" + step;
            button1.BackColor = color;
        }
    }
    
}
