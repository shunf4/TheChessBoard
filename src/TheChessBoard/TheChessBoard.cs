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
using System.Runtime.InteropServices;

namespace TheChessBoard
{
    /// <summary>
    /// 主窗体。
    /// </summary>
    public partial class TheChessBoard : Form
    {
        #region 一些默认值（颜色、宽高）的配置
        private int ButtonSquareWidth = 69;
        private int ButtonSquareHeight = 67;
        private Color ButtonSquareColor = Control.DefaultBackColor;
        private Color ButtonSquareDownColor = SystemColors.ControlDark;
        private Color ButtonSquareSelectedColor = SystemColors.ControlDark;
        private Color ButtonSquareAvailableColor = Color.FromArgb(220,255,220);
        private Color ButtonSquareMoveColor = Color.FromArgb(245,245,220);
        private Color ButtonSquareCapturedColor = Color.FromArgb(220,240,255);
        private Color ButtonSquareCheckedColor = Color.FromArgb(255, 220, 220);
        private Color ButtonSquareCheckmatedColor = Color.Red;

        private Color ColorRunning = Color.FromArgb(0, 192, 0);
        private Color ColorGameNotStarted = Color.Gray;
        private Color ColorBlackWins = Color.Black;
        private Color ColorWhiteWins = Color.White;
        private Color ColorDraw = Color.Blue;
        private Color ColorProcNotLoaded = Color.FromArgb(192, 192, 192);
        private Color ColorProcNotStarted = Color.FromArgb(156, 200, 156);
        private Color ColorBusy = Color.FromArgb(255, 0, 0);
        private Color ColorStop = Color.FromArgb(255, 0, 0);
        private Color ColorIdle = Color.FromArgb(0, 192, 0);

        private bool frequentlyRefresh = true;
        #endregion

        /// <summary>
        /// 六十四个按钮，构成棋盘的棋盘格子。
        /// </summary>
        private Button[] btnBoardSquares;

        /// <summary>
        /// ChessBoardGame 对象，实现与窗体相关的游戏进行逻辑。
        /// </summary>
        ChessBoardGameFormLogic GameFormLogic;

        /// <summary>
        /// 当前用 SAN 方式走子的一方，随着 SAN 输入区域的两个选项而变。
        /// </summary>
        Player currentSANPlayer;

        /// <summary>
        /// 鼠标点击的棋盘位置。
        /// </summary>
        Position SelectedPosition;

        /// <summary>
        /// 是一个字典。当点击了一个有棋子的棋盘格（原位置）开始选择走子时，每一个棋盘格对应一个目标位置；每一个目标位置可能对应多个 MoreDetailedMove（因为兵到达对方底格时，可以晋升为多种棋子，所以对应多个 MoreDetailedMove），用 List 组合在一起。那么每个 Button 就对应一个 List&lt;MoreDetailedMove&gt;。
        /// </summary>
        Dictionary<Button, List<MoreDetailedMove>> DestinationMoves;

        /// <summary>
        /// 历史移动的绑定源，要绑定到 GameLogic.GameMoves。
        /// </summary>
        BindingSource HistoryMovesBindingSource;

        /// <summary>
        /// 程序的名称，从应用集中提取，一般为 The Chess Board。
        /// </summary>
        public static string FormName = ((System.Reflection.AssemblyTitleAttribute)(System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false)[0])).Title.ToString();
        /// <summary>
        /// 程序的版本号，从应用集中提取。
        /// </summary>
        public static string FormVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// 构造函数，从已有的 gameFormLogic 创建窗体。
        /// </summary>
        /// <param name="gameFormLogic"></param>
        public TheChessBoard(ChessBoardGameFormLogic gameFormLogic)
        {
            InitializeComponent();
            InitializeCustomComponent();
            LoadChessBoardGame(gameFormLogic);
        }

        /// <summary>
        /// 构造函数，从一个新的 gameFormLogic 创建窗体。
        /// </summary>
        public TheChessBoard() : this(new ChessBoardGameFormLogic()) { }

        /// <summary>
        /// 载入一个 gameFormLogic。
        /// </summary>
        /// <param name="gameFormLogic"></param>
        private void LoadChessBoardGame(ChessBoardGameFormLogic gameFormLogic)
        {
            GameFormLogic = gameFormLogic;
            // 给 GameFormLogic 的事件绑定上本窗体里的方法。
            GameFormLogic.PropertyChanged -= GameFormLogicPropertyChangedSubscriber_UpdateUI;
            GameFormLogic.PropertyChanged += GameFormLogicPropertyChangedSubscriber_UpdateUI;
            GameFormLogic.GameProcedureStatusUpdated -= GameFormLogicProcedureStatusUpdatedSubscriber;
            GameFormLogic.GameProcedureStatusUpdated += GameFormLogicProcedureStatusUpdatedSubscriber;
            GameFormLogic.GameControlStatusUpdated -= GameFormLogicControlStatusUpdatedSubscriber;
            GameFormLogic.GameControlStatusUpdated += GameFormLogicControlStatusUpdatedSubscriber;
            GameFormLogic.PlayerIOStatusUpdated -= GameFormLogicPlayerIOStatusUpdatedSubscriber;
            GameFormLogic.PlayerIOStatusUpdated += GameFormLogicPlayerIOStatusUpdatedSubscriber;

            // 给本事件的一些控件绑定上 GameFormLogic 的成员。
            lblWhiteWatch.DataBindings.Clear();
            lblWhiteWatch.DataBindings.Add("Text", GameFormLogic, "WhiteStopwatchTime", false, DataSourceUpdateMode.OnPropertyChanged);
            lblBlackWatch.DataBindings.Clear();
            lblBlackWatch.DataBindings.Add("Text", GameFormLogic, "BlackStopwatchTime", false, DataSourceUpdateMode.OnPropertyChanged);

            lblWhiteCaptured.DataBindings.Clear();
            lblWhiteCaptured.DataBindings.Add("Text", GameFormLogic, "WhiteCapturedPieces", false, DataSourceUpdateMode.OnPropertyChanged);
            lblBlackCaptured.DataBindings.Clear();
            lblBlackCaptured.DataBindings.Add("Text", GameFormLogic, "BlackCapturedPieces", false, DataSourceUpdateMode.OnPropertyChanged);

            // 给 GameFormLogic 的 FormInvoke （线程安全的函数运行器）赋值为本窗体的 Invoke 函数。
            GameFormLogic.FormInvoke = Invoke;

            RefreshHistoryMoveSourceReference();
        }

        #region 和控件有关的方法
        #region 和控件初始化有关的方法

        /// <summary>
        /// 能实现标签不换行的 DataGridView
        /// </summary>
        class MyDataGridView : DataGridView
        {
            // https://stackoverflow.com/questions/910210/how-to-disable-ellipsis-of-cell-texts-in-a-windowsforms-datagridview
            protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
            {
                if (e.RowIndex != -1)
                {
                    base.OnCellPainting(e);
                    return;
                }

                var isSelected = e.State.HasFlag(DataGridViewElementStates.Selected);

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Background
                    //| DataGridViewPaintParts.Border
                    //| DataGridViewPaintParts.ContentBackground
                    //| DataGridViewPaintParts.ContentForeground
                    | DataGridViewPaintParts.ErrorIcon
                    | DataGridViewPaintParts.Focus
                    | DataGridViewPaintParts.SelectionBackground);

                using (Brush foreBrush = new SolidBrush(e.CellStyle.ForeColor),
                    selectedForeBrush = new SolidBrush(e.CellStyle.SelectionForeColor))
                {
                    if (e.Value != null)
                    {
                        StringFormat strFormat = new StringFormat();
                        strFormat.Trimming = StringTrimming.Character;
                        var brush = isSelected ? selectedForeBrush : foreBrush;

                        var fs = e.Graphics.MeasureString((string)e.Value, e.CellStyle.Font);
                        var topPos = e.CellBounds.Top + ((e.CellBounds.Height - fs.Height) / 2);

                        // I found that the cell text is drawn in the wrong position
                        // for the first cell in the column header row, hence the 4px
                        // adjustment
                        var leftPos = e.CellBounds.X;
                        if (e.RowIndex == -1 && e.ColumnIndex == 0) leftPos += 4;

                        e.Graphics.DrawString((String)e.Value, e.CellStyle.Font,
                            brush, leftPos, topPos, strFormat);
                    }
                }

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                e.Handled = true;
            }
        }

        private MyDataGridView dgvHistoryMoves;

        /// <summary>
        /// 初始化一些额外的控件。
        /// </summary>
        private void InitializeCustomComponent()
        {
            this.SuspendLayout();
            this.groupBox4.SuspendLayout();
            InitializeButtonSquares();

            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            if (true)
            {
                FormClosing += (sender, e) =>
                {
                    if (MessageBox.Show("确认要退出吗？", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        e.Cancel = false;
                        BusifyCursor();
                        GameFormLogic.KillAllAndResetStatus();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                };
            }


            // Log
            SetLog(string.Format(@"{{\rtf1\ansicpg936 \b {0} {1}\b0 \line 窗体控件设置完成\line}}", FormName, FormVersion));

            // dgvHistoryMoves
            // 

            dgvHistoryMoves = new MyDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistoryMoves)).BeginInit();
            this.dgvHistoryMoves.AllowUserToAddRows = false;
            this.dgvHistoryMoves.AllowUserToDeleteRows = false;
            this.dgvHistoryMoves.AllowUserToResizeRows = false;
            this.dgvHistoryMoves.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvHistoryMoves.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvHistoryMoves.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistoryMoves.Location = new System.Drawing.Point(12, 26);
            this.dgvHistoryMoves.Name = "dgvHistoryMoves";
            this.dgvHistoryMoves.ReadOnly = true;
            this.dgvHistoryMoves.RowHeadersVisible = false;
            this.dgvHistoryMoves.RowTemplate.Height = 27;
            this.dgvHistoryMoves.Size = new System.Drawing.Size(272, 367);
            this.dgvHistoryMoves.TabIndex = 22;

            dgvHistoryMoves.AutoGenerateColumns = false;
            dgvHistoryMoves.MultiSelect = true;
            dgvHistoryMoves.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistoryMoves.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvHistoryMoves.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvHistoryMoves.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
            dgvHistoryMoves.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvHistoryMoves.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHistoryMoves.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 13, FontStyle.Bold, GraphicsUnit.Pixel);

            dgvHistoryMoves.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "No",
                Width = 35,
                ReadOnly = true,
                Name = "Index",
                DataPropertyName = "Index"
            });
            dgvHistoryMoves.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "行动方",
                Width = 45,
                ReadOnly = true,
                Name = "PlayerString",
                DataPropertyName = "PlayerString"
            });
            dgvHistoryMoves.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SAN",
                Width = 52,
                ReadOnly = true,
                Name = "SANString",
                DataPropertyName = "SANString"
            });
            dgvHistoryMoves.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "文字描述",
                Width = 114,
                ReadOnly = true,
                Name = "FriendlyText",
                DataPropertyName = "FriendlyText"
            });
            dgvHistoryMoves.SelectionChanged += (obj, e) =>
            {
                
                BindingSource h = (BindingSource)dgvHistoryMoves.DataSource;
                if (h == null)
                    return;
                BindingList<MoreDetailedMoveImitator> l = (BindingList<MoreDetailedMoveImitator>)h.DataSource;
                if (l == null)
                    return;

                if (dgvHistoryMoves.SelectedRows.Count == 0)
                    return;

                UpdateBoardButtons(l[dgvHistoryMoves.SelectedRows.Cast<DataGridViewRow>().Max((x)=>x.Index)]);
            };
            this.groupBox4.Controls.Add(this.dgvHistoryMoves);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistoryMoves)).EndInit();

            groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            groupBox4.PerformLayout();
            this.PerformLayout();
        }

        /// <summary>
        /// 初始化六十四个棋盘格按钮，在棋盘 Panel 里加上它们。
        /// </summary>
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

        /// <summary>
        /// 这个窗体完全载入后，执行的操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TheChessBoard_Load(object sender, EventArgs e)
        {
            /// 加一个日志监听器，可以将日志通过 AppendLog 方法记录到右边的日志富文本框里。 
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new RichTextBoxTraceListener(AppendLog));
            Trace.AutoFlush = true;
            Trace.TraceInformation("日志组件开始运作");

            GameFormLogic.InvokeAllUpdates();
        }

        #endregion

        /// <summary>
        /// 更新窗体内的鼠标指针为“忙。”
        /// </summary>
        void ArrowBusifyCursor() { this.Cursor = Cursors.AppStarting; }
        /// <summary>
        /// 更新窗体内的鼠标指针为“忙。”
        /// </summary>
        void BusifyCursor() { this.Cursor = Cursors.WaitCursor; }
        /// <summary>
        /// 更新窗体内的鼠标指针为“正常。”
        /// </summary>
        void RestoreCursor() { this.Cursor = Cursors.Default; }

        /// <summary>
        /// 给日志富文本框增加一条日志。
        /// </summary>
        /// <param name="logRTF">要添加的日志。</param>
        public void AppendLog(String logRTF)
        {
            if (this.IsHandleCreated == false)
            {
                rtbLog.Select(rtbLog.TextLength, 0); ;
                rtbLog.SelectedRtf = logRTF;
            }
            else 
                this.BeginInvoke(new Action(() =>
                {
                    rtbLog.Select(rtbLog.TextLength, 0); ;
                    rtbLog.SelectedRtf = logRTF;
                }));
        }
        
        /// <summary>
        /// 设置日志富文本框的内容。
        /// </summary>
        /// <param name="logRTF">要设置的日志内容。</param>
        public void SetLog(String logRTF)
        {
            if (this.IsHandleCreated == false)
            {
                rtbLog.Select(0, rtbLog.TextLength); ;
                rtbLog.SelectedRtf = logRTF;
            }
            else
                this.BeginInvoke(new Action(() =>
                {
                    rtbLog.Select(0, rtbLog.TextLength);
                    rtbLog.SelectedRtf = logRTF;
                }));
        }

        /// <summary>
        /// 是一段比较长的逻辑，不过做的事情比较简单，就是根据当前 GameFormLogic 的状态和当前游戏的行动方，正确设置窗体里各种控件的可用性（Enabled）和是否选中（Checked）属性。
        /// </summary>
        private void SetControlsEnabledAccordingToWhoseTurnAndStatus()
        {
            if (GameFormLogic.Mode == GameMode.Manual)
                if (GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.Running)
                {
                    if (GameFormLogic.CareWhoseTurnItIs)
                    {
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            rdbBlack.Enabled = true;
                            rdbBlack.Checked = true;
                            rdbWhite.Enabled = false;
                            rdbWhite.Checked = false;
                        }
                        else
                        {
                            rdbBlack.Enabled = false;
                            rdbBlack.Checked = false;
                            rdbWhite.Enabled = true;
                            rdbWhite.Checked = true;
                        }
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            btnBlackReadMove.Enabled = GameFormLogic.BlackStatus == StdIOState.NotRequesting && !GameFormLogic.HasBlackManuallyMoved;
                            btnWhiteReadMove.Enabled = false;
                        }
                        else
                        {
                            btnWhiteReadMove.Enabled = GameFormLogic.WhiteStatus == StdIOState.NotRequesting && !GameFormLogic.HasWhiteManuallyMoved;
                            btnBlackReadMove.Enabled = false;
                        }
                    }
                    else
                    {
                        rdbBlack.Enabled = true;
                        rdbWhite.Enabled = true;

                        btnWhiteReadMove.Enabled = GameFormLogic.WhiteStatus == StdIOState.NotRequesting && !GameFormLogic.HasWhiteManuallyMoved && GameFormLogic.WhoseTurn == Player.White;
                        btnBlackReadMove.Enabled = GameFormLogic.BlackStatus == StdIOState.NotRequesting && !GameFormLogic.HasBlackManuallyMoved && GameFormLogic.WhoseTurn == Player.Black;
                    }
                }
                else
                {
                    if (GameFormLogic.WhoseTurn == Player.Black)
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = true;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = false;
                    }
                    else
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = false;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = true;
                    }
                    btnWhiteReadMove.Enabled = false;
                    btnBlackReadMove.Enabled = false;
                }
            else if (GameFormLogic.Mode == GameMode.BlackAuto)
                if (GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.Running)
                {
                    if (GameFormLogic.CareWhoseTurnItIs)
                    {
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            rdbBlack.Enabled = false;
                            rdbBlack.Checked = true;
                            rdbWhite.Enabled = false;
                            rdbWhite.Checked = false;
                        }
                        else
                        {
                            rdbBlack.Enabled = false;
                            rdbBlack.Checked = false;
                            rdbWhite.Enabled = true;
                            rdbWhite.Checked = true;
                        }
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            btnBlackReadMove.Enabled = false;
                            btnWhiteReadMove.Enabled = false;
                        }
                        else
                        {
                            btnWhiteReadMove.Enabled = GameFormLogic.WhiteStatus == StdIOState.NotRequesting && !GameFormLogic.HasWhiteManuallyMoved;
                            btnBlackReadMove.Enabled = false;
                        }
                    }
                    else
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = false;
                        rdbWhite.Enabled = true;
                        rdbWhite.Checked = true;

                        btnWhiteReadMove.Enabled = GameFormLogic.WhiteStatus == StdIOState.NotRequesting && !GameFormLogic.HasWhiteManuallyMoved && GameFormLogic.WhoseTurn == Player.White;
                        btnBlackReadMove.Enabled = GameFormLogic.BlackStatus == StdIOState.NotRequesting && !GameFormLogic.HasBlackManuallyMoved && GameFormLogic.WhoseTurn == Player.Black;
                    }
                }
                else
                {
                    if (GameFormLogic.WhoseTurn == Player.Black)
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = true;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = false;
                    }
                    else
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = false;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = true;
                    }
                    btnWhiteReadMove.Enabled = false;
                    btnBlackReadMove.Enabled = false;
                }
            else if (GameFormLogic.Mode == GameMode.WhiteAuto)
                if (GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.Running)
                {
                    if (GameFormLogic.CareWhoseTurnItIs)
                    {
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            rdbBlack.Enabled = true;
                            rdbBlack.Checked = true;
                            rdbWhite.Enabled = false;
                            rdbWhite.Checked = false;
                        }
                        else
                        {
                            rdbBlack.Enabled = false;
                            rdbBlack.Checked = false;
                            rdbWhite.Enabled = false;
                            rdbWhite.Checked = true;
                        }
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            btnBlackReadMove.Enabled = GameFormLogic.BlackStatus == StdIOState.NotRequesting && !GameFormLogic.HasBlackManuallyMoved;
                            btnWhiteReadMove.Enabled = false;
                        }
                        else
                        {
                            btnWhiteReadMove.Enabled = false;
                            btnBlackReadMove.Enabled = false;
                        }
                    }
                    else
                    {
                        rdbBlack.Enabled = true;
                        rdbBlack.Checked = true;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = false;

                        btnWhiteReadMove.Enabled = GameFormLogic.WhiteStatus == StdIOState.NotRequesting && !GameFormLogic.HasWhiteManuallyMoved && GameFormLogic.WhoseTurn == Player.White;
                        btnBlackReadMove.Enabled = GameFormLogic.BlackStatus == StdIOState.NotRequesting && !GameFormLogic.HasBlackManuallyMoved && GameFormLogic.WhoseTurn == Player.Black;
                    }
                }
                else
                {
                    if (GameFormLogic.WhoseTurn == Player.Black)
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = true;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = false;
                    }
                    else
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = false;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = true;
                    }
                    btnWhiteReadMove.Enabled = false;
                    btnBlackReadMove.Enabled = false;
                }
            else if (GameFormLogic.Mode == GameMode.BlackAuto)
                if (GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.Running)
                {
                    if (GameFormLogic.CareWhoseTurnItIs)
                    {
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            rdbBlack.Enabled = false;
                            rdbBlack.Checked = true;
                            rdbWhite.Enabled = false;
                            rdbWhite.Checked = false;
                        }
                        else
                        {
                            rdbBlack.Enabled = false;
                            rdbBlack.Checked = false;
                            rdbWhite.Enabled = true;
                            rdbWhite.Checked = true;
                        }
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            btnBlackReadMove.Enabled = false;
                            btnWhiteReadMove.Enabled = false;
                        }
                        else
                        {
                            btnWhiteReadMove.Enabled = GameFormLogic.WhiteStatus == StdIOState.NotRequesting && !GameFormLogic.HasWhiteManuallyMoved;
                            btnBlackReadMove.Enabled = false;
                        }
                    }
                    else
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = false;
                        rdbWhite.Enabled = true;
                        rdbWhite.Checked = true;

                        btnWhiteReadMove.Enabled = GameFormLogic.WhiteStatus == StdIOState.NotRequesting && !GameFormLogic.HasWhiteManuallyMoved && GameFormLogic.WhoseTurn == Player.White;
                        btnBlackReadMove.Enabled = GameFormLogic.BlackStatus == StdIOState.NotRequesting && !GameFormLogic.HasBlackManuallyMoved && GameFormLogic.WhoseTurn == Player.Black;
                    }
                }
                else
                {
                    if (GameFormLogic.WhoseTurn == Player.Black)
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = true;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = false;
                    }
                    else
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = false;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = true;
                    }
                    btnWhiteReadMove.Enabled = false;
                    btnBlackReadMove.Enabled = false;
                }
            else if (GameFormLogic.Mode == GameMode.BothAuto)
                if (GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.Running)
                {
                    if (GameFormLogic.CareWhoseTurnItIs)
                    {
                        if (GameFormLogic.WhoseTurn == Player.Black)
                        {
                            rdbBlack.Enabled = false;
                            rdbBlack.Checked = true;
                            rdbWhite.Enabled = false;
                            rdbWhite.Checked = false;
                        }
                        else
                        {
                            rdbBlack.Enabled = false;
                            rdbBlack.Checked = false;
                            rdbWhite.Enabled = false;
                            rdbWhite.Checked = true;
                        }

                    }
                    else
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = GameFormLogic.WhoseTurn == Player.Black;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = GameFormLogic.WhoseTurn == Player.White;
                    }
                    btnWhiteReadMove.Enabled = false;
                    btnBlackReadMove.Enabled = false;
                }
                else
                {
                    if (GameFormLogic.WhoseTurn == Player.Black)
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = true;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = false;
                    }
                    else
                    {
                        rdbBlack.Enabled = false;
                        rdbBlack.Checked = false;
                        rdbWhite.Enabled = false;
                        rdbWhite.Checked = true;
                    }
                    btnWhiteReadMove.Enabled = false;
                    btnBlackReadMove.Enabled = false;
                }
            else if (GameFormLogic.Mode == GameMode.NotStarted)
            {
                rdbBlack.Enabled = false;
                rdbBlack.Checked = GameFormLogic.WhoseTurn == Player.Black;
                rdbWhite.Enabled = false;
                rdbWhite.Checked = GameFormLogic.WhoseTurn == Player.White;

                btnWhiteReadMove.Enabled = false;
                btnBlackReadMove.Enabled = false;
            }
            if (frequentlyRefresh)
            {
                rdbBlack.Refresh();
                rdbBlack.Refresh();
                rdbWhite.Refresh();
                rdbWhite.Refresh();

                btnWhiteReadMove.Refresh();
                btnBlackReadMove.Refresh();
            }
        }

        #endregion

        #region 与控件触发事件有关的方法
        /// <summary>
        /// 当点击 SAN 输入区域内的“走一步”按钮时，触发 SAN 走子。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                GameFormLogic.ManualMove(txbMoveStr.Text, currentSANPlayer, out Piece pieceJustCaptured);
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

        /// <summary>
        /// 当点击 SAN 输入区域内的“白方”、“黑方”选项按钮时，更换当前 SAN 行动方。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SANPlayerChanged(object sender, EventArgs e)
        {
            if (rdbBlack.Checked)
            {
                currentSANPlayer = ChessDotNet.Player.Black;
            }
            if (rdbWhite.Checked)
            {
                currentSANPlayer = ChessDotNet.Player.White;
            }
        }

        /// <summary>
        /// “忽略当前执棋”变化后要做的一系列操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbDontCareWhoseTurnItIs_CheckedChanged(object sender, EventArgs e)
        {
            GameFormLogic.CareWhoseTurnItIs = !ckbDontCareWhoseTurnItIs.Checked;
            SetControlsEnabledAccordingToWhoseTurnAndStatus();
            GameFormLogic.ControlStatus = ChessBoardGameControlState.Idle;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            txbMoveStr.Focus();
        }

        /// <summary>
        /// 根据点击的棋盘格按钮的行、列，返回一个特别定制的点击函数。
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="File"></param>
        /// <returns></returns>
        private EventHandler SquareClickCurried(int Row, int File)
        {
            return (sender, e) => { SquareClick(8 - Row, (File)File); };
        }

        /// <summary>
        /// 根据点击的棋盘格按钮的行、列，返回一个特别定制的右键点击函数。
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="File"></param>
        /// <returns></returns>
        private MouseEventHandler SquareRightMouseButtonCurried(int Row, int File)
        {
            return (sender, e) =>
            {
                if (e.Button.Equals(MouseButtons.Right))
                    SquareRightMouseButton(8 - Row, (File)File);
            };
        }

        /// <summary>
        /// 当右键点击某一个棋盘格按钮时，将从这个棋盘格出发的所有操作列在一个表 aggregateResult 里，调用移动消歧义对话框 MoveDisambiguationDialog，让用户选择一个走子。
        /// </summary>
        /// <param name="currRank"></param>
        /// <param name="currFile"></param>
        void SquareRightMouseButton(int currRank, File currFile)
        {
            bool didSomething = SquareClick(currRank, currFile, false, false);
            if (didSomething && GameFormLogic.ControlStatus == ChessBoardGameControlState.Selected)
            {
                if (DestinationMoves.Count == 0)
                    return;
                if (DestinationMoves.Count == 1)
                    return;
                //var DestinationMovesCopy = new Dictionary<Button, List<MoreDetailedMove>>(DestinationMoves);
                var aggregateResult = DestinationMoves.Aggregate((a, b) => { return new KeyValuePair<Button, List<MoreDetailedMove>>(a.Key, a.Value.Union(b.Value).ToList()); });

                MoveDisambiguationDialog disaDialog = new MoveDisambiguationDialog(aggregateResult.Value);
                disaDialog.CallbackEvent += DisambiguationDone;
                disaDialog.ShowDialog();
            }
        }

        /// <summary>
        /// 当走子消歧义完成之后，要做的事。
        /// </summary>
        /// <param name="move"></param>
        void DisambiguationDone(MoreDetailedMove move)
        {
            GameFormLogic.ManualMove(move, true, out Piece pieceJustCaptured);
        }

        /// <summary>
        /// 取消棋盘格选中时，要做的事。
        /// </summary>
        void SquareCancelSelect()
        {
            CleanSquareColor();
        }

        /// <summary>
        /// 当右键点击某一个棋盘格按钮时，要做的事，例如将窗体空间状态 ControlStatus 设为 Selected，
        /// 或是在选定了一个走子之后执行走子操作。
        /// </summary>
        /// <param name="currRank"></param>
        /// <param name="currFile"></param>
        /// <param name="allowCancelSelect"></param>
        /// <param name="allowMoveSelect"></param>
        /// <returns>返回值表示这个点击是否有效触发了操作。</returns>
        bool SquareClick(int currRank, File currFile, bool allowCancelSelect = true, bool allowMoveSelect = true)
        {
            // 当不是可以点击的状态时，什么都不敢
            if (GameFormLogic.Mode == GameMode.BothAuto && GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.Running)
                return false;

            // 当不是可以点击的状态时，什么都不做。
            if (GameFormLogic.ProcedureStatus != ChessBoardGameProcedureState.Running)
            {
                if (GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.NotStarted)
                    MessageBox.Show("游戏尚未开始，请选择模式后点击左侧“开始”。", "游戏尚未开始", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                    MessageBox.Show("游戏已经结束，请点击“重置”后重新开始。", "游戏已经结束", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                return false;
            }


            // 计算出对应的 Position，获取对应的 Button。
            var pos = new Position(currFile, currRank);
            var clickedButton = btnBoardSquares[8 * (8 - currRank) + (int)currFile];

            if (GameFormLogic.ControlStatus == ChessBoardGameControlState.Selected)
            {
                // 是否取消选定。
                bool cancelSelect = pos.Equals(SelectedPosition) && allowCancelSelect;
                // 是否执行走子。
                bool moveSelect = DestinationMoves.ContainsKey(clickedButton) && allowMoveSelect;
                if (cancelSelect)
                {
                    GameFormLogic.ControlStatus = ChessBoardGameControlState.Idle;
                }

                if (moveSelect)
                {
                    // 如果要走子，看看目标格对应的 MoreDetailedMove 是不是只有一个。如果只有一个，直接执行；否则弹出消歧义窗口。
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

            // 当状态为 Idle，或者是 Selected 但是并没有取消选定或是走子时（重新 Select）
            if (GameFormLogic.ControlStatus == ChessBoardGameControlState.Idle || GameFormLogic.ControlStatus == ChessBoardGameControlState.Selected)
            {
                // 当在历史走子中查看了历史棋盘，点击一下棋盘执行的操作是强制回到最新的棋盘。
                if (dgvHistoryMoves.SelectedRows.Count != 0 && dgvHistoryMoves.SelectedRows.Cast<DataGridViewRow>().Max((x) => x.Index) != dgvHistoryMoves.Rows.Count - 1 || dgvHistoryMoves.SelectedRows.Count == 0)
                {
                    dgvHistoryMoves.ClearSelection();
                    dgvHistoryMoves.Rows[dgvHistoryMoves.Rows.Count - 1].Selected = true;
                    return false;
                }

                var piece = GameFormLogic.Game.GetPieceAt(pos);
                // 当点击的棋盘格上有棋子时（开始选择走子）
                if (piece != null)
                {
                    var validMoves = GameFormLogic.Game.GetValidMoves(pos, false);
                    CleanSquareColor();
                    SelectedPosition = pos;
                    GameFormLogic.ControlStatus = ChessBoardGameControlState.Selected;

                    DestinationMoves.Clear();

                    // 将可有的走子以 <棋盘格按钮, 对应的走子列表> 的一个个键值对的形式存入 DestinationMoves。
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
                    // 当不走子、不取消时，只能让控件状态回到“闲置。”
                    GameFormLogic.ControlStatus = ChessBoardGameControlState.Idle;
                }
            }
            return false;
        }

        /// <summary>
        /// 点击“白方读入一步”的操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWhiteReadMove_Click(object sender, EventArgs e)
        {
            GameFormLogic.ProcessAllowOutputAndWait(StdIOType.White);
        }

        /// <summary>
        /// 点击“黑方读入一步”的操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBlackReadMove_Click(object sender, EventArgs e)
        {
            GameFormLogic.ProcessAllowOutputAndWait(StdIOType.Black);
        }

        /// <summary>
        /// 读入白方 AI。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadWhiteAI_Click(object sender, EventArgs e)
        {
            InputExecCommandDialog inputDialog = new InputExecCommandDialog(Player.White);
            inputDialog.txbExecPath.Text = Properties.Settings.Default.WhiteDefaultExecPath;
            inputDialog.txbExecArguments.Text = Properties.Settings.Default.WhiteDefaultExecArguments;
            inputDialog.txbExecPath.Items.AddRange(Properties.Settings.Default.AIExecPathHistory.Cast<String>().ToArray());
            inputDialog.txbExecArguments.Items.AddRange(Properties.Settings.Default.AIExecArgumentsHistory.Cast<String>().ToArray());
            inputDialog.CallbackEvent += (player, execPath, execArguments) =>
            {
                if (GameFormLogic.LoadAIExec(player, execPath, execArguments) == false)
                    return;
                if(Properties.Settings.Default.AutoSaveAIConfig)
                {
                    Properties.Settings.Default.WhiteDefaultExecPath = execPath;
                    Properties.Settings.Default.WhiteDefaultExecArguments = execArguments;
                }
                if (!Properties.Settings.Default.AIExecPathHistory.Contains(execPath))
                {
                    if (Properties.Settings.Default.AIExecPathHistory.Count > 20)
                        Properties.Settings.Default.AIExecPathHistory.RemoveAt(Properties.Settings.Default.AIExecPathHistory.Count - 1);
                    Properties.Settings.Default.AIExecPathHistory.Insert(0, execPath);
                }

                if (!Properties.Settings.Default.AIExecArgumentsHistory.Contains(execArguments))
                {
                    if (Properties.Settings.Default.AIExecArgumentsHistory.Count > 20)
                        Properties.Settings.Default.AIExecArgumentsHistory.RemoveAt(Properties.Settings.Default.AIExecArgumentsHistory.Count - 1);
                    Properties.Settings.Default.AIExecArgumentsHistory.Insert(0, execArguments);
                }
                Properties.Settings.Default.Save();
            };
            inputDialog.ShowDialog();
        }

        /// <summary>
        /// 读入黑方 AI。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadBlackAI_Click(object sender, EventArgs e)
        {
            InputExecCommandDialog inputDialog = new InputExecCommandDialog(Player.Black);
            inputDialog.txbExecPath.Text = Properties.Settings.Default.BlackDefaultExecPath;
            inputDialog.txbExecArguments.Text = Properties.Settings.Default.BlackDefaultExecArguments;
            inputDialog.txbExecPath.Items.AddRange(Properties.Settings.Default.AIExecPathHistory.Cast<String>().ToArray());
            inputDialog.txbExecArguments.Items.AddRange(Properties.Settings.Default.AIExecArgumentsHistory.Cast<String>().ToArray());
            inputDialog.CallbackEvent += (player, execPath, execArguments) =>
            {
                if (GameFormLogic.LoadAIExec(player, execPath, execArguments) == false)
                    return;
                if (Properties.Settings.Default.AutoSaveAIConfig)
                {
                    Properties.Settings.Default.BlackDefaultExecPath = execPath;
                    Properties.Settings.Default.BlackDefaultExecArguments = execArguments;
                }
                if (!Properties.Settings.Default.AIExecPathHistory.Contains(execPath))
                {
                    if (Properties.Settings.Default.AIExecPathHistory.Count >= 20)
                        Properties.Settings.Default.AIExecPathHistory.RemoveAt(Properties.Settings.Default.AIExecPathHistory.Count - 1);
                    Properties.Settings.Default.AIExecPathHistory.Insert(0, execPath);
                }

                if (!Properties.Settings.Default.AIExecArgumentsHistory.Contains(execArguments))
                {
                    if (Properties.Settings.Default.AIExecArgumentsHistory.Count >= 20)
                        Properties.Settings.Default.AIExecArgumentsHistory.RemoveAt(Properties.Settings.Default.AIExecArgumentsHistory.Count - 1);
                    Properties.Settings.Default.AIExecArgumentsHistory.Insert(0, execArguments);
                }
                Properties.Settings.Default.Save();
            };
            inputDialog.ShowDialog();
        }

        /// <summary>
        /// 点击“开始”按钮时的操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModeConfirm_Click(object sender, EventArgs e)
        {
            GameMode mode;
            if (rdbBlackAuto.Checked)
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
            GameFormLogic.Start(mode);
        }

        /// <summary>
        /// 点击“重置”时的操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllReset_Click(object sender, EventArgs e)
        {
            BusifyCursor();
            GameFormLogic.ResetGame();
            RefreshHistoryMoveSourceReference();
            RestoreCursor();
        }

        #endregion

        #region FormGame 引发事件触发的方法
        /// <summary>
        /// 重新设置历史记录 DataGridView 的数据源。
        /// </summary>
        private void RefreshHistoryMoveSourceReference()
        {
            HistoryMovesBindingSource = new BindingSource();
            HistoryMovesBindingSource.DataSource = GameFormLogic.GameMoves;
            dgvHistoryMoves.DataSource = HistoryMovesBindingSource;
            dgvHistoryMoves.Select();
        }

        /// <summary>
        /// 游戏逻辑中 ProcedureStatus 改变事件的订阅器。
        /// </summary>
        /// <param name="e"></param>
        private void GameFormLogicProcedureStatusUpdatedSubscriber(StatusUpdatedEventArgs e)
        {

            if (GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.Running)
            {
                if (e.UpdateImportant == true)
                {
                    // New game started
                    DestinationMoves = new Dictionary<Button, List<MoreDetailedMove>>();
                    // History
                    RefreshHistoryMoveSourceReference();

                    GameFormLogic.CareWhoseTurnItIs = !(ckbDontCareWhoseTurnItIs.Checked);
                    btnLoadWhiteAI.Enabled = false;
                    btnLoadBlackAI.Enabled = false;

                    this.Activate();
                }

                btnStart.Enabled = false;
                btnMove.Enabled = true;
                pnlBlackMode.Enabled = false;
                pnlWhiteMode.Enabled = false;
                btnStart.Enabled = false;
                btnStop.Enabled = (GameFormLogic.Mode == GameMode.BothAuto);


                lblFormStatus.ForeColor = ColorRunning;
                lblFormStatusText.Text = "进行中";
            }
            else
            {
                btnMove.Enabled = false;
                btnStop.Enabled = false;
                if (GameFormLogic.ProcedureStatus == ChessBoardGameProcedureState.NotStarted)
                {
                    lblFormStatus.ForeColor = ColorGameNotStarted;
                    lblFormStatusText.Text = "未开始";
                    btnLoadWhiteAI.Enabled = true;
                    btnLoadBlackAI.Enabled = true;

                    btnStart.Enabled = true;
                    pnlBlackMode.Enabled = true;
                    pnlWhiteMode.Enabled = true;
                }
                else
                {
                    btnStart.Enabled = false;
                    pnlBlackMode.Enabled = false;
                    pnlWhiteMode.Enabled = false;

                    this.BeginInvoke(new Action(delegate { MessageBox.Show(e.Reason, "游戏结束", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }));
                    switch (GameFormLogic.ProcedureStatus)
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
                        case ChessBoardGameProcedureState.Stopped:
                            lblFormStatus.ForeColor = ColorStop;
                            lblFormStatusText.Text = "终止";
                            break;
                    }
                }
            }

            if (frequentlyRefresh)
            {
                btnLoadWhiteAI.Refresh();
                btnLoadBlackAI.Refresh();
                btnStart.Refresh();
                btnMove.Refresh();
                pnlBlackMode.Refresh();
                pnlWhiteMode.Refresh();
                btnStart.Refresh();
                lblFormStatus.Refresh();
                lblFormStatusText.Refresh();
                dgvHistoryMoves.Refresh();
            }
            SetControlsEnabledAccordingToWhoseTurnAndStatus();
        }

        /// <summary>
        /// ControlStatus 更新事件的订阅器。
        /// </summary>
        /// <param name="e"></param>
        private void GameFormLogicControlStatusUpdatedSubscriber(StatusUpdatedEventArgs e)
        {
            if (GameFormLogic.ControlStatus == ChessBoardGameControlState.Idle || GameFormLogic.ControlStatus == ChessBoardGameControlState.NotStarted || GameFormLogic.ControlStatus == ChessBoardGameControlState.StdIORunning || GameFormLogic.ControlStatus == ChessBoardGameControlState.Stopped)
            {
                SquareCancelSelect();
            }

            if (GameFormLogic.ControlStatus == ChessBoardGameControlState.Idle || GameFormLogic.ControlStatus == ChessBoardGameControlState.Selected)
            {
                SANPlayerChanged(null, null);
                if(GameFormLogic.Mode != GameMode.BothAuto)
                    if (btnMove.Enabled == false)
                        btnMove.Enabled = true;
                RestoreCursor();
            }

            if (GameFormLogic.ControlStatus == ChessBoardGameControlState.StdIORunning)
            {
                btnMove.Enabled = false;
                ArrowBusifyCursor();
            }
            if (frequentlyRefresh)
            { btnMove.Refresh(); }
            SetControlsEnabledAccordingToWhoseTurnAndStatus();
        }

        /// <summary>
        /// PlayerIOStatus 更新这一事件的订阅器。
        /// </summary>
        /// <param name="e"></param>
        private void GameFormLogicPlayerIOStatusUpdatedSubscriber(StatusUpdatedEventArgs e)
        {
            if (GameFormLogic.WhiteStatus == StdIOState.NotLoaded)
            {
                rdbWhiteAuto.Enabled = false;
                rdbWhiteManual.Checked = true;
                lblWhiteStatus.ForeColor = ColorProcNotLoaded;
                lblWhiteStatusText.Text = "未装载";
            }
            else if (GameFormLogic.WhiteStatus == StdIOState.NotStarted)
            {
                rdbWhiteAuto.Enabled = true;
                lblWhiteStatus.ForeColor = ColorProcNotStarted;
                lblWhiteStatusText.Text = "未开始";
            }
            else if (GameFormLogic.WhiteStatus == StdIOState.NotRequesting)
            {
                rdbWhiteAuto.Enabled = true;
                lblWhiteStatus.ForeColor = ColorIdle;
                lblWhiteStatusText.Text = "空闲";
            }
            else if (GameFormLogic.WhiteStatus == StdIOState.Requesting)
            {
                rdbWhiteAuto.Enabled = true;
                lblWhiteStatus.ForeColor = ColorBusy;
                lblWhiteStatusText.Text = "请求中/阻塞";
            }

            if (GameFormLogic.BlackStatus == StdIOState.NotLoaded)
            {
                rdbBlackAuto.Enabled = false;
                rdbBlackManual.Checked = true;
                lblBlackStatus.ForeColor = ColorProcNotLoaded;
                lblBlackStatusText.Text = "未装载";
            }
            else if (GameFormLogic.BlackStatus == StdIOState.NotStarted)
            {
                rdbBlackAuto.Enabled = true;
                lblBlackStatus.ForeColor = ColorProcNotStarted;
                lblBlackStatusText.Text = "未开始";
            }
            else if (GameFormLogic.BlackStatus == StdIOState.NotRequesting)
            {
                rdbBlackAuto.Enabled = true;
                lblBlackStatus.ForeColor = ColorIdle;
                lblBlackStatusText.Text = "空闲";
            }
            else if (GameFormLogic.BlackStatus == StdIOState.Requesting)
            {
                rdbBlackAuto.Enabled = true;
                lblBlackStatus.ForeColor = ColorBusy;
                lblBlackStatusText.Text = "请求中/阻塞";
            }
            if (frequentlyRefresh)
            {
                rdbWhiteAuto.Refresh();
                rdbWhiteManual.Refresh();
                lblWhiteStatus.Refresh();
                lblWhiteStatusText.Refresh();
                rdbBlackAuto.Refresh();
                rdbBlackManual.Refresh();
                lblBlackStatus.Refresh();
                lblBlackStatusText.Refresh();
            }
            SetControlsEnabledAccordingToWhoseTurnAndStatus();
        }

        /// <summary>
        /// PropertyChanged 事件的订阅器（不是唯一的订阅器，窗体的数据绑定（DataBinding）会自动订阅这个事件）。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameFormLogicPropertyChangedSubscriber_UpdateUI(object sender, PropertyChangedEventArgs e)
        {
            if (e == null || e.PropertyName == "BoardPrint")
            {
                UpdateBoardButtons();
            }
            if (e == null || e.PropertyName == "WhoseTurn")
            {
                UpdateWhoseTurn();
            }
            if (e == null || e.PropertyName == "GameMoves")
            {
                dgvHistoryMoves.ClearSelection();
                dgvHistoryMoves.Rows[dgvHistoryMoves.Rows.Count - 1].Selected = true;
                dgvHistoryMoves.FirstDisplayedScrollingRowIndex = dgvHistoryMoves.Rows.Count - 1;
                dgvHistoryMoves.Refresh();
            }
        }

        /// <summary>
        /// 更新棋盘格按钮的文字和底色。
        /// </summary>
        /// <param name="move"></param>
        public void UpdateBoardButtons(MoreDetailedMoveImitator move = null)
        {
            var bp = move == null ? GameFormLogic.BoardPrint : move.BoardPrint;
            MoreDetailedMove lastMove = move == null ? null : move.AssociatedMoreDetailedMove;
            int oIndex = -1;
            int dIndex = -1;
            bool check = false;
            bool checkmate = false;
            bool isCapture = false;
            bool isEnpassant = false;
            Player player = Player.None;

            if (lastMove == null && GameFormLogic.Game.Moves.Count >= 1)
            {
                lastMove = GameFormLogic.Game.Moves.Last();
            }

            if (lastMove != null)
            {
                oIndex = (int)lastMove.OriginalPosition.File + (8 - (int)lastMove.OriginalPosition.Rank) * 8;
                dIndex = (int)lastMove.NewPosition.File + (8 - (int)lastMove.NewPosition.Rank) * 8;
                check = lastMove.IsChecking.Value;
                checkmate = lastMove.IsCheckmate.Value;
                isCapture = lastMove.IsCapture;
                isEnpassant = lastMove.IsEnpassant;
                player = lastMove.Player;
            }
            
            for (var i = 0; i < btnBoardSquares.Length; i++)
            {
                btnBoardSquares[i].Text = bp[i].ToString();
                if (i == oIndex)
                {
                    btnBoardSquares[i].BackColor = ButtonSquareMoveColor;
                }
                else if (isEnpassant && (player == Player.White && i == dIndex + 8 || player == Player.Black && i == dIndex - 8))
                {
                    btnBoardSquares[i].BackColor = ButtonSquareCapturedColor;
                }
                else if (i == dIndex)
                {
                    if (isCapture && !isEnpassant)
                        btnBoardSquares[i].BackColor = ButtonSquareCapturedColor;
                    else
                        btnBoardSquares[i].BackColor = ButtonSquareMoveColor;
                }
                else if (checkmate && ((lastMove.Player == Player.White && char.ToLower(bp[i]) == 'l') || (lastMove.Player == Player.Black && char.ToLower(bp[i]) == 'k')))
                {
                    btnBoardSquares[i].BackColor = ButtonSquareCheckmatedColor;
                }
                else if (check && ((lastMove.Player == Player.White && char.ToLower(bp[i]) == 'l') || (lastMove.Player == Player.Black && char.ToLower(bp[i]) == 'k')))
                {
                    btnBoardSquares[i].BackColor = ButtonSquareCheckedColor;
                }
                else
                    btnBoardSquares[i].BackColor = ButtonSquareColor;
                btnBoardSquares[i].Refresh();
            }

            txbFen.Text = GameFormLogic.Game.GetFen();
            if (frequentlyRefresh)
            { txbFen.Refresh(); }
        }

        /// <summary>
        /// 更新行动方。
        /// </summary>
        public void UpdateWhoseTurn()
        {
            var wt = GameFormLogic.WhoseTurn;
            lblCurrentPlayer.Text = "当前执棋：" + (wt == Player.White ? "白" : "黑");
            if (frequentlyRefresh)
            { lblCurrentPlayer.Refresh(); }
            SetControlsEnabledAccordingToWhoseTurnAndStatus();
        }

        /// <summary>
        /// 清除所有棋盘格的高亮色。
        /// </summary>
        void CleanSquareColor()
        {
            foreach (var bs in btnBoardSquares)
                bs.BackColor = ButtonSquareColor;
        }
        #endregion

        private void btnStop_Click(object sender, EventArgs e)
        {
            GameFormLogic.Stop();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            cmsAbout.Show(btnAbout, new Point(0, btnAbout.Height));
        }

        private void smiAbout_Click(object sender, EventArgs e)
        {
            new AboutBox.AboutBox { Owner = this }.ShowDialog();
        }

        private void smiQuote_Click(object sender, EventArgs e)
        {
            new QuoteBox { Owner = this }.ShowDialog();
        }

        private void smiSettings_Click(object sender, EventArgs e)
        {
            new SettingsBox().ShowDialog();
        }
    }
}
