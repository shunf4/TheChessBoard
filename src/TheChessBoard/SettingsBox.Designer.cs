namespace TheChessBoard
{
    partial class SettingsBox
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.ckbHideAIWindow = new System.Windows.Forms.CheckBox();
            this.nudWatchChessTime = new System.Windows.Forms.NumericUpDown();
            this.btnBrowse4 = new System.Windows.Forms.Button();
            this.txbBlackDefaultExecArguments = new System.Windows.Forms.TextBox();
            this.btnBrowse3 = new System.Windows.Forms.Button();
            this.txbBlackDefaultExecPath = new System.Windows.Forms.TextBox();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.txbWhiteDefaultExecArguments = new System.Windows.Forms.TextBox();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.txbWhiteDefaultExecPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ckbAutoSaveAIConfig = new System.Windows.Forms.CheckBox();
            this.btnSaveAndExit = new System.Windows.Forms.Button();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWatchChessTime)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlMain.Controls.Add(this.ckbHideAIWindow);
            this.pnlMain.Controls.Add(this.nudWatchChessTime);
            this.pnlMain.Controls.Add(this.btnBrowse4);
            this.pnlMain.Controls.Add(this.txbBlackDefaultExecArguments);
            this.pnlMain.Controls.Add(this.btnBrowse3);
            this.pnlMain.Controls.Add(this.txbBlackDefaultExecPath);
            this.pnlMain.Controls.Add(this.btnBrowse2);
            this.pnlMain.Controls.Add(this.txbWhiteDefaultExecArguments);
            this.pnlMain.Controls.Add(this.btnBrowse1);
            this.pnlMain.Controls.Add(this.txbWhiteDefaultExecPath);
            this.pnlMain.Controls.Add(this.label5);
            this.pnlMain.Controls.Add(this.label6);
            this.pnlMain.Controls.Add(this.label4);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.ckbAutoSaveAIConfig);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(862, 344);
            this.pnlMain.TabIndex = 0;
            // 
            // ckbHideAIWindow
            // 
            this.ckbHideAIWindow.AutoSize = true;
            this.ckbHideAIWindow.Checked = global::TheChessBoard.Properties.Settings.Default.HideAIWindow;
            this.ckbHideAIWindow.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TheChessBoard.Properties.Settings.Default, "HideAIWindow", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ckbHideAIWindow.Location = new System.Drawing.Point(292, 33);
            this.ckbHideAIWindow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckbHideAIWindow.Name = "ckbHideAIWindow";
            this.ckbHideAIWindow.Size = new System.Drawing.Size(358, 24);
            this.ckbHideAIWindow.TabIndex = 17;
            this.ckbHideAIWindow.Text = "运行 AI 程序时隐藏窗口（修改后需重新载入 AI）";
            this.ckbHideAIWindow.UseVisualStyleBackColor = true;
            // 
            // nudWatchChessTime
            // 
            this.nudWatchChessTime.Location = new System.Drawing.Point(292, 79);
            this.nudWatchChessTime.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.nudWatchChessTime.Name = "nudWatchChessTime";
            this.nudWatchChessTime.Size = new System.Drawing.Size(120, 27);
            this.nudWatchChessTime.TabIndex = 16;
            this.nudWatchChessTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnBrowse4
            // 
            this.btnBrowse4.Location = new System.Drawing.Point(748, 280);
            this.btnBrowse4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowse4.Name = "btnBrowse4";
            this.btnBrowse4.Size = new System.Drawing.Size(84, 31);
            this.btnBrowse4.TabIndex = 15;
            this.btnBrowse4.Text = "插入...";
            this.btnBrowse4.UseVisualStyleBackColor = true;
            this.btnBrowse4.Click += new System.EventHandler(this.btnBrowse4_Click);
            // 
            // txbBlackDefaultExecArguments
            // 
            this.txbBlackDefaultExecArguments.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TheChessBoard.Properties.Settings.Default, "BlackDefaultExecArguments", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txbBlackDefaultExecArguments.Location = new System.Drawing.Point(292, 280);
            this.txbBlackDefaultExecArguments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbBlackDefaultExecArguments.Name = "txbBlackDefaultExecArguments";
            this.txbBlackDefaultExecArguments.Size = new System.Drawing.Size(448, 27);
            this.txbBlackDefaultExecArguments.TabIndex = 14;
            this.txbBlackDefaultExecArguments.Text = global::TheChessBoard.Properties.Settings.Default.BlackDefaultExecArguments;
            // 
            // btnBrowse3
            // 
            this.btnBrowse3.Location = new System.Drawing.Point(748, 235);
            this.btnBrowse3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowse3.Name = "btnBrowse3";
            this.btnBrowse3.Size = new System.Drawing.Size(84, 31);
            this.btnBrowse3.TabIndex = 13;
            this.btnBrowse3.Text = "浏览...";
            this.btnBrowse3.UseVisualStyleBackColor = true;
            this.btnBrowse3.Click += new System.EventHandler(this.btnBrowse3_Click);
            // 
            // txbBlackDefaultExecPath
            // 
            this.txbBlackDefaultExecPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TheChessBoard.Properties.Settings.Default, "BlackDefaultExecPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txbBlackDefaultExecPath.Location = new System.Drawing.Point(292, 235);
            this.txbBlackDefaultExecPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbBlackDefaultExecPath.Name = "txbBlackDefaultExecPath";
            this.txbBlackDefaultExecPath.Size = new System.Drawing.Size(448, 27);
            this.txbBlackDefaultExecPath.TabIndex = 12;
            this.txbBlackDefaultExecPath.Text = global::TheChessBoard.Properties.Settings.Default.BlackDefaultExecPath;
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.Location = new System.Drawing.Point(748, 192);
            this.btnBrowse2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(84, 31);
            this.btnBrowse2.TabIndex = 11;
            this.btnBrowse2.Text = "插入...";
            this.btnBrowse2.UseVisualStyleBackColor = true;
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // txbWhiteDefaultExecArguments
            // 
            this.txbWhiteDefaultExecArguments.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TheChessBoard.Properties.Settings.Default, "WhiteDefaultExecArguments", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txbWhiteDefaultExecArguments.Location = new System.Drawing.Point(292, 192);
            this.txbWhiteDefaultExecArguments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbWhiteDefaultExecArguments.Name = "txbWhiteDefaultExecArguments";
            this.txbWhiteDefaultExecArguments.Size = new System.Drawing.Size(448, 27);
            this.txbWhiteDefaultExecArguments.TabIndex = 10;
            this.txbWhiteDefaultExecArguments.Text = global::TheChessBoard.Properties.Settings.Default.WhiteDefaultExecArguments;
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.Location = new System.Drawing.Point(748, 151);
            this.btnBrowse1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(84, 31);
            this.btnBrowse1.TabIndex = 9;
            this.btnBrowse1.Text = "浏览...";
            this.btnBrowse1.UseVisualStyleBackColor = true;
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse1_Click);
            // 
            // txbWhiteDefaultExecPath
            // 
            this.txbWhiteDefaultExecPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TheChessBoard.Properties.Settings.Default, "WhiteDefaultExecPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txbWhiteDefaultExecPath.Location = new System.Drawing.Point(292, 151);
            this.txbWhiteDefaultExecPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbWhiteDefaultExecPath.Name = "txbWhiteDefaultExecPath";
            this.txbWhiteDefaultExecPath.Size = new System.Drawing.Size(448, 27);
            this.txbWhiteDefaultExecPath.TabIndex = 8;
            this.txbWhiteDefaultExecPath.Text = global::TheChessBoard.Properties.Settings.Default.WhiteDefaultExecPath;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 280);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "黑 AI 默认参数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 239);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "黑 AI 默认可执行文件路径";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "白 AI 默认参数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "白 AI 默认可执行文件路径";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(418, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "毫秒";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "自动走棋时观棋延时";
            // 
            // ckbAutoSaveAIConfig
            // 
            this.ckbAutoSaveAIConfig.AutoSize = true;
            this.ckbAutoSaveAIConfig.Checked = global::TheChessBoard.Properties.Settings.Default.AutoSaveAIConfig;
            this.ckbAutoSaveAIConfig.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbAutoSaveAIConfig.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TheChessBoard.Properties.Settings.Default, "AutoSaveAIConfig", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ckbAutoSaveAIConfig.Location = new System.Drawing.Point(30, 33);
            this.ckbAutoSaveAIConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckbAutoSaveAIConfig.Name = "ckbAutoSaveAIConfig";
            this.ckbAutoSaveAIConfig.Size = new System.Drawing.Size(204, 24);
            this.ckbAutoSaveAIConfig.TabIndex = 0;
            this.ckbAutoSaveAIConfig.Text = "载入时自动保存 AI 的配置";
            this.ckbAutoSaveAIConfig.UseVisualStyleBackColor = true;
            // 
            // btnSaveAndExit
            // 
            this.btnSaveAndExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSaveAndExit.Location = new System.Drawing.Point(725, 362);
            this.btnSaveAndExit.Name = "btnSaveAndExit";
            this.btnSaveAndExit.Size = new System.Drawing.Size(115, 36);
            this.btnSaveAndExit.TabIndex = 1;
            this.btnSaveAndExit.Text = "保存并返回(&S)";
            this.btnSaveAndExit.UseVisualStyleBackColor = true;
            this.btnSaveAndExit.Click += new System.EventHandler(this.btnSaveAndExit_Click);
            // 
            // SettingsBox
            // 
            this.AcceptButton = this.btnSaveAndExit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 416);
            this.Controls.Add(this.btnSaveAndExit);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsBox";
            this.ShowIcon = false;
            this.Text = "设定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsBox_FormClosing);
            this.Load += new System.EventHandler(this.SettingsBox_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWatchChessTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.CheckBox ckbAutoSaveAIConfig;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse4;
        private System.Windows.Forms.TextBox txbBlackDefaultExecArguments;
        private System.Windows.Forms.Button btnBrowse3;
        private System.Windows.Forms.TextBox txbBlackDefaultExecPath;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.TextBox txbWhiteDefaultExecArguments;
        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.TextBox txbWhiteDefaultExecPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSaveAndExit;
        private System.Windows.Forms.NumericUpDown nudWatchChessTime;
        private System.Windows.Forms.CheckBox ckbHideAIWindow;
    }
}