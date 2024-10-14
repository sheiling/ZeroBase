namespace XCore.UserControls
{
    partial class FrmPostion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xPositionTable1 = new XCore.XPositionTable();
            this.SuspendLayout();
            // 
            // xPositionTable1
            // 
            this.xPositionTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPositionTable1.Location = new System.Drawing.Point(0, 0);
            this.xPositionTable1.Name = "xPositionTable1";
            this.xPositionTable1.Size = new System.Drawing.Size(585, 256);
            this.xPositionTable1.TabIndex = 0;
            this.xPositionTable1.TaskId = 1;
            // 
            // FrmPostion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 256);
            this.Controls.Add(this.xPositionTable1);
            this.Name = "FrmPostion";
            this.Text = "点位";
            this.ResumeLayout(false);

        }

        #endregion

        private XPositionTable xPositionTable1;
    }
}