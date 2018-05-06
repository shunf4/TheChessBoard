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
            this.btnWhiteReadLine = new System.Windows.Forms.Button();
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
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnWhiteReadLine
            // 
            this.btnWhiteReadLine.Location = new System.Drawing.Point(40, 31);
            this.btnWhiteReadLine.Name = "btnWhiteReadLine";
            this.btnWhiteReadLine.Size = new System.Drawing.Size(125, 53);
            this.btnWhiteReadLine.TabIndex = 1;
            this.btnWhiteReadLine.Text = "白方读入一行";
            this.btnWhiteReadLine.UseVisualStyleBackColor = true;
            this.btnWhiteReadLine.Visible = false;
            // 
            // btnBlackReadLine
            // 
            this.btnBlackReadLine.Location = new System.Drawing.Point(211, 31);
            this.btnBlackReadLine.Name = "btnBlackReadLine";
            this.btnBlackReadLine.Size = new System.Drawing.Size(125, 53);
            this.btnBlackReadLine.TabIndex = 2;
            this.btnBlackReadLine.Text = "黑方读入一行";
            this.btnBlackReadLine.UseVisualStyleBackColor = true;
            this.btnBlackReadLine.Visible = false;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(458, 640);
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
            this.rdbWhite.Location = new System.Drawing.Point(27, 24);
            this.rdbWhite.Name = "rdbWhite";
            this.rdbWhite.Size = new System.Drawing.Size(184, 30);
            this.rdbWhite.TabIndex = 5;
            this.rdbWhite.TabStop = true;
            this.rdbWhite.Text = "白方(&W)";
            this.rdbWhite.UseVisualStyleBackColor = true;
            this.rdbWhite.CheckedChanged += new System.EventHandler(this.SANPlayerChanged);
            // 
            // rdbBlack
            // 
            this.rdbBlack.Location = new System.Drawing.Point(27, 60);
            this.rdbBlack.Name = "rdbBlack";
            this.rdbBlack.Size = new System.Drawing.Size(184, 34);
            this.rdbBlack.TabIndex = 6;
            this.rdbBlack.Text = "黑方(&B)";
            this.rdbBlack.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbWhite);
            this.groupBox1.Controls.Add(this.rdbBlack);
            this.groupBox1.Location = new System.Drawing.Point(220, 599);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 105);
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
            this.panel1.Location = new System.Drawing.Point(8, 624);
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
            this.panel2.Size = new System.Drawing.Size(567, 720);
            this.panel2.TabIndex = 17;
            // 
            // ckbDontCareWhoseTurnItIs
            // 
            this.ckbDontCareWhoseTurnItIs.AutoSize = true;
            this.ckbDontCareWhoseTurnItIs.Location = new System.Drawing.Point(384, 12);
            this.ckbDontCareWhoseTurnItIs.Name = "ckbDontCareWhoseTurnItIs";
            this.ckbDontCareWhoseTurnItIs.Size = new System.Drawing.Size(135, 24);
            this.ckbDontCareWhoseTurnItIs.TabIndex = 18;
            this.ckbDontCareWhoseTurnItIs.Text = "忽略当前执棋(&I)";
            this.ckbDontCareWhoseTurnItIs.UseVisualStyleBackColor = true;
            this.ckbDontCareWhoseTurnItIs.CheckedChanged += new System.EventHandler(this.ckbDontCareWhoseTurnItIs_CheckedChanged);
            // 
            // lblCurrentPlayer
            // 
            this.lblCurrentPlayer.Location = new System.Drawing.Point(233, 13);
            this.lblCurrentPlayer.Name = "lblCurrentPlayer";
            this.lblCurrentPlayer.Size = new System.Drawing.Size(107, 23);
            this.lblCurrentPlayer.TabIndex = 17;
            this.lblCurrentPlayer.Text = "当前执棋：";
            // 
            // PrimitiveBoard
            // 
            this.AcceptButton = this.btnMove;
            this.ClientSize = new System.Drawing.Size(925, 744);
            this.Controls.Add(this.btnBlackReadLine);
            this.Controls.Add(this.btnWhiteReadLine);
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
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnWhiteReadLine;
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
    }
}