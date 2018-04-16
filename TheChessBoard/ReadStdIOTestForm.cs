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
        Process proc1;
        Process proc2;

        public ReadStdIOTestForm()
        {
            InitializeComponent();
        }

        private void ReadStdIOTestForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            proc1 = new Process();
            proc2 = new Process();
            proc1.StartInfo.FileName = @"..\..\..\TestConsoleApp1\bin\Debug\TestConsoleApp1.exe";
            proc1.StartInfo.UseShellExecute = false;
            proc1.StartInfo.RedirectStandardInput = true;
            proc1.StartInfo.RedirectStandardOutput = true;
            proc1.StartInfo.CreateNoWindow = true;
            proc1.Start();

            proc2.StartInfo.FileName = @"..\..\..\TestConsoleApp2\bin\Debug\TestConsoleApp2.exe";
            proc2.StartInfo.UseShellExecute = false;
            proc2.StartInfo.RedirectStandardInput = true;
            proc2.StartInfo.RedirectStandardOutput = true;
            proc2.StartInfo.CreateNoWindow = true;
            proc2.Start();

            if (proc1 != null && proc2 != null)
            {
                proc2.StandardInput.WriteLine(txbInput2.Text);
                txbOut1.Text = proc1.StandardOutput.ReadToEnd();
                txbOut2.Text = proc2.StandardOutput.ReadToEnd();
            }

            proc1.WaitForExit();
            proc2.WaitForExit();
        }
    }
}
