namespace TheChessBoard
{
    partial class PrimitiveBoard
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
            this.btnWhiteProcStart = new System.Windows.Forms.Button();
            this.btnBlackReadLine = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.txbMoveStr = new System.Windows.Forms.TextBox();
            this.rdbWhite = new System.Windows.Forms.RadioButton();
            this.rdbBlack = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ckbDontCareWhoseTurnItIs = new System.Windows.Forms.CheckBox();
            this.lblCurrentPlayer = new System.Windows.Forms.Label();
            this.lblWhiteWatch = new System.Windows.Forms.Label();
            this.btnWhiteReadLine = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblFormStatus = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblFormStatusCaption = new System.Windows.Forms.Label();
            this.lblFormStatusText = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWhiteStatusCaption = new System.Windows.Forms.Label();
            this.lblWhiteStatus = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblBlackAIStatusText = new System.Windows.Forms.Label();
            this.lblBlackStatusCaption = new System.Windows.Forms.Label();
            this.lblBlackStatus = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.rdbWhiteManual = new System.Windows.Forms.RadioButton();
            this.rdbWhiteAuto = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbBlackAuto = new System.Windows.Forms.RadioButton();
            this.rdbBlackManual = new System.Windows.Forms.RadioButton();
            this.btnModeConfirm = new System.Windows.Forms.Button();
            this.dgvHistoryMoves = new System.Windows.Forms.DataGridView();
            this.btnAllReset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistoryMoves)).BeginInit();
            this.SuspendLayout();
            // 
            // btnWhiteProcStart
            // 
            this.btnWhiteProcStart.Location = new System.Drawing.Point(35, 31);
            this.btnWhiteProcStart.Name = "btnWhiteProcStart";
            this.btnWhiteProcStart.Size = new System.Drawing.Size(125, 53);
            this.btnWhiteProcStart.TabIndex = 1;
            this.btnWhiteProcStart.Text = "白方开始运行";
            this.btnWhiteProcStart.UseVisualStyleBackColor = true;
            this.btnWhiteProcStart.Click += new System.EventHandler(this.btnWhiteProcStart_Click);
            // 
            // btnBlackReadLine
            // 
            this.btnBlackReadLine.Location = new System.Drawing.Point(182, 31);
            this.btnBlackReadLine.Name = "btnBlackReadLine";
            this.btnBlackReadLine.Size = new System.Drawing.Size(125, 53);
            this.btnBlackReadLine.TabIndex = 2;
            this.btnBlackReadLine.Text = "黑方读入一行";
            this.btnBlackReadLine.UseVisualStyleBackColor = true;
            this.btnBlackReadLine.Visible = false;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(457, 625);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(91, 30);
            this.btnMove.TabIndex = 15;
            this.btnMove.Text = "走一步(&S)";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // txbMoveStr
            // 
            this.txbMoveStr.Location = new System.Drawing.Point(77, 16);
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
            this.groupBox1.Location = new System.Drawing.Point(210, 597);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 71);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "执白执黑";
            // 
            // pnlBoard
            // 
            this.pnlBoard.Location = new System.Drawing.Point(8, 45);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(552, 536);
            this.pnlBoard.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txbMoveStr);
            this.panel1.Location = new System.Drawing.Point(9, 607);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 59);
            this.panel1.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "输入S&AN";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ckbDontCareWhoseTurnItIs);
            this.panel2.Controls.Add(this.lblCurrentPlayer);
            this.panel2.Controls.Add(this.btnMove);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.pnlBoard);
            this.panel2.Location = new System.Drawing.Point(342, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(567, 679);
            this.panel2.TabIndex = 17;
            // 
            // ckbDontCareWhoseTurnItIs
            // 
            this.ckbDontCareWhoseTurnItIs.AutoSize = true;
            this.ckbDontCareWhoseTurnItIs.Location = new System.Drawing.Point(384, 10);
            this.ckbDontCareWhoseTurnItIs.Name = "ckbDontCareWhoseTurnItIs";
            this.ckbDontCareWhoseTurnItIs.Size = new System.Drawing.Size(135, 24);
            this.ckbDontCareWhoseTurnItIs.TabIndex = 18;
            this.ckbDontCareWhoseTurnItIs.Text = "忽略当前执棋(&I)";
            this.ckbDontCareWhoseTurnItIs.UseVisualStyleBackColor = true;
            this.ckbDontCareWhoseTurnItIs.CheckedChanged += new System.EventHandler(this.ckbDontCareWhoseTurnItIs_CheckedChanged);
            // 
            // lblCurrentPlayer
            // 
            this.lblCurrentPlayer.Location = new System.Drawing.Point(233, 11);
            this.lblCurrentPlayer.Name = "lblCurrentPlayer";
            this.lblCurrentPlayer.Size = new System.Drawing.Size(107, 23);
            this.lblCurrentPlayer.TabIndex = 17;
            this.lblCurrentPlayer.Text = "当前执棋：";
            // 
            // lblWhiteWatch
            // 
            this.lblWhiteWatch.AutoSize = true;
            this.lblWhiteWatch.Location = new System.Drawing.Point(216, 4);
            this.lblWhiteWatch.Name = "lblWhiteWatch";
            this.lblWhiteWatch.Size = new System.Drawing.Size(53, 20);
            this.lblWhiteWatch.TabIndex = 18;
            this.lblWhiteWatch.Text = "--:--.--";
            // 
            // btnWhiteReadLine
            // 
            this.btnWhiteReadLine.Location = new System.Drawing.Point(35, 90);
            this.btnWhiteReadLine.Name = "btnWhiteReadLine";
            this.btnWhiteReadLine.Size = new System.Drawing.Size(125, 53);
            this.btnWhiteReadLine.TabIndex = 19;
            this.btnWhiteReadLine.Text = "白方读入一行";
            this.btnWhiteReadLine.UseVisualStyleBackColor = true;
            this.btnWhiteReadLine.Click += new System.EventHandler(this.btnWhiteReadLine_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Location = new System.Drawing.Point(23, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 132);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状态";
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
            // lblFormStatusCaption
            // 
            this.lblFormStatusCaption.AutoSize = true;
            this.lblFormStatusCaption.Location = new System.Drawing.Point(26, 4);
            this.lblFormStatusCaption.Name = "lblFormStatusCaption";
            this.lblFormStatusCaption.Size = new System.Drawing.Size(52, 20);
            this.lblFormStatusCaption.TabIndex = 22;
            this.lblFormStatusCaption.Text = "Board";
            this.lblFormStatusCaption.Click += new System.EventHandler(this.lblFormStatusCaption_Click);
            // 
            // lblFormStatusText
            // 
            this.lblFormStatusText.AutoSize = true;
            this.lblFormStatusText.Location = new System.Drawing.Point(110, 4);
            this.lblFormStatusText.Name = "lblFormStatusText";
            this.lblFormStatusText.Size = new System.Drawing.Size(54, 20);
            this.lblFormStatusText.TabIndex = 23;
            this.lblFormStatusText.Text = "无状态";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.lblWhiteStatusCaption);
            this.panel4.Controls.Add(this.lblWhiteWatch);
            this.panel4.Controls.Add(this.lblWhiteStatus);
            this.panel4.Location = new System.Drawing.Point(12, 60);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(272, 31);
            this.panel4.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.TabIndex = 23;
            this.label2.Text = "无状态";
            // 
            // lblWhiteStatusCaption
            // 
            this.lblWhiteStatusCaption.AutoSize = true;
            this.lblWhiteStatusCaption.Location = new System.Drawing.Point(26, 4);
            this.lblWhiteStatusCaption.Name = "lblWhiteStatusCaption";
            this.lblWhiteStatusCaption.Size = new System.Drawing.Size(71, 20);
            this.lblWhiteStatusCaption.TabIndex = 22;
            this.lblWhiteStatusCaption.Text = "White AI";
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
            // panel5
            // 
            this.panel5.Controls.Add(this.lblBlackAIStatusText);
            this.panel5.Controls.Add(this.lblBlackStatusCaption);
            this.panel5.Controls.Add(this.lblBlackStatus);
            this.panel5.Location = new System.Drawing.Point(12, 94);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(272, 31);
            this.panel5.TabIndex = 25;
            // 
            // lblBlackAIStatusText
            // 
            this.lblBlackAIStatusText.AutoSize = true;
            this.lblBlackAIStatusText.Location = new System.Drawing.Point(110, 4);
            this.lblBlackAIStatusText.Name = "lblBlackAIStatusText";
            this.lblBlackAIStatusText.Size = new System.Drawing.Size(54, 20);
            this.lblBlackAIStatusText.TabIndex = 23;
            this.lblBlackAIStatusText.Text = "无状态";
            // 
            // lblBlackStatusCaption
            // 
            this.lblBlackStatusCaption.AutoSize = true;
            this.lblBlackStatusCaption.Location = new System.Drawing.Point(26, 4);
            this.lblBlackStatusCaption.Name = "lblBlackStatusCaption";
            this.lblBlackStatusCaption.Size = new System.Drawing.Size(65, 20);
            this.lblBlackStatusCaption.TabIndex = 22;
            this.lblBlackStatusCaption.Text = "Black AI";
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAllReset);
            this.groupBox3.Controls.Add(this.btnModeConfirm);
            this.groupBox3.Controls.Add(this.panel7);
            this.groupBox3.Controls.Add(this.panel6);
            this.groupBox3.Location = new System.Drawing.Point(23, 302);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 164);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "模式";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.rdbWhiteAuto);
            this.panel6.Controls.Add(this.rdbWhiteManual);
            this.panel6.Location = new System.Drawing.Point(12, 26);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(125, 90);
            this.panel6.TabIndex = 0;
            this.panel6.Paint += new System.Windows.Forms.PaintEventHandler(this.panel6_Paint);
            // 
            // rdbWhiteManual
            // 
            this.rdbWhiteManual.AutoSize = true;
            this.rdbWhiteManual.Checked = true;
            this.rdbWhiteManual.Location = new System.Drawing.Point(14, 33);
            this.rdbWhiteManual.Name = "rdbWhiteManual";
            this.rdbWhiteManual.Size = new System.Drawing.Size(90, 24);
            this.rdbWhiteManual.TabIndex = 0;
            this.rdbWhiteManual.TabStop = true;
            this.rdbWhiteManual.Text = "手动操作";
            this.rdbWhiteManual.UseVisualStyleBackColor = true;
            // 
            // rdbWhiteAuto
            // 
            this.rdbWhiteAuto.AutoSize = true;
            this.rdbWhiteAuto.Location = new System.Drawing.Point(14, 63);
            this.rdbWhiteAuto.Name = "rdbWhiteAuto";
            this.rdbWhiteAuto.Size = new System.Drawing.Size(90, 24);
            this.rdbWhiteAuto.TabIndex = 1;
            this.rdbWhiteAuto.Text = "读入程序";
            this.rdbWhiteAuto.UseVisualStyleBackColor = true;
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
            // panel7
            // 
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.rdbBlackAuto);
            this.panel7.Controls.Add(this.rdbBlackManual);
            this.panel7.Location = new System.Drawing.Point(156, 26);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(125, 90);
            this.panel7.TabIndex = 3;
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
            this.rdbBlackAuto.Location = new System.Drawing.Point(14, 63);
            this.rdbBlackAuto.Name = "rdbBlackAuto";
            this.rdbBlackAuto.Size = new System.Drawing.Size(90, 24);
            this.rdbBlackAuto.TabIndex = 1;
            this.rdbBlackAuto.Text = "读入程序";
            this.rdbBlackAuto.UseVisualStyleBackColor = true;
            // 
            // rdbBlackManual
            // 
            this.rdbBlackManual.AutoSize = true;
            this.rdbBlackManual.Checked = true;
            this.rdbBlackManual.Location = new System.Drawing.Point(14, 33);
            this.rdbBlackManual.Name = "rdbBlackManual";
            this.rdbBlackManual.Size = new System.Drawing.Size(90, 24);
            this.rdbBlackManual.TabIndex = 0;
            this.rdbBlackManual.TabStop = true;
            this.rdbBlackManual.Text = "手动操作";
            this.rdbBlackManual.UseVisualStyleBackColor = true;
            // 
            // btnModeConfirm
            // 
            this.btnModeConfirm.Location = new System.Drawing.Point(12, 126);
            this.btnModeConfirm.Name = "btnModeConfirm";
            this.btnModeConfirm.Size = new System.Drawing.Size(141, 30);
            this.btnModeConfirm.TabIndex = 4;
            this.btnModeConfirm.Text = "切换模式并开始";
            this.btnModeConfirm.UseVisualStyleBackColor = true;
            // 
            // dgvHistoryMoves
            // 
            this.dgvHistoryMoves.AllowUserToAddRows = false;
            this.dgvHistoryMoves.AllowUserToDeleteRows = false;
            this.dgvHistoryMoves.AllowUserToResizeRows = false;
            this.dgvHistoryMoves.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvHistoryMoves.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistoryMoves.Location = new System.Drawing.Point(23, 477);
            this.dgvHistoryMoves.Name = "dgvHistoryMoves";
            this.dgvHistoryMoves.ReadOnly = true;
            this.dgvHistoryMoves.RowTemplate.Height = 27;
            this.dgvHistoryMoves.Size = new System.Drawing.Size(298, 209);
            this.dgvHistoryMoves.TabIndex = 22;
            // 
            // btnAllReset
            // 
            this.btnAllReset.Location = new System.Drawing.Point(170, 126);
            this.btnAllReset.Name = "btnAllReset";
            this.btnAllReset.Size = new System.Drawing.Size(109, 30);
            this.btnAllReset.TabIndex = 5;
            this.btnAllReset.Text = "全部重置";
            this.btnAllReset.UseVisualStyleBackColor = true;
            // 
            // PrimitiveBoard
            // 
            this.AcceptButton = this.btnMove;
            this.ClientSize = new System.Drawing.Size(925, 703);
            this.Controls.Add(this.dgvHistoryMoves);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnWhiteReadLine);
            this.Controls.Add(this.btnBlackReadLine);
            this.Controls.Add(this.btnWhiteProcStart);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PrimitiveBoard";
            this.Text = "原始棋盘 & 代数记谱法测试器";
            this.Load += new System.EventHandler(this.PrimitiveBoard_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistoryMoves)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnWhiteProcStart;
        private System.Windows.Forms.Button btnBlackReadLine;
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
        private System.Windows.Forms.Button btnWhiteReadLine;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblWhiteStatusCaption;
        private System.Windows.Forms.Label lblWhiteStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblFormStatusText;
        private System.Windows.Forms.Label lblFormStatusCaption;
        private System.Windows.Forms.Label lblFormStatus;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblBlackAIStatusText;
        private System.Windows.Forms.Label lblBlackStatusCaption;
        private System.Windows.Forms.Label lblBlackStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdbWhiteAuto;
        private System.Windows.Forms.RadioButton rdbWhiteManual;
        private System.Windows.Forms.Button btnModeConfirm;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdbBlackAuto;
        private System.Windows.Forms.RadioButton rdbBlackManual;
        private System.Windows.Forms.DataGridView dgvHistoryMoves;
        private System.Windows.Forms.Button btnAllReset;
    }
}