using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheChessBoard
{
    static class PrimitiveBoardStart
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new PrimitiveBoard());
        }
    }
}
