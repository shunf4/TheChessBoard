using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessDotNet;

namespace TheChessBoard
{
    public delegate void InputExecCommandCallbackHandler(Player player, string execPath, string execArguments);
    public partial class InputExecCommandDialog : Form
    {
        public event InputExecCommandCallbackHandler CallbackEvent;
        Player CurrPlayer;
        public InputExecCommandDialog(Player currPlayer)
        {
            InitializeComponent();
            CurrPlayer = currPlayer;

            String formCaption = "设置{0}方AI";
            String execPathText = "{0}方AI的可执行文件路径";
            String argumentsText = "{0}方的运行参数";

            String hand;

            if (currPlayer == Player.White)
            {
                hand = "白";
            }
            else
            {
                hand = "黑";
            }

            this.Text = string.Format(formCaption, hand);
            lblExecPathCaption.Text = string.Format(execPathText, hand);
            lblExecArgumentsCaption.Text = string.Format(argumentsText, hand);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.OK;
            CallbackEvent?.Invoke(CurrPlayer, txbExecPath.Text, txbExecArguments.Text);
        }

        private void txbFileDialogExec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
            dialog.Title = "选择可执行文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txbExecPath.Text = dialog.FileName;
            }
        }

        private void txbFileDialogExecArg_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "所有文件 (*.*)|*.*";
            dialog.Title = "选择一个文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txbExecArguments.SelectedText = '"' + dialog.FileName + '"';
            }
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AIExecPathHistory.Clear();
            Properties.Settings.Default.AIExecArgumentsHistory.Clear();
            MessageBox.Show("清除成功。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
