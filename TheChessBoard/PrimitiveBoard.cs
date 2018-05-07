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

        public PrimitiveBoard(StdIOGame formGame, out System.Threading.SynchronizationContext context)
        {
            InitializeComponent();
            InitializeCustomComponent();
            GameLoad(formGame);
            context = System.Threading.SynchronizationContext.Current;
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
            GameStart();
        }

        private void GameLoad(StdIOGame formGame)
        {
            FormGame = formGame;
            FormGame.PropertyChanged -= FormGamePropertyChangedSubscriber_UpdateUI;
            FormGame.PropertyChanged += FormGamePropertyChangedSubscriber_UpdateUI;
            FormGame.GameProcedureStatusUpdated -= FormGameProcedureStatusUpdatedSubscriber;
            FormGame.GameProcedureStatusUpdated += FormGameProcedureStatusUpdatedSubscriber;
            FormGame.GameControlStatusUpdated -= FormGameControlStatusUpdatedSubscriber;
            FormGame.GameControlStatusUpdated += FormGameControlStatusUpdatedSubscriber;
            FormGame.AppliedMove -= AfterApplyMove;
            FormGame.AppliedMove += AfterApplyMove;

            lblWhiteWatch.DataBindings.Clear();
            lblWhiteWatch.DataBindings.Add("Text", FormGame, "plyWhiteStopwatchTime", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        void BusifyCursor(){ this.Cursor = Cursors.AppStarting; }
        void RestoreCursor(){ this.Cursor = Cursors.Default; }

        private void AfterApplyMove()
        {
            FormGame.ControlStatus = StdIOGameControlState.Idle;
            SquareCancelSelect();
        }

        private void GameStart()
        {
            FormGamePropertyChangedSubscriber_UpdateUI(null, null);
            FormGameProcedureStatusUpdatedSubscriber("");
            FormGameControlStatusUpdatedSubscriber();
            SANPlayerChanged(null, null);
            DestinationMoves = new Dictionary<Button, List<MoreDetailedMove>>();
        }

        private void FormGameProcedureStatusUpdatedSubscriber(string reason)
        {
            if (FormGame.ProcedureStatus == StdIOGameProcedureState.Running)
            {
                btnMove.Enabled = true;
            }
            else
            {
                btnMove.Enabled = false;
                MessageBox.Show(reason, "游戏结束", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void FormGameControlStatusUpdatedSubscriber()
        {
            if (FormGame.ControlStatus == StdIOGameControlState.Idle || FormGame.ControlStatus == StdIOGameControlState.Selected)
            {
                if(this.Enabled == false)
                    btnMove.Enabled = true;
                RestoreCursor();
            }
            else if (FormGame.ControlStatus == StdIOGameControlState.StdIORunning)
            {
                btnMove.Enabled = false;
                BusifyCursor();
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                FormGame.ParseAndApplyMove(txbMoveStr.Text, currentPlayer, out pieceJustCaptured);
                txbMoveStr.Clear();
            } catch (PgnException exception)
            {
                MessageBox.Show("SAN 出现解析错误 : "+ exception.Message, "SAN 输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("SAN 出现解析错误 : " + exception.Message, "SAN 输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
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
                this.Invoke(new Action(()=> { UpdateBoardButtons(); }));
            }
            if (e == null || e.PropertyName == "WhoseTurn")
            {
                this.Invoke(new Action(() => { UpdateWhoseTurn(); }));
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
            if(didSomething && FormGame.ControlStatus == StdIOGameControlState.Selected)
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
        }

        void SquareCancelSelect()
        {
            if(FormGame.ControlStatus == StdIOGameControlState.Selected)
                FormGame.ControlStatus = StdIOGameControlState.Idle;
            CleanSquareColor();
        }

        bool SquareClick(int currRank, File currFile, bool allowCancelSelect = true, bool allowMoveSelect = true)
        {
            if (FormGame.ProcedureStatus != StdIOGameProcedureState.Running)
                return false;

            var pos = new Position(currFile, currRank);
            var clickedButton = btnBoardSquares[8 * (8 - currRank) + (int)currFile];

            if (FormGame.ControlStatus == StdIOGameControlState.Selected)
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

            if (FormGame.ControlStatus == StdIOGameControlState.Idle || FormGame.ControlStatus == StdIOGameControlState.Selected)
            {
                var validMoves = FormGame.Game.GetValidMoves(pos, false);
                var piece = FormGame.Game.GetPieceAt(pos);
                if (piece != null)
                {
                    CleanSquareColor();
                    SelectedPosition = pos;
                    FormGame.ControlStatus = StdIOGameControlState.Selected;

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

        private void btnWhiteProcStart_Click(object sender, EventArgs e)
        {
            FormGame.ProcessWhiteStart();
        }

        private void btnWhiteReadLine_Click(object sender, EventArgs e)
        {
            FormGame.ProcessWhiteAllowOutputAndWait();
        }

        private void lblFormStatusCaption_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
