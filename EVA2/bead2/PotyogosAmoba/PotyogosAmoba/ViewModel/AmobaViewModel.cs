using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotyogosAmoba.Model;
using System.Collections.ObjectModel;

namespace PotyogosAmoba.ViewModel
{
    public class AmobaViewModel : ViewModelBase
    {
        #region Fields

        private PAmobaModel _model;
        Boolean isPaused;

        #endregion

        #region Properties

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public ObservableCollection<AmobaField> Fields { get; set; }

        public String XTime { get { return TimeSpan.FromSeconds(_model.PlXTime).ToString("g"); } }
        public String OTime { get { return TimeSpan.FromSeconds(_model.Pl0Time).ToString("g"); } }
        public Int32 gameSize
        {
            get { return _model.GetSize; }
            set
            {
                if (gameSize != value)
                {
                    _model.NewGame(Convert.ToInt32(value));
                    OnPropertyChanged();
                }
            }
        }
        public String CurrPlay { get { return _model.CurrentPlayer == Player.PlayerX ? "X" : "O"; } }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler<Int32> NewGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler SaveGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;

        /// <summary>
        /// Játék megállítás eseménye.
        /// </summary>
        public event EventHandler GamePause;

        #endregion

        public AmobaViewModel(PAmobaModel model)
        {
            _model = model;
            isPaused = false;
            _model.GameOver += new EventHandler<AmobaEvent>(Model_GameOver);
            _model.RefreshBoard += new EventHandler(Model_GameAdvanced);
            _model.Reset += new EventHandler(Model_Reset);

            NewGameCommand = new DelegateCommand(param => OnNewGame(Convert.ToInt32(param)));
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            PauseCommand = new DelegateCommand(param => OnGamePause());

            ResetFields();

            RefreshTable();
        }

        #region Private methods

        private void RefreshTable()
        {
            foreach(AmobaField curr in Fields)
            {
                curr.Text = _model.GetFieldValue(curr.X, curr.Y) == Player.NoPlayer ? String.Empty : _model.GetFieldValue(curr.X, curr.Y) == Player.PlayerX ? "X" : "O";
                curr.Clickable = _model.isFieldActive(curr.X, curr.Y);
            }

            OnPropertyChanged("OTime");
            OnPropertyChanged("XTime");
        }

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
                        Number = i * _model.GetSize + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields");
        }

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
        }

        private void Model_Reset(object sender, EventArgs e)
        {
            Fields.Clear();
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
