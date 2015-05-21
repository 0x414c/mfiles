using System.Collections.Generic;
using System.Windows;
using Controls.Layouts;
using Files;
using FSOps;


namespace FilesApplication {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private DependencyObject[] _appWindows;

        public DependencyObject[] AppWindows {
            get { return _appWindows; }
            private set { _appWindows = value; }
        }

        public PropertiesManager PropertiesManager { get; private set; }
        

        public App () {
            AppWindows = new DependencyObject[1];
            PropertiesManager = new PropertiesManager ();
        }


        private void App_OnStartup (object sender, StartupEventArgs e) {
            AppWindows[0] = new MainWindow ();
            PropertiesManager.Restore (
                ref _appWindows, 0, "windowLayoutSettings", 
                new List<string>(4) { "Top", "Left", "Width", "Height", "WindowState" }
            );
            InitWindow (0);
        }

        private void App_OnExit (object sender, ExitEventArgs e) {
            PropertiesManager.Save (
                ref _appWindows, 0, "windowLayoutSettings", 
                new List<string> (4) { "Top", "Left", "Width", "Height", "WindowState" }
            );
            PropertiesManager.Dispose ();
        }


        private void InitWindow (int index) {
            ReloadContents (index);
            var window = AppWindows[index] as Window;
            if (window != null) {
                window.Show ();
            }
        }

        // TODO: remember last visited dirs in settings
        private void ReloadContents (int index) {
            var mainWindow = AppWindows[index] as MainWindow;
            if (mainWindow != null) {
                mainWindow.ViewModel.Layouts.Add (new MillerColumnsLayout (new SystemRootNode ()));
                mainWindow.ViewModel.Layouts.Add (new MillerColumnsLayout (new SystemRootNode ()));
            }
        }
    }
}
