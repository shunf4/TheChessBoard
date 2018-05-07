using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace TheChessBoard
{
    static class PrimitiveBoardStart
    {
        static PrimitiveBoard boardForm;
        static void Main()
        {
            StdIOGame FormGame = new StdIOGame(@"..\..\..\TestConsoleApp1\bin\Debug\TestConsoleApp1.exe", "", "", "");

            Application.EnableVisualStyles();
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
            boardForm = new PrimitiveBoard(FormGame, out System.Threading.SynchronizationContext context);

            FormGame.LoadSynchronizationContext(context);
            Application.Run(boardForm);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
