using System;
using ZH.Model;
using System.Collections.ObjectModel;

namespace ZH.ViewModel
{
    /// <summary>
    /// Potyogós Amőba nézetmodell típusa.
    /// </summary>
    public class ZHViewModel : ViewModelBase
    {
        #region Fields

        private ZHModel _model; //játékmodell
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
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

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
        public ObservableCollection<GameField> Fields { get; set; }

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
                    Int32 newSize = Convert.ToInt32(value) == 7 || Convert.ToInt32(value) == 9 || Convert.ToInt32(value) == 11 ? Convert.ToInt32(value) : 7;
                    _model.NewGame(newSize);
                    OnPropertyChanged();
                }
            }
        }

        public String Time { get { return TimeSpan.FromSeconds(_model.GetTime).ToString("g"); } }
        public Int32 VisitedKonzik { get { return _model.KonziNum; } }

        #endregion

        #region Events

        public event EventHandler<Int32> NewGame;

        public event EventHandler LoadGame;

        public event EventHandler SaveGame;

        public event EventHandler ExitGame;

        public event EventHandler GamePause;

        #endregion

        #region Constructors

        /// <summary>
        /// Potyogós Amőba nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public ZHViewModel(ZHModel model)
        {
            _model = model;
            _model.Refresh += new EventHandler(Model_GameAdvanced);
            _model.Reset += new EventHandler(Model_Reset);

            NewGameCommand = new DelegateCommand(param => OnNewGame(Convert.ToInt32(param)));
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            PauseCommand = new DelegateCommand(param => OnGamePause());

            isPaused = false;

            ResetFields();

            RefreshTable();
        }

        #endregion

        #region Private methods

        private void RefreshTable()
        {
            foreach(GameField curr in Fields)
            {
                String text = String.Empty, Color = String.Empty;
                switch (_model.GetFieldValue(curr.X, curr.Y))
                {
                    case Mezo.Hallgato: text = "H"; Color = "G"; break;
                    case Mezo.Nothing: text = ""; Color = "W"; break;
                    case Mezo.Nyilt: text = "A"; Color = "Y"; break;
                    case Mezo.Zart: text = "A"; Color = "R"; break;
                }
                curr.Text = text; curr.Color = Color;
                curr.Clickable = _model.IsFieldClickable(curr.X, curr.Y);
            }
        }

        private void ResetFields()
        {
            Fields = new ObservableCollection<GameField>();
            for (Int32 i = 0; i < 5; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.GetSize; j++)
                {
                    Fields.Add(new GameField
                    {
                        Clickable = false,
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Number = i * _model.GetSize + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields");
        }

        private void StepGame(Int32 Index)
        {
            GameField clicked = Fields[Index];
            if (clicked.Clickable && !isPaused)
            {
                _model.Step(clicked.X, clicked.Y);
                RefreshTable();
                OnPropertyChanged("Fields");
            }
        }

        #endregion

        #region Game event handlers

        private void Model_GameAdvanced(object sender, EventArgs e)
        {
            RefreshTable();
            //OnPropertyChanged("Fields");
            OnPropertyChanged("Time");
            OnPropertyChanged("VisitedKonzik");
        }

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
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
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
