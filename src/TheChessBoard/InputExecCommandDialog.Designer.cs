namespace TheChessBoard
{
    partial class InputExecCommandDialog
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
            this.txbExecPath = new System.Windows.Forms.ComboBox();
            this.txbExecArguments = new System.Windows.Forms.ComboBox();
            this.lblExecPathCaption = new System.Windows.Forms.Label();
            this.lblExecArgumentsCaption = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txbFileDialogExec = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.txbFileDialogExecArg = new System.Windows.Forms.Button();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbExecPath
            // 
            this.txbExecPath.Location = new System.Drawing.Point(253, 22);
            this.txbExecPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbExecPath.Name = "txbExecPath";
            this.txbExecPath.Size = new System.Drawing.Size(501, 27);
            this.txbExecPath.TabIndex = 0;
            // 
            // txbExecArguments
            // 
            this.txbExecArguments.Location = new System.Drawing.Point(253, 75);
            this.txbExecArguments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbExecArguments.Name = "txbExecArguments";
            this.txbExecArguments.Size = new System.Drawing.Size(501, 27);
            this.txbExecArguments.TabIndex = 1;
            // 
            // lblExecPathCaption
            // 
            this.lblExecPathCaption.AutoSize = true;
            this.lblExecPathCaption.Location = new System.Drawing.Point(27, 25);
            this.lblExecPathCaption.Name = "lblExecPathCaption";
            this.lblExecPathCaption.Size = new System.Drawing.Size(174, 20);
            this.lblExecPathCaption.TabIndex = 2;
            this.lblExecPathCaption.Text = "红方AI的可执行文件路径";
            // 
            // lblExecArgumentsCaption
            // 
            this.lblExecArgumentsCaption.AutoSize = true;
            this.lblExecArgumentsCaption.Location = new System.Drawing.Point(27, 78);
            this.lblExecArgumentsCaption.Name = "lblExecArgumentsCaption";
            this.lblExecArgumentsCaption.Size = new System.Drawing.Size(129, 20);
            this.lblExecArgumentsCaption.TabIndex = 3;
            this.lblExecArgumentsCaption.Text = "红方AI的运行参数";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(656, 142);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(87, 33);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "确认(&O)";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(761, 142);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 33);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txbFileDialogExec
            // 
            this.txbFileDialogExec.Location = new System.Drawing.Point(776, 22);
            this.txbFileDialogExec.Name = "txbFileDialogExec";
            this.txbFileDialogExec.Size = new System.Drawing.Size(72, 27);
            this.txbFileDialogExec.TabIndex = 6;
            this.txbFileDialogExec.Text = "浏览...";
            this.txbFileDialogExec.UseVisualStyleBackColor = true;
            this.txbFileDialogExec.Click += new System.EventHandler(this.txbFileDialogExec_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlMain.Controls.Add(this.txbFileDialogExecArg);
            this.pnlMain.Controls.Add(this.txbFileDialogExec);
            this.pnlMain.Controls.Add(this.txbExecPath);
            this.pnlMain.Controls.Add(this.txbExecArguments);
            this.pnlMain.Controls.Add(this.lblExecArgumentsCaption);
            this.pnlMain.Controls.Add(this.lblExecPathCaption);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(875, 128);
            this.pnlMain.TabIndex = 7;
            // 
            // txbFileDialogExecArg
            // 
            this.txbFileDialogExecArg.Location = new System.Drawing.Point(776, 75);
            this.txbFileDialogExecArg.Name = "txbFileDialogExecArg";
            this.txbFileDialogExecArg.Size = new System.Drawing.Size(72, 27);
            this.txbFileDialogExecArg.TabIndex = 7;
            this.txbFileDialogExecArg.Text = "插入...";
            this.txbFileDialogExecArg.UseVisualStyleBackColor = true;
            this.txbFileDialogExecArg.Click += new System.EventHandler(this.txbFileDialogExecArg_Click);
            // 
            // InputExecCommandDialog
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(876, 189);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputExecCommandDialog";
            this.ShowIcon = false;
            this.Text = "设置红方AI";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblExecPathCaption;
        private System.Windows.Forms.Label lblExecArgumentsCaption;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button txbFileDialogExec;
        public System.Windows.Forms.ComboBox txbExecPath;
        public System.Windows.Forms.ComboBox txbExecArguments;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button txbFileDialogExecArg;
    }
}