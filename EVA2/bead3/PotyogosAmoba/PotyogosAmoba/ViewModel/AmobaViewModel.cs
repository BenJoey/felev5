using System;
using PotyogosAmoba.Model;
using PotyogosAmoba.Persistence;
using System.Collections.ObjectModel;

namespace PotyogosAmoba.ViewModel
{
    /// <summary>
    /// Potyogós Amőba nézetmodell típusa.
    /// </summary>
    public class AmobaViewModel : ViewModelBase
    {
        #region Fields

        private PAmobaModel _model; //játékmodell
        private SaveEntry _selectedgame;
        private String _newName = String.Empty;
        Boolean isPaused;

        #endregion

        #region Properties

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameOpenCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCloseCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameOpenCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCloseCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Játék megállítás parancs lekérdezése.
        /// </summary>
        public DelegateCommand PauseCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<AmobaField> Fields { get; set; }

        /// <summary>
        /// X játékos játékidejének lekérdezése.
        /// </summary>
        public String XTime { get { return TimeSpan.FromSeconds(_model.PlXTime).ToString("g"); } }

        /// <summary>
        /// O játékos játékidejének lekérdezése.
        /// </summary>
        public String OTime { get { return TimeSpan.FromSeconds(_model.Pl0Time).ToString("g"); } }

        /// <summary>
        /// Pályaméret lekérdezése.
        /// </summary>
        public Int32 gameSize
        {
            get { return _model.GetSize; }
            set
            {
                if (gameSize != value)
                {
                    //A játékméret nem lehet nagyobb 30x30-nál illetve kisebb 10-nél
                    Int32 newSize = Convert.ToInt32(value) > 9 && Convert.ToInt32(value) < 31 ? Convert.ToInt32(value) : 10;
                    _model.NewGame(newSize);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Soron lévő játékos lekérdezése.
        /// </summary>
        public String CurrPlay { get { return _model.CurrentPlayer == Player.PlayerX ? "X" : "O"; } }

        /// <summary>
        /// Perzisztens játékállapot mentések lekérdezése.
        /// </summary>
        public ObservableCollection<SaveEntry> Games { get; set; }

        /// <summary>
        /// Kiválasztott játékállapot mentés lekérdezése.
        /// </summary>
        public SaveEntry SelectedGame
        {
            get { return _selectedgame; }
            set
            {
                _selectedgame = value;
                if (_selectedgame != null)
                    NewName = String.Copy(_selectedgame.Name);

                OnPropertyChanged();
                LoadGameCloseCommand.RaiseCanExecuteChanged();
                SaveGameCloseCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Új játék mentés nevének lekérdezése.
        /// </summary>
        public String NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;

                OnPropertyChanged();
                SaveGameCloseCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler<Int32> NewGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler LoadGameOpen;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler<String> LoadGameClose;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler SaveGameOpen;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler<String> SaveGameClose;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;

        /// <summary>
        /// Játék megállítás eseménye.
        /// </summary>
        public event EventHandler GamePause;

        #endregion

        #region Constructors

        /// <summary>
        /// Potyogós Amőba nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public AmobaViewModel(PAmobaModel model)
        {
            _model = model;
            _model.GameOver += new EventHandler<AmobaEvent>(Model_GameOver);
            _model.Refresh += new EventHandler(Model_GameAdvanced);
            _model.Reset += new EventHandler(Model_Reset);

            NewGameCommand = new DelegateCommand(param => OnNewGame(Convert.ToInt32(param)));
            LoadGameOpenCommand = new DelegateCommand(async param =>
            {
                Games = new ObservableCollection<SaveEntry>(await _model.ListGamesAsync());
                OnLoadGameOpen();
            });
            LoadGameCloseCommand = new DelegateCommand(
                param => SelectedGame != null, // parancs végrehajthatóságának feltétele
                param => { OnLoadGameClose(SelectedGame.Name); });
            SaveGameOpenCommand = new DelegateCommand(async param =>
            {
                Games = new ObservableCollection<SaveEntry>(await _model.ListGamesAsync());
                OnSaveGameOpen();
            });
            SaveGameCloseCommand = new DelegateCommand(
                param => NewName.Length > 0, // parancs végrehajthatóságának feltétele
                param => { OnSaveGameClose(NewName); });
            ExitCommand = new DelegateCommand(param => OnExitGame());
            PauseCommand = new DelegateCommand(param => OnGamePause());

            isPaused = false;

            ResetFields();

            RefreshTable();
        }

        #endregion

        #region Private methods

        /// <summary>
		/// Tábla frissítése.
		/// </summary>
        private void RefreshTable()
        {
            foreach(AmobaField curr in Fields)
            {
                curr.Text = _model.GetFieldValue(curr.X, curr.Y) == Player.NoPlayer ? String.Empty : _model.GetFieldValue(curr.X, curr.Y) == Player.PlayerX ? "X" : "O";
                curr.Clickable = _model.IsFieldActive(curr.X, curr.Y);
            }
        }

        /// <summary>
		/// Tábla újragenerálása.
		/// </summary>
        private void ResetFields()
        {
            Fields = new ObservableCollection<AmobaField>();
            for (Int32 i = 0; i < _model.GetSize; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.GetSize; j++)
                {
                    Fields.Add(new AmobaField
                    {
                        Clickable = false,
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        isWinField = false,
                        ButtonSize = (35 - gameSize) > 10 ? (35 - gameSize) : 10,
                        Number = i * _model.GetSize + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields");
        }

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="Index">A lépett mező indexe.</param>
        private void StepGame(Int32 Index)
        {
            AmobaField clicked = Fields[Index];
            if (clicked.Clickable && !isPaused)
            {
                clicked.Text = CurrPlay;
                _model.Step(clicked.X, clicked.Y);
                OnPropertyChanged("CurrPlay");
            }
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, AmobaEvent e)
        {
            foreach (AmobaField field in Fields)
            {
                field.Clickable = false; // minden mezőt lezárunk
            }
            foreach(Tuple<Int32, Int32> a in e.WinPlace)
            {
                Int32 No = a.Item1 * gameSize + a.Item2;
                Fields[No].isWinField = true;
            }
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_GameAdvanced(object sender, EventArgs e)
        {
            RefreshTable();
            OnPropertyChanged("OTime");
            OnPropertyChanged("XTime");
        }

        /// <summary>
        /// Játék újrakezdésének eseménykezelője.
        /// </summary>
        private void Model_Reset(object sender, EventArgs e)
        {
            OnPropertyChanged("gameSize");
            OnPropertyChanged("CurrPlay");
            ResetFields();
            RefreshTable();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame(Int32 newSize)
        {
            if (NewGame != null)
                NewGame(this, newSize);
        }

        /// <summary>
        /// Játék betöltés választásának eseménykiváltása.
        /// </summary>
        private void OnLoadGameOpen()
        {
            if (LoadGameOpen != null)
                LoadGameOpen(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék betöltésének eseménykiváltása.
        /// </summary>
        private void OnLoadGameClose(String name)
        {
            if (LoadGameClose != null)
                LoadGameClose(this, name);
        }

        /// <summary>
        /// Játék mentés választásának eseménykiváltása.
        /// </summary>
        private void OnSaveGameOpen()
        {
            if (SaveGameOpen != null)
                SaveGameOpen(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentésének eseménykiváltása.
        /// </summary>
        private void OnSaveGameClose(String name)
        {
            if (SaveGameClose != null)
                SaveGameClose(this, name);
        }

        /// <summary>
        /// Játék megállításának eseménykiváltása.
        /// </summary>
        private void OnGamePause()
        {
            isPaused = !isPaused;
            if (GamePause != null)
                GamePause(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        #endregion
    }
}
