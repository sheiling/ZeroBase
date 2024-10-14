using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XCore.UserControls
{
    public partial class FrmPostion : Form
    {
        public FrmPostion(int taskid, string name)
        {
            InitializeComponent();
            xPositionTable1.TaskId = taskid;
            xPositionTable1.Posname = name + "," + name;
        }


    }
}
