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
    public partial class PrimitiveBoard : Form
    {
        public PrimitiveBoard()
        {
            InitializeComponent();
        }

        private void PrimitiveBoard_Load(object sender, EventArgs e)
        {
            StdIOGame game = new StdIOGame("", "", "", "");
            txbBoard.DataBindings.Add("Text", game, "BoardString", false, DataSourceUpdateMode.OnPropertyChanged);
            game.ParseAndApplyMove("Nc3", ChessDotNet.Player.White);
        }
    }
}
