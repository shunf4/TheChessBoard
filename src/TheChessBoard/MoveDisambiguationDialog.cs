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
    public delegate void DisambiguationCallbackHandler(MoreDetailedMove detailedMove);

    public partial class MoveDisambiguationDialog : Form
    {
        public event DisambiguationCallbackHandler CallbackEvent;

        List<MoreDetailedMove> Moves;
        
        public MoveDisambiguationDialog(List<MoreDetailedMove> moves)
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
            dgvMoves.MultiSelect = false;
            dgvMoves.CellMouseDoubleClick += (sender, e) => { this.BeginInvoke(new EventHandler(btnConfirm_Click)); };
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
            CallbackEvent?.Invoke(Moves[dgvMoves.CurrentRow.Index]);
        }
    }
}
