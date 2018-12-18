using ZH.Model;
using ZH.ViewModel;
using ZH.View;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace ZH
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private ZHModel _model;
        private ZHViewModel _viewModel;
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
            _model = new ZHModel();
            _model.NewGame(7);

            // nézemodell létrehozása
            _viewModel = new ZHViewModel(_model);
            _viewModel.NewGame += new EventHandler<Int32>(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
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

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "ZH", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
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
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        #endregion
    }
}
