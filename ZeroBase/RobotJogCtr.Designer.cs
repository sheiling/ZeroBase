using XCore.Properties;

namespace XCore
{
    partial class RobotJogCtr
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
            this.components = new System.ComponentModel.Container();
            this.Xadd = new System.Windows.Forms.Button();
            this.Xminus = new System.Windows.Forms.Button();
            this.Yminus = new System.Windows.Forms.Button();
            this.Yadd = new System.Windows.Forms.Button();
            this.Zminus = new System.Windows.Forms.Button();
            this.Zadd = new System.Windows.Forms.Button();
            this.Aadd = new System.Windows.Forms.Button();
            this.Aminus = new System.Windows.Forms.Button();
            this.btnGetPos = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtA = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtC = new System.Windows.Forms.TextBox();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbVel = new System.Windows.Forms.TrackBar();
            this.label1Speed = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.numberBox1 = new XCore.NumberBox(this.components);
            this.radioShort = new System.Windows.Forms.RadioButton();
            this.radioMediu = new System.Windows.Forms.RadioButton();
            this.radioLong = new System.Windows.Forms.RadioButton();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.Badd = new System.Windows.Forms.Button();
            this.Bminus = new System.Windows.Forms.Button();
            this.Cadd = new System.Windows.Forms.Button();
            this.Cminus = new System.Windows.Forms.Button();
            this.btnServo = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVel)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Xadd
            // 
            this.Xadd.BackgroundImage = global::XCore.Properties.Resources._down;
            this.Xadd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Xadd.Location = new System.Drawing.Point(58, 328);
            this.Xadd.Name = "Xadd";
            this.Xadd.Size = new System.Drawing.Size(45, 55);
            this.Xadd.TabIndex = 1;
            this.Xadd.Text = "+X";
            this.Xadd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Xadd.UseVisualStyleBackColor = true;
            this.Xadd.Click += new System.EventHandler(this.button3_Click);
            // 
            // Xminus
            // 
            this.Xminus.BackgroundImage = global::XCore.Properties.Resources._up;
            this.Xminus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Xminus.Location = new System.Drawing.Point(58, 248);
            this.Xminus.Name = "Xminus";
            this.Xminus.Size = new System.Drawing.Size(45, 55);
            this.Xminus.TabIndex = 1;
            this.Xminus.Text = "-X";
            this.Xminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Xminus.UseVisualStyleBackColor = true;
            this.Xminus.Click += new System.EventHandler(this.button3_Click);
            // 
            // Yminus
            // 
            this.Yminus.BackgroundImage = global::XCore.Properties.Resources._left;
            this.Yminus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Yminus.Location = new System.Drawing.Point(8, 289);
            this.Yminus.Name = "Yminus";
            this.Yminus.Size = new System.Drawing.Size(45, 55);
            this.Yminus.TabIndex = 2;
            this.Yminus.Text = "-Y";
            this.Yminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Yminus.UseVisualStyleBackColor = true;
            this.Yminus.Click += new System.EventHandler(this.button3_Click);
            // 
            // Yadd
            // 
            this.Yadd.BackgroundImage = global::XCore.Properties.Resources._right;
            this.Yadd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Yadd.Location = new System.Drawing.Point(107, 289);
            this.Yadd.Name = "Yadd";
            this.Yadd.Size = new System.Drawing.Size(45, 55);
            this.Yadd.TabIndex = 3;
            this.Yadd.Text = "+Y";
            this.Yadd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Yadd.UseVisualStyleBackColor = true;
            this.Yadd.Click += new System.EventHandler(this.button3_Click);
            // 
            // Zminus
            // 
            this.Zminus.BackgroundImage = global::XCore.Properties.Resources._down;
            this.Zminus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Zminus.Location = new System.Drawing.Point(159, 328);
            this.Zminus.Name = "Zminus";
            this.Zminus.Size = new System.Drawing.Size(45, 55);
            this.Zminus.TabIndex = 5;
            this.Zminus.Text = "-Z";
            this.Zminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Zminus.UseVisualStyleBackColor = true;
            this.Zminus.Click += new System.EventHandler(this.button3_Click);
            // 
            // Zadd
            // 
            this.Zadd.BackgroundImage = global::XCore.Properties.Resources._up;
            this.Zadd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Zadd.Location = new System.Drawing.Point(159, 248);
            this.Zadd.Name = "Zadd";
            this.Zadd.Size = new System.Drawing.Size(45, 55);
            this.Zadd.TabIndex = 4;
            this.Zadd.Text = "+Z";
            this.Zadd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Zadd.UseVisualStyleBackColor = true;
            this.Zadd.Click += new System.EventHandler(this.button3_Click);
            // 
            // Aadd
            // 
            this.Aadd.BackgroundImage = global::XCore.Properties.Resources._rotate_antiClock;
            this.Aadd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Aadd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Aadd.Location = new System.Drawing.Point(214, 328);
            this.Aadd.Name = "Aadd";
            this.Aadd.Size = new System.Drawing.Size(45, 55);
            this.Aadd.TabIndex = 7;
            this.Aadd.Text = "+A";
            this.Aadd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Aadd.UseVisualStyleBackColor = true;
            this.Aadd.Click += new System.EventHandler(this.button3_Click);
            // 
            // Aminus
            // 
            this.Aminus.BackgroundImage = global::XCore.Properties.Resources._rotate_clock;
            this.Aminus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Aminus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Aminus.Location = new System.Drawing.Point(214, 248);
            this.Aminus.Name = "Aminus";
            this.Aminus.Size = new System.Drawing.Size(45, 55);
            this.Aminus.TabIndex = 6;
            this.Aminus.Text = "-A";
            this.Aminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Aminus.UseVisualStyleBackColor = true;
            this.Aminus.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnGetPos
            // 
            this.btnGetPos.Location = new System.Drawing.Point(285, 43);
            this.btnGetPos.Name = "btnGetPos";
            this.btnGetPos.Size = new System.Drawing.Size(65, 28);
            this.btnGetPos.TabIndex = 5;
            this.btnGetPos.Text = "刷新坐标";
            this.btnGetPos.UseVisualStyleBackColor = true;
            this.btnGetPos.Click += new System.EventHandler(this.btnGetPos_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtB);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtA);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtX);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtY);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtC);
            this.groupBox1.Controls.Add(this.btnGetPos);
            this.groupBox1.Controls.Add(this.txtZ);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(5, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 107);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "坐标:";
            // 
            // txtB
            // 
            this.txtB.Location = new System.Drawing.Point(206, 47);
            this.txtB.Name = "txtB";
            this.txtB.Size = new System.Drawing.Size(56, 21);
            this.txtB.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(158, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "B(deg)";
            // 
            // txtA
            // 
            this.txtA.Location = new System.Drawing.Point(206, 17);
            this.txtA.Name = "txtA";
            this.txtA.Size = new System.Drawing.Size(56, 21);
            this.txtA.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(158, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "A(deg)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "X(mm)";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(83, 16);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(56, 21);
            this.txtX.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Y(mm)";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(83, 46);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(56, 21);
            this.txtY.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "Z(mm)";
            // 
            // txtC
            // 
            this.txtC.Location = new System.Drawing.Point(206, 77);
            this.txtC.Name = "txtC";
            this.txtC.Size = new System.Drawing.Size(56, 21);
            this.txtC.TabIndex = 13;
            // 
            // txtZ
            // 
            this.txtZ.Location = new System.Drawing.Point(83, 76);
            this.txtZ.Name = "txtZ";
            this.txtZ.Size = new System.Drawing.Size(56, 21);
            this.txtZ.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(158, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "C(deg)";
            // 
            // tbVel
            // 
            this.tbVel.AutoSize = false;
            this.tbVel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbVel.LargeChange = 10;
            this.tbVel.Location = new System.Drawing.Point(61, 398);
            this.tbVel.Maximum = 20;
            this.tbVel.Minimum = 1;
            this.tbVel.Name = "tbVel";
            this.tbVel.Size = new System.Drawing.Size(193, 30);
            this.tbVel.TabIndex = 22;
            this.tbVel.Value = 5;
            this.tbVel.Scroll += new System.EventHandler(this.tbVel_Scroll);
            // 
            // label1Speed
            // 
            this.label1Speed.AutoSize = true;
            this.label1Speed.Location = new System.Drawing.Point(266, 403);
            this.label1Speed.Name = "label1Speed";
            this.label1Speed.Size = new System.Drawing.Size(53, 12);
            this.label1Speed.TabIndex = 20;
            this.label1Speed.Text = "5%(1-20)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(24, 403);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(41, 12);
            this.label17.TabIndex = 21;
            this.label17.Text = "速度：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton6);
            this.groupBox3.Controls.Add(this.radioButton4);
            this.groupBox3.Controls.Add(this.radioButton5);
            this.groupBox3.Controls.Add(this.radioButton1);
            this.groupBox3.Controls.Add(this.radioButton2);
            this.groupBox3.Controls.Add(this.radioButton3);
            this.groupBox3.Controls.Add(this.numberBox1);
            this.groupBox3.Controls.Add(this.radioShort);
            this.groupBox3.Controls.Add(this.radioMediu);
            this.groupBox3.Controls.Add(this.radioLong);
            this.groupBox3.Location = new System.Drawing.Point(17, 437);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 110);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "步进距离";
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(197, 28);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(53, 16);
            this.radioButton6.TabIndex = 28;
            this.radioButton6.Text = "0.5mm";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(197, 80);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(41, 16);
            this.radioButton4.TabIndex = 27;
            this.radioButton4.Text = "2mm";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(261, 26);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(41, 16);
            this.radioButton5.TabIndex = 26;
            this.radioButton5.Text = "5mm";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(120, 80);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(53, 16);
            this.radioButton1.TabIndex = 25;
            this.radioButton1.Text = "0.1mm";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(120, 54);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(59, 16);
            this.radioButton2.TabIndex = 24;
            this.radioButton2.Text = "0.05mm";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(120, 26);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(59, 16);
            this.radioButton3.TabIndex = 23;
            this.radioButton3.Text = "0.01mm";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // numberBox1
            // 
            this.numberBox1.DecimalDigits = ((uint)(3u));
            this.numberBox1.Location = new System.Drawing.Point(20, 38);
            this.numberBox1.MaxValue = ((long)(9223372036854775807));
            this.numberBox1.MinValue = ((long)(-9223372036854775808));
            this.numberBox1.Name = "numberBox1";
            this.numberBox1.Size = new System.Drawing.Size(82, 21);
            this.numberBox1.TabIndex = 22;
            this.numberBox1.Text = "1";
            // 
            // radioShort
            // 
            this.radioShort.AutoSize = true;
            this.radioShort.Checked = true;
            this.radioShort.Location = new System.Drawing.Point(197, 54);
            this.radioShort.Name = "radioShort";
            this.radioShort.Size = new System.Drawing.Size(41, 16);
            this.radioShort.TabIndex = 21;
            this.radioShort.TabStop = true;
            this.radioShort.Text = "1mm";
            this.radioShort.UseVisualStyleBackColor = true;
            this.radioShort.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // radioMediu
            // 
            this.radioMediu.AutoSize = true;
            this.radioMediu.Location = new System.Drawing.Point(261, 54);
            this.radioMediu.Name = "radioMediu";
            this.radioMediu.Size = new System.Drawing.Size(47, 16);
            this.radioMediu.TabIndex = 20;
            this.radioMediu.Text = "10mm";
            this.radioMediu.UseVisualStyleBackColor = true;
            this.radioMediu.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // radioLong
            // 
            this.radioLong.AutoSize = true;
            this.radioLong.Location = new System.Drawing.Point(261, 80);
            this.radioLong.Name = "radioLong";
            this.radioLong.Size = new System.Drawing.Size(47, 16);
            this.radioLong.TabIndex = 19;
            this.radioLong.Text = "50mm";
            this.radioLong.UseVisualStyleBackColor = true;
            this.radioLong.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // btnReset
            // 
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReset.Location = new System.Drawing.Point(102, 182);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(69, 36);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "复位";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnStop
            // 
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStop.Location = new System.Drawing.Point(23, 182);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(69, 36);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Location = new System.Drawing.Point(272, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 28);
            this.label4.TabIndex = 25;
            // 
            // btnStart
            // 
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Location = new System.Drawing.Point(260, 182);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(69, 36);
            this.btnStart.TabIndex = 26;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox1.Location = new System.Drawing.Point(28, 141);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(123, 20);
            this.checkBox1.TabIndex = 28;
            this.checkBox1.Text = "机械手操作权";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // Badd
            // 
            this.Badd.BackgroundImage = global::XCore.Properties.Resources._rotate_antiClock;
            this.Badd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Badd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Badd.Location = new System.Drawing.Point(269, 328);
            this.Badd.Name = "Badd";
            this.Badd.Size = new System.Drawing.Size(45, 55);
            this.Badd.TabIndex = 30;
            this.Badd.Text = "+B";
            this.Badd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Badd.UseVisualStyleBackColor = true;
            // 
            // Bminus
            // 
            this.Bminus.BackgroundImage = global::XCore.Properties.Resources._rotate_clock;
            this.Bminus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Bminus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Bminus.Location = new System.Drawing.Point(269, 248);
            this.Bminus.Name = "Bminus";
            this.Bminus.Size = new System.Drawing.Size(45, 55);
            this.Bminus.TabIndex = 29;
            this.Bminus.Text = "-B";
            this.Bminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Bminus.UseVisualStyleBackColor = true;
            this.Bminus.Click += new System.EventHandler(this.Bminus_Click);
            // 
            // Cadd
            // 
            this.Cadd.BackgroundImage = global::XCore.Properties.Resources._rotate_antiClock;
            this.Cadd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Cadd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Cadd.Location = new System.Drawing.Point(324, 328);
            this.Cadd.Name = "Cadd";
            this.Cadd.Size = new System.Drawing.Size(45, 55);
            this.Cadd.TabIndex = 32;
            this.Cadd.Text = "+C";
            this.Cadd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Cadd.UseVisualStyleBackColor = true;
            // 
            // Cminus
            // 
            this.Cminus.BackgroundImage = global::XCore.Properties.Resources._rotate_clock;
            this.Cminus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Cminus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Cminus.Location = new System.Drawing.Point(324, 248);
            this.Cminus.Name = "Cminus";
            this.Cminus.Size = new System.Drawing.Size(45, 55);
            this.Cminus.TabIndex = 31;
            this.Cminus.Text = "-C";
            this.Cminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Cminus.UseVisualStyleBackColor = true;
            this.Cminus.Click += new System.EventHandler(this.Cminus_Click);
            // 
            // btnServo
            // 
            this.btnServo.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnServo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnServo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnServo.Location = new System.Drawing.Point(181, 181);
            this.btnServo.Name = "btnServo";
            this.btnServo.Size = new System.Drawing.Size(69, 36);
            this.btnServo.TabIndex = 33;
            this.btnServo.Text = "伺服ON";
            this.btnServo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnServo.UseVisualStyleBackColor = true;
            this.btnServo.CheckedChanged += new System.EventHandler(this.btnServo_CheckedChanged);
            // 
            // RobotJogCtr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnServo);
            this.Controls.Add(this.Cadd);
            this.Controls.Add(this.Cminus);
            this.Controls.Add(this.Badd);
            this.Controls.Add(this.Bminus);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Aadd);
            this.Controls.Add(this.Aminus);
            this.Controls.Add(this.Zminus);
            this.Controls.Add(this.Zadd);
            this.Controls.Add(this.Yadd);
            this.Controls.Add(this.Yminus);
            this.Controls.Add(this.Xminus);
            this.Controls.Add(this.Xadd);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.tbVel);
            this.Controls.Add(this.label1Speed);
            this.Controls.Add(this.label17);
            this.Name = "RobotJogCtr";
            this.Size = new System.Drawing.Size(379, 554);
            this.Load += new System.EventHandler(this.RobotCtr_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVel)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Xadd;
        private System.Windows.Forms.Button Xminus;
        private System.Windows.Forms.Button Yminus;
        private System.Windows.Forms.Button Yadd;
        private System.Windows.Forms.Button Zminus;
        private System.Windows.Forms.Button Zadd;
        private System.Windows.Forms.Button Aadd;
        private System.Windows.Forms.Button Aminus;
        private System.Windows.Forms.Button btnGetPos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtC;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar tbVel;
        private System.Windows.Forms.Label label1Speed;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox3;
        private NumberBox numberBox1;
        private System.Windows.Forms.RadioButton radioShort;
        private System.Windows.Forms.RadioButton radioMediu;
        private System.Windows.Forms.RadioButton radioLong;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.TextBox txtB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Badd;
        private System.Windows.Forms.Button Bminus;
        private System.Windows.Forms.Button Cadd;
        private System.Windows.Forms.Button Cminus;
        private System.Windows.Forms.CheckBox btnServo;
    }
}
