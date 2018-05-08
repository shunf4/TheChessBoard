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
using System.Diagnostics;
using System.Threading;

namespace TheChessBoard
{

    public partial class TheChessBoard : Form
    {
        #region 一些默认值（颜色、宽高）的配置
        private int ButtonSquareWidth = 69;
        private int ButtonSquareHeight = 67;
        private Color ButtonSquareColor = Control.DefaultBackColor;
        private Color ButtonSquareDownColor = SystemColors.ControlDark;
        private Color ButtonSquareSelectedColor = SystemColors.ControlDark;
        private Color ButtonSquareAvailableColor = SystemColors.Info;
        private Color ButtonSquareCheckedColor = Color.LightCoral;

        private Color ColorRunning = Color.FromArgb(0, 192, 0);
        private Color ColorNotStarted = Color.Gray;
        private Color ColorBlackWins = Color.Black;
        private Color ColorWhiteWins = Color.White;
        private Color ColorDraw = Color.Blue;
        #endregion

        #region 控件相关
        private Button[] btnBoardSquares;
        #endregion

        ChessBoardGame FormGame;
        ChessDotNet.Player currentPlayer;
        Position SelectedPosition;
        Dictionary<Button, List<MoreDetailedMove>> DestinationMoves;

        public static string FormName = "The Chess Board";
        public static string FormVersion = "0.0.1.0";

        Piece pieceJustCaptured;

        public TheChessBoard(ChessBoardGame formGame)
        {
            InitializeComponent();
            InitializeCustomComponent();
            LoadChessBoardGame(formGame);
        }

        public TheChessBoard() : this(new ChessBoardGame()) { }

        private void LoadChessBoardGame(ChessBoardGame formGame)
        {
            FormGame = formGame;
            FormGame.PropertyChanged -= FormGamePropertyChangedSubscriber_UpdateUI;
            FormGame.PropertyChanged += FormGamePropertyChangedSubscriber_UpdateUI;
            FormGame.GameProcedureStatusUpdated -= FormGameProcedureStatusUpdatedSubscriber;
            FormGame.GameProcedureStatusUpdated += FormGameProcedureStatusUpdatedSubscriber;
            FormGame.GameControlStatusUpdated -= FormGameControlStatusUpdatedSubscriber;
            FormGame.GameControlStatusUpdated += FormGameControlStatusUpdatedSubscriber;
            FormGame.PlayerIOStatusUpdated -= FormGamePlayerIOStatusUpdatedSubscriber;
            FormGame.PlayerIOStatusUpdated += FormGamePlayerIOStatusUpdatedSubscriber;
            FormGame.AppliedMove -= AfterApplyMove;
            FormGame.AppliedMove += AfterApplyMove;

            lblWhiteWatch.DataBindings.Clear();
            lblWhiteWatch.DataBindings.Add("Text", FormGame, "WhiteStopwatchTime", false, DataSourceUpdateMode.OnPropertyChanged);
            lblBlackWatch.DataBindings.Clear();
            lblBlackWatch.DataBindings.Add("Text", FormGame, "BlackStopwatchTime", false, DataSourceUpdateMode.OnPropertyChanged);

            var context = System.Threading.SynchronizationContext.Current;
            FormGame.LoadSynchronizationContext(context);
        }

        #region 和控件有关的方法
        #region 和控件初始化有关的方法

        private void InitializeCustomComponent()
        {
            this.SuspendLayout();
            InitializeButtonSquares();

            

            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ResumeLayout(false);
            this.PerformLayout();

            // Log
            SetLog(string.Format(@"{{\rtf1\ansicpg936 \b {0} {1}\b0 \line 窗体控件设置完成\line}}", FormName, FormVersion));
        }

        private void InitializeButtonSquares()
        {
            this.pnlBoard.SuspendLayout();

            pnlBoard.Controls.Clear();
            btnBoardSquares = new Button[64];

            var unifiedSize = new System.Drawing.Size(ButtonSquareWidth, ButtonSquareHeight);

            for (int r = 0; r < 8; r++)
            {
                for (int f = 0; f < 8; f++)
                {
                    btnBoardSquares[r * 8 + f] = new Button();
                    var bs = btnBoardSquares[r * 8 + f];

                    bs.Location = new Point(f * ButtonSquareWidth, r * ButtonSquareHeight);
                    bs.Name = "btnBoardSquares" + (r * 8 + f).ToString();
                    bs.Size = unifiedSize;
                    bs.TabIndex = 20 + r * 8 + f;
                    bs.TabStop = false;
                    bs.Text = "+";
                    bs.Font = new Font("Chess Leipzig", 36F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(2)));

                    bs.Margin = new Padding(0, 0, 0, 0);
                    bs.Padding = new Padding(0, 0, 0, 0);
                    bs.FlatStyle = FlatStyle.Flat;
                    bs.FlatAppearance.BorderSize = 0;
                    bs.FlatAppearance.MouseDownBackColor = SystemColors.ControlDark;
                    bs.Click += SquareClickCurried(r, f);
                    bs.MouseUp += SquareRightMouseButtonCurried(r, f);

                    pnlBoard.Controls.Add(bs);
                }
            }
            this.pnlBoard.ResumeLayout(false);
            this.pnlBoard.PerformLayout();
            
        }

        private void PrimitiveBoard_Load(object sender, EventArgs e)
        {
            FormGame.InvokeAllUpdates();
        }

        #endregion

        void BusifyCursor() { this.Cursor = Cursors.AppStarting; }
        void RestoreCursor() { this.Cursor = Cursors.Default; }

        public void AppendLog(String logRTF)
        {
            rtbLog.Select(rtbLog.TextLength, 0);
            rtbLog.SelectedRtf = logRTF;
        }

        public void SetLog(String logRTF)
        {
            rtbLog.Select(0, rtbLog.TextLength);
            rtbLog.SelectedRtf = logRTF;
        }

        private void DelimitSANPlayerIfNeeded()
        {
            if (FormGame.CareWhoseTurnItIs)
            {
                if (FormGame.WhoseTurn == Player.Black)
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

        #endregion

        #region 与控件触发事件有关的方法
        private void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                FormGame.ParseAndApplyMove(txbMoveStr.Text, currentPlayer, out pieceJustCaptured);
                txbMoveStr.Clear();
            }
            catch (PgnException exception)
            {
                MessageBox.Show("SAN 出现解析错误 : " + exception.Message, "SAN 输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("SAN 出现解析错误 : " + exception.Message, "SAN 输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        }

        private void SANPlayerChanged(object sender, EventArgs e)
        {
            if (rdbBlack.Checked)
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
            if (FormGame.CareWhoseTurnItIs)
                DelimitSANPlayerIfNeeded();
            else
            {
                rdbBlack.Enabled = true;
                rdbWhite.Enabled = true;
            }
            FormGame.ControlStatus = ChessBoardGameControlState.Idle;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            txbMoveStr.Focus();
        }

        private EventHandler SquareClickCurried(int Row, int File)
        {
            return (sender, e) => { SquareClick(8 - Row, (File)File); };
        }

        private MouseEventHandler SquareRightMouseButtonCurried(int Row, int File)
        {
            return (sender, e) => {
                if (e.Button.Equals(MouseButtons.Right))
                    SquareRightMouseButton(8 - Row, (File)File);
            };
        }

        void SquareRightMouseButton(int currRank, File currFile)
        {
            bool didSomething = SquareClick(currRank, currFile, false, false);
            if (didSomething && FormGame.ControlStatus == ChessBoardGameControlState.Selected)
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
            CleanSquareColor();
        }

        bool SquareClick(int currRank, File currFile, bool allowCancelSelect = true, bool allowMoveSelect = true)
        {
            if (FormGame.ProcedureStatus != ChessBoardGameProcedureState.Running)
            {
                if(FormGame.ProcedureStatus == ChessBoardGameProcedureState.NotStarted)
                    MessageBox.Show("游戏尚未开始，请选择模式后点击左侧“选择模式并开始”。", "游戏尚未开始", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                    MessageBox.Show("游戏已经结束，请点击“重置”后重新开始。", "游戏已经结束", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                return false;
            }

            var pos = new Position(currFile, currRank);
            var clickedButton = btnBoardSquares[8 * (8 - currRank) + (int)currFile];

            if (FormGame.ControlStatus == ChessBoardGameControlState.Selected)
            {
                bool cancelSelect = pos.Equals(SelectedPosition) && allowCancelSelect;
                bool moveSelect = DestinationMoves.ContainsKey(clickedButton) && allowMoveSelect;
                if (cancelSelect)
                {
                    FormGame.ControlStatus = ChessBoardGameControlState.Idle;
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
                        disaDialog.Dispose();
                    }
                }

                if (cancelSelect || moveSelect)
                {
                    return true;
                }
            }

            if (FormGame.ControlStatus == ChessBoardGameControlState.Idle || FormGame.ControlStatus == ChessBoardGameControlState.Selected)
            {
                var validMoves = FormGame.Game.GetValidMoves(pos, false);
                var piece = FormGame.Game.GetPieceAt(pos);
                if (piece != null)
                {
                    CleanSquareColor();
                    SelectedPosition = pos;
                    FormGame.ControlStatus = ChessBoardGameControlState.Selected;

                    DestinationMoves.Clear();

                    foreach (var move in validMoves)
                    {
                        var currButton = btnBoardSquares[(8 - move.NewPosition.Rank) * 8 + (int)move.NewPosition.File];
                        currButton.BackColor = ButtonSquareAvailableColor;
                        if (DestinationMoves.ContainsKey(currButton))
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
                    FormGame.ControlStatus = ChessBoardGameControlState.Idle;
                }
            }
            return false;
        }

        private void btnWhiteProcStart_Click(object sender, EventArgs e)
        {
            FormGame.ProcessWhiteStart();
        }

        private void btnWhiteReadLine_Click(object sender, EventArgs e)
        {
            FormGame.ProcessWhiteAllowOutputAndWait();
        }

        #endregion

        #region FormGame 引发事件触发的方法
        private void AfterApplyMove()
        {
            FormGame.ControlStatus = ChessBoardGameControlState.Idle;
        }

        private void FormGameProcedureStatusUpdatedSubscriber(StatusUpdatedEventArgs e)
        {
            
            if (FormGame.ProcedureStatus == ChessBoardGameProcedureState.Running)
            {
                if(e.UpdateImportant == true)
                {
                    // New game started
                    DestinationMoves = new Dictionary<Button, List<MoreDetailedMove>>();
                    FormGame.CareWhoseTurnItIs = !(ckbDontCareWhoseTurnItIs.Checked);
                }
                btnModeConfirm.Enabled = false;
                btnMove.Enabled = true;
                pnlBlackMode.Enabled = false;
                pnlWhiteMode.Enabled = false;
                btnModeConfirm.Enabled = false;

                lblFormStatus.ForeColor = ColorRunning;
                lblFormStatusText.Text = "进行中";
            }
            else
            {
                btnModeConfirm.Enabled = true;
                btnMove.Enabled = false;
                pnlBlackMode.Enabled = true;
                pnlWhiteMode.Enabled = true;
                btnModeConfirm.Enabled = true;

                if (FormGame.ProcedureStatus == ChessBoardGameProcedureState.NotStarted)
                {
                    lblFormStatus.ForeColor = ColorNotStarted;
                    lblFormStatusText.Text = "未开始";
                }
                else
                {
                    this.BeginInvoke(new Action(delegate { MessageBox.Show(e.Reason, "游戏结束", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }));
                    switch (FormGame.ProcedureStatus)
                    {
                        case ChessBoardGameProcedureState.BlackWins:
                            lblFormStatus.ForeColor = ColorBlackWins;
                            lblFormStatusText.Text = "黑胜";
                            break;
                        case ChessBoardGameProcedureState.WhiteWins:
                            lblFormStatus.ForeColor = ColorWhiteWins;
                            lblFormStatusText.Text = "白胜";
                            break;
                        case ChessBoardGameProcedureState.Draw:
                            lblFormStatus.ForeColor = ColorDraw;
                            lblFormStatusText.Text = "平局";
                            break;
                    }
                }
            }
        }

        private void FormGameControlStatusUpdatedSubscriber(StatusUpdatedEventArgs e)
        {
            if (FormGame.ControlStatus == ChessBoardGameControlState.Idle || FormGame.ControlStatus == ChessBoardGameControlState.NotStarted || FormGame.ControlStatus == ChessBoardGameControlState.StdIORunning)
            {
                SquareCancelSelect();
            }

            if (FormGame.ControlStatus == ChessBoardGameControlState.Idle || FormGame.ControlStatus == ChessBoardGameControlState.Selected)
            {
                SANPlayerChanged(null, null);
                if (btnMove.Enabled == false)
                    btnMove.Enabled = true;
                RestoreCursor();
            }

            if (FormGame.ControlStatus == ChessBoardGameControlState.StdIORunning)
            {
                btnMove.Enabled = false;
                BusifyCursor();
            }
        }

        private void FormGamePlayerIOStatusUpdatedSubscriber(StatusUpdatedEventArgs e)
        {
            if(FormGame.WhiteStatus == StdIOState.NotLoaded)
            {
                rdbWhiteAuto.Enabled = false;
                rdbWhiteManual.Checked = true;
                lblWhiteStatus.ForeColor = ColorNotStarted;
                lblWhiteStatusText.Text = "未装载";
            }
            else
            {
                rdbWhiteAuto.Enabled = true;
            }

            if (FormGame.BlackStatus == StdIOState.NotLoaded)
            {
                rdbBlackAuto.Enabled = false;
                rdbBlackManual.Checked = true;
                lblBlackStatus.ForeColor = ColorNotStarted;
                lblBlackStatusText.Text = "未装载";
            }
            else
            {
                rdbBlackAuto.Enabled = true;
            }
        }

        private void FormGamePropertyChangedSubscriber_UpdateUI(object sender, PropertyChangedEventArgs e)
        {

            if (e == null || e.PropertyName == "BoardPrint")
            {
                this.Invoke(new Action(() => { UpdateBoardButtons(); }));
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

        void CleanSquareColor()
        {
            foreach (var bs in btnBoardSquares)
                bs.BackColor = ButtonSquareColor;
        }
        #endregion

        private void lblFormStatusCaption_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadWhiteAI_Click(object sender, EventArgs e)
        {

        }

        private void btnModeConfirm_Click(object sender, EventArgs e)
        {
            GameMode mode;
            if(rdbBlackAuto.Checked)
            {
                if (rdbWhiteAuto.Checked)
                    mode = GameMode.BothAuto;
                else
                    mode = GameMode.BlackAuto;
            }
            else
            {
                if (rdbWhiteAuto.Checked)
                    mode = GameMode.WhiteAuto;
                else
                    mode = GameMode.Manual;
            }
            FormGame.Start(mode);
        }

        private void btnAllReset_Click(object sender, EventArgs e)
        {
            FormGame.ResetGame();
        }
    }
}
