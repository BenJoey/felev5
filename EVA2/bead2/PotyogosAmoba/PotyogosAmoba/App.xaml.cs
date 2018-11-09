using PotyogosAmoba.Model;
using PotyogosAmoba.Persistence;
using PotyogosAmoba.ViewModel;
using PotyogosAmoba.View;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace PotyogosAmoba
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private PAmobaModel _model;
        private AmobaViewModel _viewModel;
        private MainWindow _view;
        private DispatcherTimer _timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new PAmobaModel(new AmobaFileDataAccess());
            _model.GameOver += new EventHandler<AmobaEvent>(Model_GameOver);
            _model.NewGame(10);

            // nézemodell létrehozása
            _viewModel = new AmobaViewModel(_model);
            _viewModel.NewGame += new EventHandler<Int32>(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.GamePause += new EventHandler(ViewModel_GamePause);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

            // időzítő létrehozása
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        #endregion

        #region View event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object sender, Int32 e)
        {
            _timer.Stop();
            _model.NewGame(e);
            _timer.Start();
        }

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_GamePause(object sender, EventArgs e)
        {
            if (_timer.IsEnabled) _timer.Stop();
            else _timer.Start();
        }

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>
        private void View_Closing(object sender, CancelEventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "PotyogosAmoba", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást

                if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                    _timer.Start();
            }
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewModel_LoadGame(Object sender, EventArgs e)
        {
            _timer.Stop();
            try
            {
                OpenFileDialog _openFileDialog = new OpenFileDialog(); // dialógusablak
                _openFileDialog.Title = "Amőba játék betöltése";
                _openFileDialog.Filter = "Amőba Save File|*.sav";
                if (_openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGame(_openFileDialog.FileName);
                }
            }
            catch (AmobaDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "PotyogosAmoba", MessageBoxButton.OK, MessageBoxImage.Error);

                _model.NewGame(10);
            }
            _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewModel_SaveGame(Object sender, EventArgs e)
        {
            _timer.Stop();
            try
            {
                SaveFileDialog _saveFileDialog = new SaveFileDialog(); // dialógablak
                _saveFileDialog.Title = "Amőba játék mentése";
                _saveFileDialog.Filter = "Amőba Save File|*.sav";
                if (_saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGame(_saveFileDialog.FileName);
                    }
                    catch (AmobaDataException)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (AmobaDataException)
            {
                MessageBox.Show("A fájl mentése sikertelen!", "PotyogosAmoba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _timer.Start();
        }

        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék befejezésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_GameOver(Object sender, AmobaEvent e)
        {
            _timer.Stop();
            if (e.WhoWon != Player.NoPlayer)
            {
                String WinnerPlayer = e.WhoWon == Player.PlayerX ? "X" : "O";
                MessageBox.Show("Játék vége!" + Environment.NewLine + WinnerPlayer + " nyerte a játékot!" + Environment.NewLine +
                                "X játékos ideje: " + TimeSpan.FromSeconds(e.GetXTime).ToString("g") + Environment.NewLine +
                                "O játékos ideje: " + TimeSpan.FromSeconds(e.Get0Time).ToString("g"), "Potyogós Amőba", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Játék vége!" + Environment.NewLine + "A játék döntetlen lett!" + Environment.NewLine +
                                "X játékos ideje: " + TimeSpan.FromSeconds(e.GetXTime).ToString("g") + Environment.NewLine +
                                "O játékos ideje: " + TimeSpan.FromSeconds(e.Get0Time).ToString("g"), "Potyogós Amőba", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        #endregion
    }
}
