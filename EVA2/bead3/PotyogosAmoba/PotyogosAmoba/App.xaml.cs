using PotyogosAmoba.Model;
using PotyogosAmoba.Persistence;
using PotyogosAmoba.ViewModel;
using PotyogosAmoba.View;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Linq;
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
        private LoadWindow _loadWindow;
        private SaveWindow _saveWindow;
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
            IAmobaDataAccess dataAccess;
            //dataAccess = new AmobaFileDataAccess(AppDomain.CurrentDomain.BaseDirectory); // fájl alapú mentés
            dataAccess = new AmobaDbDataAccess("name=AmobaMdoel"); //adatbázis alapú mentés

            _model = new PAmobaModel(dataAccess);
            _model.GameOver += new EventHandler<AmobaEvent>(Model_GameOver);
            _model.NewGame(10);

            // nézemodell létrehozása
            _viewModel = new AmobaViewModel(_model);
            _viewModel.NewGame += new EventHandler<Int32>(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGameOpen += new EventHandler(ViewModel_LoadGameOpen);
            _viewModel.LoadGameClose += new EventHandler<String>(ViewModel_LoadGameClose);
            _viewModel.SaveGameOpen += new EventHandler(ViewModel_SaveGameOpen);
            _viewModel.SaveGameClose += new EventHandler<String>(ViewModel_SaveGameClose);
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

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Új táblaméret</param>
        private void ViewModel_NewGame(object sender, Int32 e)
        {
            _timer.Stop();
            _model.NewGame(e);
            _timer.Start();
        }

        /// <summary>
        /// Játék megállításának eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_GamePause(object sender, EventArgs e)
        {
            if (_timer.IsEnabled) _timer.Stop();
            else _timer.Start();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_LoadGameOpen(Object sender, EventArgs e)
        {
            _timer.Stop();
            _viewModel.SelectedGame = null; // kezdetben nincsen kiválasztott elem

            _loadWindow = new LoadWindow(); // létrehozzuk a játék állapot betöltő ablakot
            _loadWindow.DataContext = _viewModel;
            _loadWindow.ShowDialog(); // megjelenítjük dialógusként
            _timer.Start();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGameClose(object sender, String name)
        {
            if (name != null)
            {
                try
                {
                    await _model.LoadGame(name);
                }
                catch
                {
                    MessageBox.Show("Játék betöltése sikertelen!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _loadWindow.Close(); // játékállapot betöltőtő ablak bezárása
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_SaveGameOpen(Object sender, EventArgs e)
        {
            _timer.Stop();
            _viewModel.SelectedGame = null; // kezdetben nincsen kiválasztott elem
            _viewModel.NewName = String.Empty;

            _saveWindow = new SaveWindow(); // létrehozzuk a játék állapot mentő ablakot
            _saveWindow.DataContext = _viewModel;
            _saveWindow.ShowDialog(); // megjelenítjük dialógusként

            _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGameClose(object sender, String name)
        {
            if (name != null)
            {
                try
                {
                    // felülírás ellenőrzése
                    var games = await _model.ListGamesAsync();
                    if (games.All(g => g.Name != name) ||
                        MessageBox.Show("Biztos, hogy felülírja a meglévő mentést?", "PotyogosAmoba",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _model.SaveGame(name);
                    }
                }
                catch
                {
                    MessageBox.Show("Játék mentése sikertelen!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _saveWindow.Close(); // játékállapot mentő ablak bezárása
        }

        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="e">Amőba esemény típus a játék végéhez tartozó információkkal</param>
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
