using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TheChessBoard
{
    static class TheChessBoardStart
    {
        static TheChessBoard boardForm;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();

            boardForm = new TheChessBoard();
            Application.Run(boardForm);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
