namespace TheChessBoard
{
    partial class TheChessBoard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TheChessBoard));
            this.btnMove = new System.Windows.Forms.Button();
            this.txbMoveStr = new System.Windows.Forms.TextBox();
            this.rdbWhite = new System.Windows.Forms.RadioButton();
            this.rdbBlack = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txbFen = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ckbDontCareWhoseTurnItIs = new System.Windows.Forms.CheckBox();
            this.lblCurrentPlayer = new System.Windows.Forms.Label();
            this.lblWhiteWatch = new System.Windows.Forms.Label();
            this.btnWhiteReadMove = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblBlackWatch = new System.Windows.Forms.Label();
            this.lblBlackStatusText = new System.Windows.Forms.Label();
            this.lblBlackStatusCaption = new System.Windows.Forms.Label();
            this.lblBlackStatus = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblWhiteStatusText = new System.Windows.Forms.Label();
            this.lblWhiteStatusCaption = new System.Windows.Forms.Label();
            this.lblWhiteStatus = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblFormStatusText = new System.Windows.Forms.Label();
            this.lblFormStatusCaption = new System.Windows.Forms.Label();
            this.lblFormStatus = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnPauseCont = new System.Windows.Forms.Button();
            this.btnAllReset = new System.Windows.Forms.Button();
            this.btnModeConfirm = new System.Windows.Forms.Button();
            this.pnlBlackMode = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbBlackAuto = new System.Windows.Forms.RadioButton();
            this.rdbBlackManual = new System.Windows.Forms.RadioButton();
            this.pnlWhiteMode = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.rdbWhiteAuto = new System.Windows.Forms.RadioButton();
            this.rdbWhiteManual = new System.Windows.Forms.RadioButton();
            this.btnLoadWhiteAI = new System.Windows.Forms.Button();
            this.btnLoadBlackAI = new System.Windows.Forms.Button();
            this.btnBlackReadMove = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.cmsAbout = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.smiQuote = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.pnlBlackMode.SuspendLayout();
            this.pnlWhiteMode.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.cmsAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(449, 31);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(91, 59);
            this.btnMove.TabIndex = 15;
            this.btnMove.Text = "走一步(Enter)";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // txbMoveStr
            // 
            this.txbMoveStr.Location = new System.Drawing.Point(80, 23);
            this.txbMoveStr.Name = "txbMoveStr";
            this.txbMoveStr.Size = new System.Drawing.Size(98, 27);
            this.txbMoveStr.TabIndex = 0;
            // 
            // rdbWhite
            // 
            this.rdbWhite.Checked = true;
            this.rdbWhite.Location = new System.Drawing.Point(20, 31);
            this.rdbWhite.Name = "rdbWhite";
            this.rdbWhite.Size = new System.Drawing.Size(95, 30);
            this.rdbWhite.TabIndex = 5;
            this.rdbWhite.TabStop = true;
            this.rdbWhite.Text = "白方(&W)";
            this.rdbWhite.UseVisualStyleBackColor = true;
            this.rdbWhite.CheckedChanged += new System.EventHandler(this.SANPlayerChanged);
            // 
            // rdbBlack
            // 
            this.rdbBlack.Location = new System.Drawing.Point(121, 31);
            this.rdbBlack.Name = "rdbBlack";
            this.rdbBlack.Size = new System.Drawing.Size(95, 30);
            this.rdbBlack.TabIndex = 6;
            this.rdbBlack.Text = "黑方(&B)";
            this.rdbBlack.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbWhite);
            this.groupBox1.Controls.Add(this.rdbBlack);
            this.groupBox1.Location = new System.Drawing.Point(218, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 71);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "执白执黑";
            // 
            // pnlBoard
            // 
            this.pnlBoard.Location = new System.Drawing.Point(8, 28);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(552, 536);
            this.pnlBoard.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txbMoveStr);
            this.panel1.Location = new System.Drawing.Point(17, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 59);
            this.panel1.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "输入S&AN";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox6);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Controls.Add(this.ckbDontCareWhoseTurnItIs);
            this.panel2.Controls.Add(this.lblCurrentPlayer);
            this.panel2.Controls.Add(this.pnlBoard);
            this.panel2.Location = new System.Drawing.Point(342, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(567, 818);
            this.panel2.TabIndex = 17;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txbFen);
            this.groupBox6.Location = new System.Drawing.Point(8, 744);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(552, 68);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "当前 FEN";
            // 
            // txbFen
            // 
            this.txbFen.Location = new System.Drawing.Point(17, 27);
            this.txbFen.Name = "txbFen";
            this.txbFen.ReadOnly = true;
            this.txbFen.Size = new System.Drawing.Size(525, 27);
            this.txbFen.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.panel1);
            this.groupBox5.Controls.Add(this.groupBox1);
            this.groupBox5.Controls.Add(this.btnMove);
            this.groupBox5.Location = new System.Drawing.Point(8, 636);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(552, 105);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "SAN 测试";
            // 
            // ckbDontCareWhoseTurnItIs
            // 
            this.ckbDontCareWhoseTurnItIs.AutoSize = true;
            this.ckbDontCareWhoseTurnItIs.Location = new System.Drawing.Point(419, 595);
            this.ckbDontCareWhoseTurnItIs.Name = "ckbDontCareWhoseTurnItIs";
            this.ckbDontCareWhoseTurnItIs.Size = new System.Drawing.Size(135, 24);
            this.ckbDontCareWhoseTurnItIs.TabIndex = 18;
            this.ckbDontCareWhoseTurnItIs.Text = "忽略当前执棋(&I)";
            this.ckbDontCareWhoseTurnItIs.UseVisualStyleBackColor = true;
            this.ckbDontCareWhoseTurnItIs.CheckedChanged += new System.EventHandler(this.ckbDontCareWhoseTurnItIs_CheckedChanged);
            // 
            // lblCurrentPlayer
            // 
            this.lblCurrentPlayer.Location = new System.Drawing.Point(222, 596);
            this.lblCurrentPlayer.Name = "lblCurrentPlayer";
            this.lblCurrentPlayer.Size = new System.Drawing.Size(107, 23);
            this.lblCurrentPlayer.TabIndex = 17;
            this.lblCurrentPlayer.Text = "当前执棋：";
            // 
            // lblWhiteWatch
            // 
            this.lblWhiteWatch.AutoSize = true;
            this.lblWhiteWatch.Location = new System.Drawing.Point(167, 5);
            this.lblWhiteWatch.Name = "lblWhiteWatch";
            this.lblWhiteWatch.Size = new System.Drawing.Size(53, 20);
            this.lblWhiteWatch.TabIndex = 18;
            this.lblWhiteWatch.Text = "--:--.--";
            // 
            // btnWhiteReadMove
            // 
            this.btnWhiteReadMove.Location = new System.Drawing.Point(35, 68);
            this.btnWhiteReadMove.Name = "btnWhiteReadMove";
            this.btnWhiteReadMove.Size = new System.Drawing.Size(134, 38);
            this.btnWhiteReadMove.TabIndex = 19;
            this.btnWhiteReadMove.Text = "白方读入一步(&Y)";
            this.btnWhiteReadMove.UseVisualStyleBackColor = true;
            this.btnWhiteReadMove.Click += new System.EventHandler(this.btnWhiteReadMove_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Location = new System.Drawing.Point(23, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 132);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状态";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lblBlackWatch);
            this.panel5.Controls.Add(this.lblBlackStatusText);
            this.panel5.Controls.Add(this.lblBlackStatusCaption);
            this.panel5.Controls.Add(this.lblBlackStatus);
            this.panel5.Location = new System.Drawing.Point(12, 94);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(272, 31);
            this.panel5.TabIndex = 25;
            // 
            // lblBlackWatch
            // 
            this.lblBlackWatch.AutoSize = true;
            this.lblBlackWatch.Location = new System.Drawing.Point(167, 4);
            this.lblBlackWatch.Name = "lblBlackWatch";
            this.lblBlackWatch.Size = new System.Drawing.Size(53, 20);
            this.lblBlackWatch.TabIndex = 24;
            this.lblBlackWatch.Text = "--:--.--";
            // 
            // lblBlackStatusText
            // 
            this.lblBlackStatusText.AutoSize = true;
            this.lblBlackStatusText.Location = new System.Drawing.Point(75, 4);
            this.lblBlackStatusText.Name = "lblBlackStatusText";
            this.lblBlackStatusText.Size = new System.Drawing.Size(54, 20);
            this.lblBlackStatusText.TabIndex = 23;
            this.lblBlackStatusText.Text = "无状态";
            // 
            // lblBlackStatusCaption
            // 
            this.lblBlackStatusCaption.AutoSize = true;
            this.lblBlackStatusCaption.Location = new System.Drawing.Point(21, 4);
            this.lblBlackStatusCaption.Name = "lblBlackStatusCaption";
            this.lblBlackStatusCaption.Size = new System.Drawing.Size(39, 20);
            this.lblBlackStatusCaption.TabIndex = 22;
            this.lblBlackStatusCaption.Text = "黑AI";
            // 
            // lblBlackStatus
            // 
            this.lblBlackStatus.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBlackStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblBlackStatus.Location = new System.Drawing.Point(0, 1);
            this.lblBlackStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlackStatus.Name = "lblBlackStatus";
            this.lblBlackStatus.Size = new System.Drawing.Size(23, 30);
            this.lblBlackStatus.TabIndex = 21;
            this.lblBlackStatus.Text = "●";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblWhiteStatusText);
            this.panel4.Controls.Add(this.lblWhiteStatusCaption);
            this.panel4.Controls.Add(this.lblWhiteWatch);
            this.panel4.Controls.Add(this.lblWhiteStatus);
            this.panel4.Location = new System.Drawing.Point(12, 60);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(272, 31);
            this.panel4.TabIndex = 24;
            // 
            // lblWhiteStatusText
            // 
            this.lblWhiteStatusText.AutoSize = true;
            this.lblWhiteStatusText.Location = new System.Drawing.Point(75, 5);
            this.lblWhiteStatusText.Name = "lblWhiteStatusText";
            this.lblWhiteStatusText.Size = new System.Drawing.Size(54, 20);
            this.lblWhiteStatusText.TabIndex = 23;
            this.lblWhiteStatusText.Text = "无状态";
            this.lblWhiteStatusText.Click += new System.EventHandler(this.label2_Click);
            // 
            // lblWhiteStatusCaption
            // 
            this.lblWhiteStatusCaption.AutoSize = true;
            this.lblWhiteStatusCaption.Location = new System.Drawing.Point(21, 4);
            this.lblWhiteStatusCaption.Name = "lblWhiteStatusCaption";
            this.lblWhiteStatusCaption.Size = new System.Drawing.Size(39, 20);
            this.lblWhiteStatusCaption.TabIndex = 22;
            this.lblWhiteStatusCaption.Text = "白AI";
            // 
            // lblWhiteStatus
            // 
            this.lblWhiteStatus.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWhiteStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblWhiteStatus.Location = new System.Drawing.Point(0, 1);
            this.lblWhiteStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblWhiteStatus.Name = "lblWhiteStatus";
            this.lblWhiteStatus.Size = new System.Drawing.Size(23, 30);
            this.lblWhiteStatus.TabIndex = 21;
            this.lblWhiteStatus.Text = "●";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblFormStatusText);
            this.panel3.Controls.Add(this.lblFormStatusCaption);
            this.panel3.Controls.Add(this.lblFormStatus);
            this.panel3.Location = new System.Drawing.Point(12, 26);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 31);
            this.panel3.TabIndex = 22;
            // 
            // lblFormStatusText
            // 
            this.lblFormStatusText.AutoSize = true;
            this.lblFormStatusText.Location = new System.Drawing.Point(75, 4);
            this.lblFormStatusText.Name = "lblFormStatusText";
            this.lblFormStatusText.Size = new System.Drawing.Size(54, 20);
            this.lblFormStatusText.TabIndex = 23;
            this.lblFormStatusText.Text = "无状态";
            // 
            // lblFormStatusCaption
            // 
            this.lblFormStatusCaption.AutoSize = true;
            this.lblFormStatusCaption.Location = new System.Drawing.Point(21, 4);
            this.lblFormStatusCaption.Name = "lblFormStatusCaption";
            this.lblFormStatusCaption.Size = new System.Drawing.Size(39, 20);
            this.lblFormStatusCaption.TabIndex = 22;
            this.lblFormStatusCaption.Text = "局面";
            this.lblFormStatusCaption.Click += new System.EventHandler(this.lblFormStatusCaption_Click);
            // 
            // lblFormStatus
            // 
            this.lblFormStatus.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFormStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblFormStatus.Location = new System.Drawing.Point(0, 1);
            this.lblFormStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblFormStatus.Name = "lblFormStatus";
            this.lblFormStatus.Size = new System.Drawing.Size(23, 30);
            this.lblFormStatus.TabIndex = 21;
            this.lblFormStatus.Text = "●";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAbout);
            this.groupBox3.Controls.Add(this.btnPauseCont);
            this.groupBox3.Controls.Add(this.btnAllReset);
            this.groupBox3.Controls.Add(this.btnModeConfirm);
            this.groupBox3.Controls.Add(this.pnlBlackMode);
            this.groupBox3.Controls.Add(this.pnlWhiteMode);
            this.groupBox3.Location = new System.Drawing.Point(23, 261);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 164);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "模式";
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(243, 126);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(38, 30);
            this.btnAbout.TabIndex = 21;
            this.btnAbout.Text = "...";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnPauseCont
            // 
            this.btnPauseCont.Location = new System.Drawing.Point(88, 126);
            this.btnPauseCont.Name = "btnPauseCont";
            this.btnPauseCont.Size = new System.Drawing.Size(70, 30);
            this.btnPauseCont.TabIndex = 6;
            this.btnPauseCont.Text = "停止(&P)";
            this.btnPauseCont.UseVisualStyleBackColor = true;
            this.btnPauseCont.Click += new System.EventHandler(this.btnPauseCont_Click);
            // 
            // btnAllReset
            // 
            this.btnAllReset.Location = new System.Drawing.Point(164, 126);
            this.btnAllReset.Name = "btnAllReset";
            this.btnAllReset.Size = new System.Drawing.Size(70, 30);
            this.btnAllReset.TabIndex = 5;
            this.btnAllReset.Text = "重置(&R)";
            this.btnAllReset.UseVisualStyleBackColor = true;
            this.btnAllReset.Click += new System.EventHandler(this.btnAllReset_Click);
            // 
            // btnModeConfirm
            // 
            this.btnModeConfirm.Location = new System.Drawing.Point(12, 126);
            this.btnModeConfirm.Name = "btnModeConfirm";
            this.btnModeConfirm.Size = new System.Drawing.Size(70, 30);
            this.btnModeConfirm.TabIndex = 4;
            this.btnModeConfirm.Text = "开始(&S)";
            this.btnModeConfirm.UseVisualStyleBackColor = true;
            this.btnModeConfirm.Click += new System.EventHandler(this.btnModeConfirm_Click);
            // 
            // pnlBlackMode
            // 
            this.pnlBlackMode.Controls.Add(this.label4);
            this.pnlBlackMode.Controls.Add(this.rdbBlackAuto);
            this.pnlBlackMode.Controls.Add(this.rdbBlackManual);
            this.pnlBlackMode.Location = new System.Drawing.Point(156, 26);
            this.pnlBlackMode.Name = "pnlBlackMode";
            this.pnlBlackMode.Size = new System.Drawing.Size(125, 90);
            this.pnlBlackMode.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(42, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "黑方";
            // 
            // rdbBlackAuto
            // 
            this.rdbBlackAuto.AutoSize = true;
            this.rdbBlackAuto.Location = new System.Drawing.Point(29, 63);
            this.rdbBlackAuto.Name = "rdbBlackAuto";
            this.rdbBlackAuto.Size = new System.Drawing.Size(60, 24);
            this.rdbBlackAuto.TabIndex = 1;
            this.rdbBlackAuto.Text = "自动";
            this.rdbBlackAuto.UseVisualStyleBackColor = true;
            // 
            // rdbBlackManual
            // 
            this.rdbBlackManual.AutoSize = true;
            this.rdbBlackManual.Checked = true;
            this.rdbBlackManual.Location = new System.Drawing.Point(29, 33);
            this.rdbBlackManual.Name = "rdbBlackManual";
            this.rdbBlackManual.Size = new System.Drawing.Size(60, 24);
            this.rdbBlackManual.TabIndex = 0;
            this.rdbBlackManual.TabStop = true;
            this.rdbBlackManual.Text = "手动";
            this.rdbBlackManual.UseVisualStyleBackColor = true;
            // 
            // pnlWhiteMode
            // 
            this.pnlWhiteMode.Controls.Add(this.label3);
            this.pnlWhiteMode.Controls.Add(this.rdbWhiteAuto);
            this.pnlWhiteMode.Controls.Add(this.rdbWhiteManual);
            this.pnlWhiteMode.Location = new System.Drawing.Point(12, 26);
            this.pnlWhiteMode.Name = "pnlWhiteMode";
            this.pnlWhiteMode.Size = new System.Drawing.Size(125, 90);
            this.pnlWhiteMode.TabIndex = 0;
            this.pnlWhiteMode.Paint += new System.Windows.Forms.PaintEventHandler(this.panel6_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(39, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "白方";
            // 
            // rdbWhiteAuto
            // 
            this.rdbWhiteAuto.AutoSize = true;
            this.rdbWhiteAuto.Location = new System.Drawing.Point(29, 63);
            this.rdbWhiteAuto.Name = "rdbWhiteAuto";
            this.rdbWhiteAuto.Size = new System.Drawing.Size(60, 24);
            this.rdbWhiteAuto.TabIndex = 1;
            this.rdbWhiteAuto.Text = "自动";
            this.rdbWhiteAuto.UseVisualStyleBackColor = true;
            // 
            // rdbWhiteManual
            // 
            this.rdbWhiteManual.AutoSize = true;
            this.rdbWhiteManual.Checked = true;
            this.rdbWhiteManual.Location = new System.Drawing.Point(29, 33);
            this.rdbWhiteManual.Name = "rdbWhiteManual";
            this.rdbWhiteManual.Size = new System.Drawing.Size(60, 24);
            this.rdbWhiteManual.TabIndex = 0;
            this.rdbWhiteManual.TabStop = true;
            this.rdbWhiteManual.Text = "手动";
            this.rdbWhiteManual.UseVisualStyleBackColor = true;
            // 
            // btnLoadWhiteAI
            // 
            this.btnLoadWhiteAI.Location = new System.Drawing.Point(35, 22);
            this.btnLoadWhiteAI.Name = "btnLoadWhiteAI";
            this.btnLoadWhiteAI.Size = new System.Drawing.Size(134, 40);
            this.btnLoadWhiteAI.TabIndex = 23;
            this.btnLoadWhiteAI.Text = "载入白方AI(&L)...";
            this.btnLoadWhiteAI.UseVisualStyleBackColor = true;
            this.btnLoadWhiteAI.Click += new System.EventHandler(this.btnLoadWhiteAI_Click);
            // 
            // btnLoadBlackAI
            // 
            this.btnLoadBlackAI.Location = new System.Drawing.Point(180, 22);
            this.btnLoadBlackAI.Name = "btnLoadBlackAI";
            this.btnLoadBlackAI.Size = new System.Drawing.Size(134, 40);
            this.btnLoadBlackAI.TabIndex = 24;
            this.btnLoadBlackAI.Text = "载入黑方AI(&O)...";
            this.btnLoadBlackAI.UseVisualStyleBackColor = true;
            this.btnLoadBlackAI.Click += new System.EventHandler(this.btnLoadBlackAI_Click);
            // 
            // btnBlackReadMove
            // 
            this.btnBlackReadMove.Location = new System.Drawing.Point(180, 68);
            this.btnBlackReadMove.Name = "btnBlackReadMove";
            this.btnBlackReadMove.Size = new System.Drawing.Size(134, 38);
            this.btnBlackReadMove.TabIndex = 25;
            this.btnBlackReadMove.Text = "黑方读入一步(&U)";
            this.btnBlackReadMove.UseVisualStyleBackColor = true;
            this.btnBlackReadMove.Click += new System.EventHandler(this.btnBlackReadMove_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(23, 431);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(298, 399);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "历史";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rtbLog);
            this.groupBox7.Location = new System.Drawing.Point(916, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(349, 818);
            this.groupBox7.TabIndex = 27;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "日志";
            // 
            // rtbLog
            // 
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLog.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbLog.Location = new System.Drawing.Point(7, 27);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(336, 785);
            this.rtbLog.TabIndex = 30;
            this.rtbLog.Text = "测试日志内容";
            // 
            // cmsAbout
            // 
            this.cmsAbout.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsAbout.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smiAbout,
            this.smiQuote});
            this.cmsAbout.Name = "cmsAbout";
            this.cmsAbout.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmsAbout.Size = new System.Drawing.Size(121, 52);
            // 
            // smiAbout
            // 
            this.smiAbout.AutoSize = false;
            this.smiAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.smiAbout.Name = "smiAbout";
            this.smiAbout.Size = new System.Drawing.Size(120, 24);
            this.smiAbout.Text = "关于...";
            this.smiAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.smiAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.smiAbout.Click += new System.EventHandler(this.smiAbout_Click);
            // 
            // smiQuote
            // 
            this.smiQuote.AutoSize = false;
            this.smiQuote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.smiQuote.Name = "smiQuote";
            this.smiQuote.Size = new System.Drawing.Size(120, 24);
            this.smiQuote.Text = "名言...";
            this.smiQuote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.smiQuote.Click += new System.EventHandler(this.smiQuote_Click);
            // 
            // TheChessBoard
            // 
            this.AcceptButton = this.btnMove;
            this.ClientSize = new System.Drawing.Size(1277, 842);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.btnBlackReadMove);
            this.Controls.Add(this.btnLoadBlackAI);
            this.Controls.Add(this.btnLoadWhiteAI);
            this.Controls.Add(this.btnWhiteReadMove);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TheChessBoard";
            this.Text = "The Chess Board";
            this.Load += new System.EventHandler(this.PrimitiveBoard_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.pnlBlackMode.ResumeLayout(false);
            this.pnlBlackMode.PerformLayout();
            this.pnlWhiteMode.ResumeLayout(false);
            this.pnlWhiteMode.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.cmsAbout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.TextBox txbMoveStr;
        private System.Windows.Forms.RadioButton rdbWhite;
        private System.Windows.Forms.RadioButton rdbBlack;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnlBoard;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox ckbDontCareWhoseTurnItIs;
        private System.Windows.Forms.Label lblCurrentPlayer;
        private System.Windows.Forms.Label lblWhiteWatch;
        private System.Windows.Forms.Button btnWhiteReadMove;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblWhiteStatusText;
        private System.Windows.Forms.Label lblWhiteStatusCaption;
        private System.Windows.Forms.Label lblWhiteStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblFormStatusText;
        private System.Windows.Forms.Label lblFormStatusCaption;
        private System.Windows.Forms.Label lblFormStatus;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblBlackStatusText;
        private System.Windows.Forms.Label lblBlackStatusCaption;
        private System.Windows.Forms.Label lblBlackStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel pnlWhiteMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdbWhiteAuto;
        private System.Windows.Forms.RadioButton rdbWhiteManual;
        private System.Windows.Forms.Button btnModeConfirm;
        private System.Windows.Forms.Panel pnlBlackMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdbBlackAuto;
        private System.Windows.Forms.RadioButton rdbBlackManual;
        private System.Windows.Forms.Button btnAllReset;
        private System.Windows.Forms.Button btnLoadWhiteAI;
        private System.Windows.Forms.Button btnLoadBlackAI;
        private System.Windows.Forms.Button btnBlackReadMove;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txbFen;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label lblBlackWatch;
        private System.Windows.Forms.Button btnPauseCont;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.ContextMenuStrip cmsAbout;
        private System.Windows.Forms.ToolStripMenuItem smiAbout;
        private System.Windows.Forms.ToolStripMenuItem smiQuote;
    }
}