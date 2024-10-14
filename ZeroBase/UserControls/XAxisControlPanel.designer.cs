namespace ZeroBase
{
    partial class XAxisControlPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XAxisControlPanel));
            this.Bar_Vel = new System.Windows.Forms.TrackBar();
            this.Comb_Distance = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Btn_Home = new System.Windows.Forms.Button();
            this.Btn_Forward = new System.Windows.Forms.Button();
            this.Btn_Back = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.LB_Home = new System.Windows.Forms.ToolStripStatusLabel();
            this.LB_AxisNo = new System.Windows.Forms.ToolStripStatusLabel();
            this.LB_Pos = new System.Windows.Forms.ToolStripStatusLabel();
            this.PB_MEL = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.PB_ORG = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.PB_PEL = new System.Windows.Forms.PictureBox();
            this.PB_ALM = new System.Windows.Forms.PictureBox();
            this.PB_ASTP = new System.Windows.Forms.PictureBox();
            this.PB_SVON = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Comb_AxisNo = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.bt_JOG_N = new System.Windows.Forms.Button();
            this.bt_JOG_P = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_Acc = new System.Windows.Forms.TextBox();
            this.Btn_Stop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_Vel)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MEL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ORG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_PEL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ALM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ASTP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_SVON)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Bar_Vel
            // 
            this.Bar_Vel.Location = new System.Drawing.Point(12, 67);
            this.Bar_Vel.Margin = new System.Windows.Forms.Padding(4);
            this.Bar_Vel.Name = "Bar_Vel";
            this.Bar_Vel.Size = new System.Drawing.Size(167, 45);
            this.Bar_Vel.TabIndex = 1;
            this.Bar_Vel.Scroll += new System.EventHandler(this.Bar_Vel_Scroll);
            // 
            // Comb_Distance
            // 
            this.Comb_Distance.FormattingEnabled = true;
            this.Comb_Distance.Location = new System.Drawing.Point(53, 34);
            this.Comb_Distance.Margin = new System.Windows.Forms.Padding(4);
            this.Comb_Distance.Name = "Comb_Distance";
            this.Comb_Distance.Size = new System.Drawing.Size(124, 24);
            this.Comb_Distance.TabIndex = 2;
            this.Comb_Distance.SelectedIndexChanged += new System.EventHandler(this.Comb_Distance_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "位移";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "轴号";
            // 
            // Btn_Home
            // 
            this.Btn_Home.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Home.Image")));
            this.Btn_Home.Location = new System.Drawing.Point(359, 84);
            this.Btn_Home.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Home.Name = "Btn_Home";
            this.Btn_Home.Size = new System.Drawing.Size(55, 50);
            this.Btn_Home.TabIndex = 8;
            this.Btn_Home.Text = "回零";
            this.Btn_Home.UseVisualStyleBackColor = true;
            this.Btn_Home.Click += new System.EventHandler(this.Btn_Home_Click);
            // 
            // Btn_Forward
            // 
            this.Btn_Forward.Image = global::ZeroBase.Properties.Resources._add;
            this.Btn_Forward.Location = new System.Drawing.Point(290, 5);
            this.Btn_Forward.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Forward.Name = "Btn_Forward";
            this.Btn_Forward.Size = new System.Drawing.Size(55, 46);
            this.Btn_Forward.TabIndex = 4;
            this.Btn_Forward.Text = "+";
            this.Btn_Forward.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.Btn_Forward.UseVisualStyleBackColor = true;
            this.Btn_Forward.Click += new System.EventHandler(this.Btn_Forward_Click);
            // 
            // Btn_Back
            // 
            this.Btn_Back.Image = global::ZeroBase.Properties.Resources._minus;
            this.Btn_Back.Location = new System.Drawing.Point(224, 5);
            this.Btn_Back.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Back.Name = "Btn_Back";
            this.Btn_Back.Size = new System.Drawing.Size(55, 46);
            this.Btn_Back.TabIndex = 3;
            this.Btn_Back.Text = "-";
            this.Btn_Back.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.Btn_Back.UseVisualStyleBackColor = true;
            this.Btn_Back.Click += new System.EventHandler(this.Btn_Back_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.LB_Home,
            this.LB_AxisNo,
            this.LB_Pos});
            this.statusStrip1.Location = new System.Drawing.Point(0, 287);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(471, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // LB_Home
            // 
            this.LB_Home.Name = "LB_Home";
            this.LB_Home.Size = new System.Drawing.Size(56, 17);
            this.LB_Home.Text = "未初始化";
            // 
            // LB_AxisNo
            // 
            this.LB_AxisNo.Name = "LB_AxisNo";
            this.LB_AxisNo.Size = new System.Drawing.Size(27, 17);
            this.LB_AxisNo.Text = "0：";
            // 
            // LB_Pos
            // 
            this.LB_Pos.Name = "LB_Pos";
            this.LB_Pos.Size = new System.Drawing.Size(39, 17);
            this.LB_Pos.Text = "0.000";
            // 
            // PB_MEL
            // 
            this.PB_MEL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PB_MEL.Location = new System.Drawing.Point(59, 122);
            this.PB_MEL.Name = "PB_MEL";
            this.PB_MEL.Size = new System.Drawing.Size(30, 30);
            this.PB_MEL.TabIndex = 10;
            this.PB_MEL.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "MEL";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(105, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "ORG";
            // 
            // PB_ORG
            // 
            this.PB_ORG.Location = new System.Drawing.Point(103, 122);
            this.PB_ORG.Name = "PB_ORG";
            this.PB_ORG.Size = new System.Drawing.Size(30, 30);
            this.PB_ORG.TabIndex = 12;
            this.PB_ORG.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(148, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 16);
            this.label6.TabIndex = 15;
            this.label6.Text = "PEL";
            // 
            // PB_PEL
            // 
            this.PB_PEL.Location = new System.Drawing.Point(147, 122);
            this.PB_PEL.Name = "PB_PEL";
            this.PB_PEL.Size = new System.Drawing.Size(30, 30);
            this.PB_PEL.TabIndex = 14;
            this.PB_PEL.TabStop = false;
            // 
            // PB_ALM
            // 
            this.PB_ALM.Location = new System.Drawing.Point(191, 122);
            this.PB_ALM.Name = "PB_ALM";
            this.PB_ALM.Size = new System.Drawing.Size(30, 30);
            this.PB_ALM.TabIndex = 16;
            this.PB_ALM.TabStop = false;
            // 
            // PB_ASTP
            // 
            this.PB_ASTP.Location = new System.Drawing.Point(235, 122);
            this.PB_ASTP.Name = "PB_ASTP";
            this.PB_ASTP.Size = new System.Drawing.Size(30, 30);
            this.PB_ASTP.TabIndex = 17;
            this.PB_ASTP.TabStop = false;
            // 
            // PB_SVON
            // 
            this.PB_SVON.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PB_SVON.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PB_SVON.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_SVON.Location = new System.Drawing.Point(15, 122);
            this.PB_SVON.Name = "PB_SVON";
            this.PB_SVON.Size = new System.Drawing.Size(30, 30);
            this.PB_SVON.TabIndex = 18;
            this.PB_SVON.TabStop = false;
            this.PB_SVON.Click += new System.EventHandler(this.PB_SVON_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(191, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "ALM";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(234, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 16);
            this.label8.TabIndex = 20;
            this.label8.Text = "ASTP";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "SVON";
            // 
            // Comb_AxisNo
            // 
            this.Comb_AxisNo.FormattingEnabled = true;
            this.Comb_AxisNo.Location = new System.Drawing.Point(53, 4);
            this.Comb_AxisNo.Margin = new System.Windows.Forms.Padding(4);
            this.Comb_AxisNo.Name = "Comb_AxisNo";
            this.Comb_AxisNo.Size = new System.Drawing.Size(124, 24);
            this.Comb_AxisNo.TabIndex = 22;
            this.Comb_AxisNo.SelectedIndexChanged += new System.EventHandler(this.Comb_AxisNo_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.PB_SVON);
            this.panel1.Controls.Add(this.PB_ASTP);
            this.panel1.Controls.Add(this.PB_ALM);
            this.panel1.Controls.Add(this.PB_MEL);
            this.panel1.Controls.Add(this.PB_PEL);
            this.panel1.Controls.Add(this.PB_ORG);
            this.panel1.Controls.Add(this.bt_JOG_N);
            this.panel1.Controls.Add(this.bt_JOG_P);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.textBox_Acc);
            this.panel1.Controls.Add(this.Btn_Stop);
            this.panel1.Controls.Add(this.Btn_Home);
            this.panel1.Controls.Add(this.Comb_AxisNo);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.Comb_Distance);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.Btn_Back);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.Btn_Forward);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.Bar_Vel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 287);
            this.panel1.TabIndex = 23;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(349, 194);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 70);
            this.button2.TabIndex = 40;
            this.button2.Text = "清报警";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(349, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 50);
            this.button1.TabIndex = 40;
            this.button1.Text = "清零";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(142, 171);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 12);
            this.label16.TabIndex = 39;
            this.label16.Text = "正极限";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(193, 171);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 12);
            this.label15.TabIndex = 38;
            this.label15.Text = "报警";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(232, 171);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 37;
            this.label14.Text = "异常停止";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(103, 171);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 36;
            this.label13.Text = "原点";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(52, 171);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 35;
            this.label12.Text = "负极限";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(13, 171);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 34;
            this.label11.Text = "使能";
            // 
            // bt_JOG_N
            // 
            this.bt_JOG_N.Image = global::ZeroBase.Properties.Resources._minus;
            this.bt_JOG_N.Location = new System.Drawing.Point(221, 66);
            this.bt_JOG_N.Margin = new System.Windows.Forms.Padding(4);
            this.bt_JOG_N.Name = "bt_JOG_N";
            this.bt_JOG_N.Size = new System.Drawing.Size(55, 46);
            this.bt_JOG_N.TabIndex = 28;
            this.bt_JOG_N.Text = "JOG";
            this.bt_JOG_N.UseVisualStyleBackColor = true;
            this.bt_JOG_N.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bt_JOG_N_MouseDown);
            this.bt_JOG_N.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bt_JOG_N_MouseUp);
            // 
            // bt_JOG_P
            // 
            this.bt_JOG_P.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bt_JOG_P.Image = global::ZeroBase.Properties.Resources._add;
            this.bt_JOG_P.Location = new System.Drawing.Point(291, 67);
            this.bt_JOG_P.Margin = new System.Windows.Forms.Padding(4);
            this.bt_JOG_P.Name = "bt_JOG_P";
            this.bt_JOG_P.Size = new System.Drawing.Size(55, 46);
            this.bt_JOG_P.TabIndex = 27;
            this.bt_JOG_P.Text = "JOG";
            this.bt_JOG_P.UseVisualStyleBackColor = false;
            this.bt_JOG_P.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bt_JOG_P_MouseDown);
            this.bt_JOG_P.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bt_JOG_P_MouseUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(287, 163);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 16);
            this.label10.TabIndex = 26;
            this.label10.Text = "mm/s^2";
            // 
            // textBox_Acc
            // 
            this.textBox_Acc.Location = new System.Drawing.Point(287, 126);
            this.textBox_Acc.Name = "textBox_Acc";
            this.textBox_Acc.Size = new System.Drawing.Size(53, 26);
            this.textBox_Acc.TabIndex = 25;
            this.textBox_Acc.TextChanged += new System.EventHandler(this.textBox_Acc_TextChanged);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.Image = global::ZeroBase.Properties.Resources._wait;
            this.Btn_Stop.Location = new System.Drawing.Point(359, 21);
            this.Btn_Stop.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(55, 46);
            this.Btn_Stop.TabIndex = 24;
            this.Btn_Stop.Text = "停止";
            this.Btn_Stop.UseVisualStyleBackColor = true;
            this.Btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // XAxisControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "XAxisControlPanel";
            this.Size = new System.Drawing.Size(471, 309);
            ((System.ComponentModel.ISupportInitialize)(this.Bar_Vel)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MEL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ORG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_PEL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ALM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ASTP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_SVON)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar Bar_Vel;
        private System.Windows.Forms.ComboBox Comb_Distance;
        private System.Windows.Forms.Button Btn_Back;
        private System.Windows.Forms.Button Btn_Forward;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Btn_Home;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LB_Home;
        private System.Windows.Forms.ToolStripStatusLabel LB_AxisNo;
        private System.Windows.Forms.ToolStripStatusLabel LB_Pos;
        private System.Windows.Forms.PictureBox PB_MEL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox PB_ORG;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox PB_PEL;
        private System.Windows.Forms.PictureBox PB_ALM;
        private System.Windows.Forms.PictureBox PB_ASTP;
        private System.Windows.Forms.PictureBox PB_SVON;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox Comb_AxisNo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Btn_Stop;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_Acc;
        private System.Windows.Forms.Button bt_JOG_N;
        private System.Windows.Forms.Button bt_JOG_P;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
