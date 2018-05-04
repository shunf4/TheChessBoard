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
        StdIOGame game;
        ChessDotNet.Player currentPlayer;
        public PrimitiveBoard()
        {
            InitializeComponent();
        }

        private void PrimitiveBoard_Load(object sender, EventArgs e)
        {
            game = new StdIOGame("", "", "", "");
            txbBoard.DataBindings.Add("Text", game, "BoardString", false, DataSourceUpdateMode.OnPropertyChanged);
            playerCheckedChanged(null, null);
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            game.ParseAndApplyMove(txbMoveStr.Text, currentPlayer);
        }

        private void playerCheckedChanged(object sender, EventArgs e)
        {
            if(rdbBlack.Checked)
            {
                currentPlayer = ChessDotNet.Player.Black;
            }
            if (rdbWhite.Checked)
            {
                currentPlayer = ChessDotNet.Player.White;
            }
        }
    }
}
