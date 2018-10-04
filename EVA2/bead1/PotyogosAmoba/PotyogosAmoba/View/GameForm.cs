using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PotyogosAmoba.Model;

namespace PotyogosAmoba
{
    public partial class GameForm : Form
    {
        #region Fields

        private PAmobaModel _model;
        private Label[,] gameBoard;
        private Button[] gameButtons;
        private Timer _timer;

        #endregion

        #region Constructors

        public GameForm()
        {
            InitializeComponent();

            _model = new PAmobaModel();
            _model.GameOver += new EventHandler<AmobaEvent>(Game_gameover);

            _timer = new Timer();
            _timer.Interval = 1000;

            _timer.Tick += new EventHandler(Timer_Tick);
        }

        #endregion

        #region Game event handlers

        private void Game_gameover(Object sender, AmobaEvent e)
        {
            _timer.Stop();

            foreach (Button b in gameButtons)
                b.Enabled = false;
            if (e.WhoWon != Player.NoPlayer)
            {
                foreach (Tuple<Int32, Int32> a in e.WinPlace)
                    gameBoard[a.Item1, a.Item2].BackColor = Color.Yellow;
                String WinnerPlayer = e.WhoWon == Player.PlayerX ? "X" : "O";
                MessageBox.Show("Játék vége!" + Environment.NewLine + WinnerPlayer + " nyerte a játékot!" + Environment.NewLine +
                                "X játékos ideje: " + TimeSpan.FromSeconds(e.GetXTime).ToString("g") + Environment.NewLine +
                                "O játékos ideje: " + TimeSpan.FromSeconds(e.Get0Time).ToString("g"), "PA", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Játék vége!" + Environment.NewLine + "A játék döntetlen lett!" + Environment.NewLine +
                                "X játékos ideje: " + TimeSpan.FromSeconds(e.GetXTime).ToString("g") + Environment.NewLine +
                                "O játékos ideje: " + TimeSpan.FromSeconds(e.Get0Time).ToString("g"), "PA", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        #endregion

        #region Form event handlers

        private void Table_Gen_Click(object sender, EventArgs e)
        {
            DeleteBoard();

            _model.NewGame(Convert.ToInt32(_tableSize.Value));
            GenerateTable();
            if (_timer.Enabled) _timer.Stop();
            _PauseButton.Visible = true;
            _timer.Start();
        }

        private void PauseButtonHandler(Object sender, EventArgs e)
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                _PauseButton.Text = "Start";
            }
            else
            {
                _timer.Start();
                _PauseButton.Text = "Pause";
            }
        }
            #endregion

        #region Timer event handlers

        /// <summary>
        /// Időzítő eseménykeztelője.
        /// </summary>
        private void Timer_Tick(Object sender, EventArgs e)
        {
            _model.AdvanceTime();
            _XTIMEDISPLAY.Text = TimeSpan.FromSeconds(_model.PlXTime).ToString("g");
            _0TIMEDISPLAY.Text = TimeSpan.FromSeconds(_model.Pl0Time).ToString("g");
        }

        #endregion

        #region Button Row event handlers

        private void ButtonRow_Click(Object sender, MouseEventArgs e)
        {
            if (_timer.Enabled)
            {
                _model.Step((sender as Button).TabIndex);
                SetupTable();
                _model.GameCheck();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Új tábla létrehozása.
        /// </summary>
        private void GenerateTable()
        {
            _ButtonRow2.ColumnCount = _model.GetSize;
            _gameDisplayTable.RowCount = _gameDisplayTable.ColumnCount = _model.GetSize;
            gameButtons = new Button[_model.GetSize];
            gameBoard = new Label[_model.GetSize, _model.GetSize];
            for (Int32 i = 0; i < _model.GetSize; i++)
            {
                gameButtons[i] = new Button();
                gameButtons[i].FlatStyle = FlatStyle.Flat;
                gameButtons[i].TabIndex = i;
                gameButtons[i].Size = new Size(30, 30);
                gameButtons[i].MouseClick += new MouseEventHandler(ButtonRow_Click);
                _ButtonRow2.Controls.Add(gameButtons[i], i, 1);
                for (Int32 j = 0; j < _model.GetSize; j++)
                {
                    gameBoard[j, i] = new Label();
                    gameBoard[j, i].BorderStyle = BorderStyle.FixedSingle;
                    gameBoard[j, i].TextAlign = ContentAlignment.MiddleCenter;
                    gameBoard[j, i].Size = new Size(30, 30);
                    gameBoard[j, i].Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    gameBoard[j, i].FlatStyle = FlatStyle.Flat;
                    _gameDisplayTable.Controls.Add(gameBoard[j, i], j, i);
                }
            }
            SetupTable();
        }

        /// <summary>
        /// Tábla beállítása.
        /// </summary>
        private void SetupTable()
        {
            for (Int32 i = 0; i < _model.GetSize; i++)
            {
                if (_model.GetFieldValue(i, 0) != Player.NoPlayer)
                {
                    gameButtons[i].Enabled = false; //Ha egy oszlop megtelt akkor már nem lehet a gombjára kattintani
                    gameButtons[i].BackColor = Color.Black;
                }
                else gameButtons[i].BackColor = Color.Cyan;
                for (Int32 j = 0; j < _model.GetSize; j++)
                {
                    switch (_model.GetFieldValue(i, j))
                    {
                        case Player.PlayerX:
                            gameBoard[i, j].Text = "X";
                            gameBoard[i, j].BackColor = Color.Pink;
                            break;
                        case Player.Player0:
                            gameBoard[i, j].Text = "O";
                            gameBoard[i, j].BackColor = Color.LightGreen;
                            break;
                        default:
                            gameBoard[i, j].Text = "";
                            gameBoard[i, j].BackColor = Color.White;
                            break;
                    }
                }
            }
            CurrentPlayerDisplay.Text = _model.CurrentPlayer == Player.PlayerX ? "X" : "O";
        }

        /// <summary>
        /// Tábla és gombok törlése az interfészről
        /// </summary>
        private void DeleteBoard()
        {
            if (gameButtons != null)
            {
                foreach (Button a in gameButtons)
                    _ButtonRow2.Controls.Remove(a);
                foreach (Label l in gameBoard)
                    _gameDisplayTable.Controls.Remove(l);
            }
        }

        #endregion
    }
}
