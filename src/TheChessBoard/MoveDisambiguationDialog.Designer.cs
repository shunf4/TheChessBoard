namespace TheChessBoard
{
    partial class MoveDisambiguationDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.dgvMoves = new System.Windows.Forms.DataGridView();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMoves)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(324, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "您的选择动作对应多种落子方式。请选择一种：";
            // 
            // dgvMoves
            // 
            this.dgvMoves.AllowUserToAddRows = false;
            this.dgvMoves.AllowUserToDeleteRows = false;
            this.dgvMoves.AllowUserToOrderColumns = true;
            this.dgvMoves.AllowUserToResizeRows = false;
            this.dgvMoves.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvMoves.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMoves.Location = new System.Drawing.Point(45, 75);
            this.dgvMoves.Name = "dgvMoves";
            this.dgvMoves.ReadOnly = true;
            this.dgvMoves.RowHeadersWidth = 25;
            this.dgvMoves.RowTemplate.Height = 27;
            this.dgvMoves.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMoves.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMoves.Size = new System.Drawing.Size(386, 264);
            this.dgvMoves.TabIndex = 3;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(229, 362);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(89, 32);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "确定(&O)";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(341, 362);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 32);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MoveDisambiguationDialog
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(479, 416);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dgvMoves);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MoveDisambiguationDialog";
            this.Text = "请选择一个落子";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMoves)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvMoves;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
    }
}