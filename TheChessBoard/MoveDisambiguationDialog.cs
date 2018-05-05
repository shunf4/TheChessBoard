using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using ChessDotNet;

namespace TheChessBoard
{
    public delegate void DisambiguationCallbackHandler(DetailedMove detailedMove);

    public partial class MoveDisambiguationDialog : Form
    {
        public event DisambiguationCallbackHandler CallbackEvent;
        List<DetailedMove> Moves;
        public MoveDisambiguationDialog(List<DetailedMove> moves)
        {
            InitializeComponent();
            Moves = moves;
            //movesBindingSource = new BindingSource();
            dgvMoves.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "文字描述",
                Width = 180,
                ReadOnly = true,
                Name = "FriendlyText",
                DataPropertyName = "FriendlyText"
            });
            dgvMoves.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SAN 字串",
                Width = 170,
                ReadOnly = true,
                Name = "SANString",
                DataPropertyName = "SANString"
            });
            dgvMoves.AutoGenerateColumns = false;
            dgvMoves.DataSource = Moves;
            dgvMoves.Select();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(CallbackEvent != null)
            {
                CallbackEvent.Invoke(Moves[dgvMoves.CurrentRow.Index]);
            }
            this.Close();
        }
    }
}
