using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TheChessBoard
{
    public partial class ReadStdIOTestForm : Form
    {
        ReadWriteStdIO rwstd = new ReadWriteStdIO();

        public ReadStdIOTestForm()
        {
            InitializeComponent();
        }

        private void ReadStdIOTestForm_Load(object sender, EventArgs e)
        {
            rwstd.Init(@"..\..\..\TestConsoleApp1\bin\Debug\TestConsoleApp1.exe", "", @"..\..\..\TestConsoleApp2\bin\Debug\TestConsoleApp2.exe", "");
            txbOut1.DataBindings.Add("Text", rwstd, "outputStr1");
            txbOut2.DataBindings.Add("Text", rwstd, "outputStr2");
            btnStart.DataBindings.Add("Enabled", rwstd, "procNotStarted");
            btnStop.DataBindings.Add("Enabled", rwstd, "procStarted");
            btnConfirm.DataBindings.Add("Enabled", rwstd, "procStarted");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            rwstd.WriteToProc2(txbInput2.Text);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            rwstd.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            rwstd.Stop();
        }
    }
}
