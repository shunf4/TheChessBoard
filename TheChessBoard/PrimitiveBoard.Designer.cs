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
            this.txbBoard = new System.Windows.Forms.TextBox();
            this.btnMove = new System.Windows.Forms.Button();
            this.txbMoveStr = new System.Windows.Forms.TextBox();
            this.rdbWhite = new System.Windows.Forms.RadioButton();
            this.rdbBlack = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnWhiteReadLine
            // 
            this.btnWhiteReadLine.Location = new System.Drawing.Point(48, 135);
            this.btnWhiteReadLine.Name = "btnWhiteReadLine";
            this.btnWhiteReadLine.Size = new System.Drawing.Size(125, 53);
            this.btnWhiteReadLine.TabIndex = 1;
            this.btnWhiteReadLine.Text = "白方读入一行";
            this.btnWhiteReadLine.UseVisualStyleBackColor = true;
            this.btnWhiteReadLine.Visible = false;
            // 
            // btnBlackReadLine
            // 
            this.btnBlackReadLine.Location = new System.Drawing.Point(775, 135);
            this.btnBlackReadLine.Name = "btnBlackReadLine";
            this.btnBlackReadLine.Size = new System.Drawing.Size(125, 53);
            this.btnBlackReadLine.TabIndex = 2;
            this.btnBlackReadLine.Text = "黑方读入一行";
            this.btnBlackReadLine.UseVisualStyleBackColor = true;
            this.btnBlackReadLine.Visible = false;
            // 
            // txbBoard
            // 
            this.txbBoard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txbBoard.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txbBoard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbBoard.Cursor = System.Windows.Forms.Cursors.Cross;
            this.txbBoard.Font = new System.Drawing.Font("Chess Leipzig", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.txbBoard.Location = new System.Drawing.Point(196, 65);
            this.txbBoard.Margin = new System.Windows.Forms.Padding(0);
            this.txbBoard.Multiline = true;
            this.txbBoard.Name = "txbBoard";
            this.txbBoard.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txbBoard.Size = new System.Drawing.Size(559, 525);
            this.txbBoard.TabIndex = 2;
            this.txbBoard.WordWrap = false;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(618, 632);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(108, 38);
            this.btnMove.TabIndex = 3;
            this.btnMove.Text = "走一步(&S)";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // txbMoveStr
            // 
            this.txbMoveStr.Location = new System.Drawing.Point(211, 633);
            this.txbMoveStr.Name = "txbMoveStr";
            this.txbMoveStr.Size = new System.Drawing.Size(184, 25);
            this.txbMoveStr.TabIndex = 0;
            // 
            // rdbWhite
            // 
            this.rdbWhite.AutoSize = true;
            this.rdbWhite.Checked = true;
            this.rdbWhite.Location = new System.Drawing.Point(46, 29);
            this.rdbWhite.Name = "rdbWhite";
            this.rdbWhite.Size = new System.Drawing.Size(58, 19);
            this.rdbWhite.TabIndex = 5;
            this.rdbWhite.TabStop = true;
            this.rdbWhite.Text = "白方";
            this.rdbWhite.UseVisualStyleBackColor = true;
            this.rdbWhite.CheckedChanged += new System.EventHandler(this.playerCheckedChanged);
            // 
            // rdbBlack
            // 
            this.rdbBlack.AutoSize = true;
            this.rdbBlack.Location = new System.Drawing.Point(46, 64);
            this.rdbBlack.Name = "rdbBlack";
            this.rdbBlack.Size = new System.Drawing.Size(58, 19);
            this.rdbBlack.TabIndex = 6;
            this.rdbBlack.Text = "黑方";
            this.rdbBlack.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbWhite);
            this.groupBox1.Controls.Add(this.rdbBlack);
            this.groupBox1.Location = new System.Drawing.Point(412, 605);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(173, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "执白执黑";
            // 
            // PrimitiveBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 717);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txbMoveStr);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnBlackReadLine);
            this.Controls.Add(this.btnWhiteReadLine);
            this.Controls.Add(this.txbBoard);
            this.Name = "PrimitiveBoard";
            this.Text = "代数记谱法验证器";
            this.Load += new System.EventHandler(this.PrimitiveBoard_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnWhiteReadLine;
        private System.Windows.Forms.Button btnBlackReadLine;
        private System.Windows.Forms.TextBox txbBoard;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.TextBox txbMoveStr;
        private System.Windows.Forms.RadioButton rdbWhite;
        private System.Windows.Forms.RadioButton rdbBlack;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}