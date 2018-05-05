using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheChessBoard
{
    static class TestStart
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new ReadStdIOTestForm());
        }
    }
}
