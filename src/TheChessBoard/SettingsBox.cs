using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheChessBoard
{
    public partial class SettingsBox : Form
    {
        public SettingsBox()
        {
            InitializeComponent();
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
            dialog.Title = "选择可执行文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txbWhiteDefaultExecPath.Text = dialog.FileName;
            }
        }

        private void btnBrowse3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
            dialog.Title = "选择可执行文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txbBlackDefaultExecPath.Text = dialog.FileName;
            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "所有文件 (*.*)|*.*";
            dialog.Title = "选择一个文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txbWhiteDefaultExecArguments.SelectedText = '"' + dialog.FileName + '"';
            }
        }

        private void btnBrowse4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "所有文件 (*.*)|*.*";
            dialog.Title = "选择一个文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txbBlackDefaultExecArguments.SelectedText = '"' + dialog.FileName + '"';
            }
        }

        private void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.WatchChessTime = decimal.ToUInt32(nudWatchChessTime.Value);
            Properties.Settings.Default.Save();
        }

        private void SettingsBox_Load(object sender, EventArgs e)
        {
            nudWatchChessTime.Value = Properties.Settings.Default.WatchChessTime;
        }

        private void SettingsBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Reload();
        }
    }
}
