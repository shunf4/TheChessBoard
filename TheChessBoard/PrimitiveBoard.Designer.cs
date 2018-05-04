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
            // 
            // btnBlackReadLine
            // 
            this.btnBlackReadLine.Location = new System.Drawing.Point(775, 135);
            this.btnBlackReadLine.Name = "btnBlackReadLine";
            this.btnBlackReadLine.Size = new System.Drawing.Size(125, 53);
            this.btnBlackReadLine.TabIndex = 2;
            this.btnBlackReadLine.Text = "黑方读入一行";
            this.btnBlackReadLine.UseVisualStyleBackColor = true;
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
            this.txbBoard.Text = "RNRNRNRN\r\nKQKQKQKQ\r\nkqkqkqkq\r\nbbbbbbbb\r\noooooooo\r\n";
            this.txbBoard.WordWrap = false;
            // 
            // PrimitiveBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 717);
            this.Controls.Add(this.btnBlackReadLine);
            this.Controls.Add(this.btnWhiteReadLine);
            this.Controls.Add(this.txbBoard);
            this.Name = "PrimitiveBoard";
            this.Text = "PrimitiveBoard";
            this.Load += new System.EventHandler(this.PrimitiveBoard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnWhiteReadLine;
        private System.Windows.Forms.Button btnBlackReadLine;
        private System.Windows.Forms.TextBox txbBoard;
    }
}