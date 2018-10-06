using System;
using System.Drawing;
using System.Windows.Forms;
using PotyogosAmoba.Persistence;
using PotyogosAmoba.Model;

namespace PotyogosAmoba
{
    public partial class GameForm : Form
    {
        #region Fields

        private IAmobaDataAccess _dataaccess;
        private PAmobaModel _model;
        private Button[,] gameBoard;
        private Timer _timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Játékablak betöltése.
        /// </summary>
        public GameForm()
        {
            InitializeComponent();

            _dataaccess = new AmobaFileDataAccess();
            _model = new PAmobaModel(_dataaccess);
            _model.GameOver += new EventHandler<AmobaEvent>(Game_gameover);
            _model.RefreshBoard += new EventHandler(Refresh);


            _timer = new Timer();
            _timer.Interval = 1000;

            _timer.Tick += new EventHandler(Timer_Tick);
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék befejezésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_gameover(Object sender, AmobaEvent e)
        {
            _timer.Stop();

            _menuFileSaveGame.Enabled = false;

            foreach (Button b in gameBoard)
                b.Enabled = false;
            if (e.WhoWon != Player.NoPlayer)
            {
                foreach (Tuple<Int32, Int32> a in e.WinPlace)
                    gameBoard[a.Item1, a.Item2].BackColor = Color.Yellow;
                String WinnerPlayer = e.WhoWon == Player.PlayerX ? "X" : "O";
                MessageBox.Show("Játék vége!" + Environment.NewLine + WinnerPlayer + " nyerte a játékot!" + Environment.NewLine +
                                "X játékos ideje: " + TimeSpan.FromSeconds(e.GetXTime).ToString("g") + Environment.NewLine +
                                "O játékos ideje: " + TimeSpan.FromSeconds(e.Get0Time).ToString("g"), "Potyogós Amőba", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Játék vége!" + Environment.NewLine + "A játék döntetlen lett!" + Environment.NewLine +
                                "X játékos ideje: " + TimeSpan.FromSeconds(e.GetXTime).ToString("g") + Environment.NewLine +
                                "O játékos ideje: " + TimeSpan.FromSeconds(e.Get0Time).ToString("g"), "Potyogós Amőba", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /// <summary>
        /// Játékmező frissítésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(Object sender, EventArgs e){ SetupTable(); }

        #endregion

        #region Menu event handlers

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoadGame_Click(Object sender, EventArgs e)
        {
            _timer.Stop();

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await _model.LoadGame(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (AmobaDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame(10);
                }
                GenerateTable();
            }
            _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveGame_Click(Object sender, EventArgs e)
        {
            _timer.Stop();

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játék mentése
                    await _model.SaveGame(_saveFileDialog.FileName);
                }
                catch (AmobaDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            _timer.Start();
        }

        /// <summary>
        /// Kilépés eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(Object sender, EventArgs e)
        {
            _timer.Stop();

            // megkérdezzük, hogy biztos ki szeretne-e lépni
            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Sudoku játék", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // ha igennel válaszol
                Close();
            }
            else
            {
                _timer.Start();
            }
        }

        #endregion

        #region Form event handlers

        /// <summary>
        /// Játék indításának eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table_Gen_Click(object sender, EventArgs e)
        {
            _model.NewGame(Convert.ToInt32(_tableSize.Value));
            GenerateTable();
            if (_timer.Enabled) _timer.Stop();
            _PauseButton.Visible = true;
            _timer.Start();
        }

        /// <summary>
        /// Játék megállításának eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(Object sender, EventArgs e)
        {
            _model.AdvanceTime();
            _XTIMEDISPLAY.Text = TimeSpan.FromSeconds(_model.PlXTime).ToString("g");
            _0TIMEDISPLAY.Text = TimeSpan.FromSeconds(_model.Pl0Time).ToString("g");
        }

        #endregion

        #region Button Grid event handlers

        /// <summary>
        /// Játék oszlopra kattintás eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGrid_Click(Object sender, MouseEventArgs e)
        {
            if (_timer.Enabled)
            {
                Int32 RowInd = (sender as Button).TabIndex / _model.GetSize;
                Int32 ColumnInd = (sender as Button).TabIndex % _model.GetSize;
                if (RowInd != 0) // Ha nem az oszlop legfelső gombja akkor bekapcsoljuk a felette l.évő gombot
                    gameBoard[RowInd - 1, ColumnInd].Enabled = true;
                _model.Step(RowInd, ColumnInd);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Új tábla létrehozása.
        /// </summary>
        private void GenerateTable()
        {
            DeleteBoard();
            _menuFileSaveGame.Enabled = true;

            _gameDisplayTable.RowCount = _gameDisplayTable.ColumnCount = _model.GetSize;
            gameBoard = new Button[_model.GetSize, _model.GetSize];
            for (Int32 i = 0; i < _model.GetSize; i++)
                for (Int32 j = 0; j < _model.GetSize; j++)
                {
                    gameBoard[i, j] = new Button();
                    gameBoard[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    gameBoard[i, j].Size = new Size(30, 30);
                    gameBoard[i, j].TabIndex = i * _model.GetSize + j;
                    gameBoard[i, j].Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                    gameBoard[i, j].MouseClick += new MouseEventHandler(ButtonGrid_Click);
                    gameBoard[i, j].FlatStyle = FlatStyle.Flat;
                    if (i == _model.GetSize - 1 || (_model.GetFieldValue(i, j) == Player.NoPlayer && _model.GetFieldValue(i + 1, j) != Player.NoPlayer))
                        gameBoard[i, j].Enabled = true;
                    else
                        gameBoard[i, j].Enabled = false;
                    _gameDisplayTable.Controls.Add(gameBoard[i, j], j, i);
                }
            SetupTable();
        }

        /// <summary>
        /// Tábla beállítása.
        /// </summary>
        private void SetupTable()
        {
            foreach(Button b in gameBoard)
            {
                Int32 x = b.TabIndex / _model.GetSize;
                Int32 y = b.TabIndex % _model.GetSize;
                switch(_model.GetFieldValue(x,y))
                {
                    case Player.PlayerX:
                        b.Text = "X";
                        b.BackColor = Color.Pink;
                        b.Enabled = false;
                        break;
                    case Player.Player0:
                        b.Text = "O";
                        b.BackColor = Color.LightGreen;
                        b.Enabled = false;
                        break;
                    default:
                        b.Text = "";
                        b.BackColor = Color.White;
                        break;
                }
                if (b.Enabled)
                    b.BackColor = Color.Cyan;
            }
            CurrentPlayerDisplay.Text = _model.CurrentPlayer == Player.PlayerX ? "X" : "O";
        }

        /// <summary>
        /// Tábla és gombok törlése az interfészről
        /// </summary>
        private void DeleteBoard()
        {
            if (gameBoard != null)
            {
                foreach (Button b in gameBoard)
                    _gameDisplayTable.Controls.Remove(b);
            }
        }

        #endregion
    }
}
