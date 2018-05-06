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
    

    public partial class PrimitiveBoard : Form
    {
        private int ButtonSquareWidth = 69;
        private int ButtonSquareHeight = 67;
        private Color ButtonSquareColor = Control.DefaultBackColor;
        private Color ButtonSquareDownColor = SystemColors.ControlDark;
        private Color ButtonSquareSelectedColor = SystemColors.ControlDark;
        private Color ButtonSquareAvailableColor = SystemColors.Info;
        private Color ButtonSquareCheckedColor = Color.LightCoral;

        StdIOGame FormGame;
        ChessDotNet.Player currentPlayer;
        Position SelectedPosition;
        Dictionary<Button, List<MoreDetailedMove>> DestinationMoves;

        Piece pieceJustCaptured;

        public PrimitiveBoard(StdIOGame formGame)
        {
            FormGame = formGame;
            InitializeComponent();
            InitializeCustomComponent();
        }

        private Button[] btnBoardSquares;

        private void InitializeCustomComponent()
        {
            this.pnlBoard.SuspendLayout();
            this.SuspendLayout();
            pnlBoard.Controls.Clear();
            btnBoardSquares = new Button[64];

            for (int r = 0; r < 8; r++)
            {
                for(int f = 0; f < 8; f++)
                {
                    btnBoardSquares[r * 8 + f] = new Button();
                    var bs = btnBoardSquares[r * 8 + f];

                    pnlBoard.Controls.Add(bs);
                    bs.Location = new Point(f * ButtonSquareWidth, r * ButtonSquareHeight);
                    bs.Name = "btnBoardSquares" + (r * 8 + f).ToString();
                    bs.Size = new System.Drawing.Size(ButtonSquareWidth, ButtonSquareHeight);
                    bs.TabIndex = 20;
                    bs.TabStop = false;
                    bs.Text = "+";
                    bs.Font = new Font("Chess Leipzig", 36F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(2)));

                    bs.Margin = new Padding(0,0,0,0);
                    bs.Padding = new Padding(0,0,0,0);
                    bs.UseVisualStyleBackColor = true;
                    bs.FlatStyle = FlatStyle.Flat;
                    bs.FlatAppearance.BorderSize = 0;
                    bs.FlatAppearance.MouseDownBackColor = SystemColors.ControlDark;
                    bs.Click += SquareClickCurried(r, f);
                    bs.MouseUp += SquareRightMouseButtonCurried(r, f);
                }
            }
            
            this.pnlBoard.ResumeLayout(false);
            this.pnlBoard.PerformLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void PrimitiveBoard_Load(object sender, EventArgs e)
        {
            FormGame.PropertyChanged += FormGamePropertyChangedSubscriber_UpdateUI;
            FormGamePropertyChangedSubscriber_UpdateUI(null, null);
            SANPlayerChanged(null, null);
            DestinationMoves = new Dictionary<Button, List<MoreDetailedMove>>();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                FormGame.ParseAndApplyMove(txbMoveStr.Text, currentPlayer, out pieceJustCaptured);
            } catch (PgnException exception)
            {
                MessageBox.Show("SAN 出现解析错误 : "+ exception.Message, "SAN 输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("SAN 出现解析错误 : " + exception.Message, "SAN 输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
            SquareCancelSelect();
        }

        private void SANPlayerChanged(object sender, EventArgs e)
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

        private void ckbDontCareWhoseTurnItIs_CheckedChanged(object sender, EventArgs e)
        {
            FormGame.CareWhoseTurnItIs = !ckbDontCareWhoseTurnItIs.Checked;
            if(FormGame.CareWhoseTurnItIs)
                DelimitSANPlayerIfNeeded();
            else
            {
                rdbBlack.Enabled = true;
                rdbWhite.Enabled = true;
            }
            SquareCancelSelect();
        }

        private void DelimitSANPlayerIfNeeded()
        {
            if(FormGame.CareWhoseTurnItIs)
            {
                if(FormGame.WhoseTurn == Player.Black)
                {
                    rdbBlack.Enabled = true;
                    rdbBlack.Checked = true;
                    rdbWhite.Enabled = false;
                    rdbWhite.Checked = false;
                }
                else
                {
                    rdbBlack.Enabled = !true;
                    rdbBlack.Checked = !true;
                    rdbWhite.Enabled = !false;
                    rdbWhite.Checked = !false;
                }
            }
        }

        void FormGamePropertyChangedSubscriber_UpdateUI(object sender, PropertyChangedEventArgs e)
        {
            
            if (e == null || e.PropertyName == "BoardPrint")
            {
                UpdateBoardButtons();
            }
            if (e == null || e.PropertyName == "WhoseTurn")
            {
                UpdateWhoseTurn();
            }
        }

        public void UpdateBoardButtons()
        {
            var bp = FormGame.BoardPrint;
            for (var i = 0; i < bp.Length; i++)
            {
                btnBoardSquares[i].Text = bp[i].ToString();
            }
        }

        public void UpdateWhoseTurn()
        {
            var wt = FormGame.WhoseTurn;
            lblCurrentPlayer.Text = "当前执棋：" + (wt == Player.White ? "白" : "黑");
            DelimitSANPlayerIfNeeded();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            txbMoveStr.Focus();
        }

        private EventHandler SquareClickCurried(int Row, int File)
        {
            return (sender, e) => { SquareClick(8 -Row, (File)File); };
        }

        private MouseEventHandler SquareRightMouseButtonCurried(int Row, int File)
        {
            return (sender, e) => {
                if(e.Button.Equals(MouseButtons.Right))
                    SquareRightMouseButton(8 - Row, (File)File);
            };
        }

        void SquareRightMouseButton(int currRank, File currFile)
        {
            bool didSomething = SquareClick(currRank, currFile, false, false);
            if(didSomething && FormGame.StdIOGameStatus == StdIOGameStatus.Selected)
            {
                if (DestinationMoves.Count == 0)
                    return;
                if (DestinationMoves.Count == 1)
                    return;
                var DestinationMovesCopy = new Dictionary<Button, List<MoreDetailedMove>>(DestinationMoves);
                var aggregateResult = DestinationMovesCopy.Aggregate((a, b) => { return new KeyValuePair<Button, List<MoreDetailedMove>>(a.Key, a.Value.Union(b.Value).ToList()); });

                MoveDisambiguationDialog disaDialog = new MoveDisambiguationDialog(aggregateResult.Value);
                disaDialog.CallbackEvent += DisambiguationDone;
                disaDialog.ShowDialog();
            }
        }

        void DisambiguationDone(MoreDetailedMove move)
        {
            FormGame.ApplyMove(move, true, out pieceJustCaptured);
            SquareCancelSelect();
        }

        void SquareCancelSelect()
        {
            FormGame.StdIOGameStatus = StdIOGameStatus.Idle;
            CleanSquareColor();
        }

        bool SquareClick(int currRank, File currFile, bool allowCancelSelect = true, bool allowMoveSelect = true)
        {
            var pos = new Position(currFile, currRank);
            var clickedButton = btnBoardSquares[8 * (8 - currRank) + (int)currFile];

            if (FormGame.StdIOGameStatus == StdIOGameStatus.Selected)
            {
                bool cancelSelect = pos.Equals(SelectedPosition) && allowCancelSelect;
                bool moveSelect = DestinationMoves.ContainsKey(clickedButton) && allowMoveSelect;
                if (cancelSelect)
                {
                    SquareCancelSelect();
                }

                if (moveSelect)
                {
                    var moves = DestinationMoves[clickedButton];
                    if (moves.Count == 1)
                    {
                        DisambiguationDone(DestinationMoves[clickedButton][0]);
                    }
                    else
                    {
                        MoveDisambiguationDialog disaDialog = new MoveDisambiguationDialog(moves);
                        disaDialog.CallbackEvent += DisambiguationDone;
                        disaDialog.ShowDialog();
                    }
                }

                if (cancelSelect || moveSelect)
                {
                    return true;
                }
            }

            if (FormGame.StdIOGameStatus == StdIOGameStatus.Idle || FormGame.StdIOGameStatus == StdIOGameStatus.Selected)
            {
                var validMoves = FormGame.Game.GetValidMoves(pos, false);
                var piece = FormGame.Game.GetPieceAt(pos);
                if (piece != null)
                {
                    CleanSquareColor();
                    SelectedPosition = pos;
                    FormGame.StdIOGameStatus = StdIOGameStatus.Selected;

                    DestinationMoves.Clear();

                    foreach (var move in validMoves)
                    {
                        var currButton = btnBoardSquares[(8 - move.NewPosition.Rank) * 8 + (int)move.NewPosition.File];
                        currButton.BackColor = ButtonSquareAvailableColor;
                        if(DestinationMoves.ContainsKey(currButton))
                        {
                            DestinationMoves[currButton].Add(move);
                        }
                        else
                            DestinationMoves.Add(currButton, new List<MoreDetailedMove> { move });
                    }
                    clickedButton.BackColor = ButtonSquareSelectedColor;
                    return true;
                }
                else
                {
                    SquareCancelSelect();
                }
            }
            return false;
        }

        void CleanSquareColor()
        {
            foreach (var bs in btnBoardSquares)
                bs.BackColor = ButtonSquareColor;
        }

        
    }
}
