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
        static StdIOGame FormGame = new StdIOGame("", "", "", "");
        static PrimitiveBoard boardForm;
        static void Main()
        {
            Application.EnableVisualStyles();
            boardForm = new PrimitiveBoard(FormGame);
            Application.Run(boardForm);
        }
    }
}
