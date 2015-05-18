using System.Collections.Generic;
using System.Windows;
using Controls.Layouts;
using FSOps;


namespace FilesApplication {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public List<MainWindow> AppWindows { get; private set; }


        public App () {
            AppWindows = new List<MainWindow> (1);
        }


        private void App_OnStartup (object sender, StartupEventArgs e) {
            AppWindows.Add (new MainWindow ());
            InitWindow (0);
        }

        private void App_OnExit (object sender, ExitEventArgs e) { }


        private void InitWindow (int index) {
            ReloadContents (index);
            AppWindows[index].Show ();
        }

        // TODO: remember last visited dirs in settings
        private void ReloadContents (int index) {
            AppWindows[index].ViewModel.Layouts.Add (new MillerColumnsLayout (new SystemRootNode ()));
            AppWindows[index].ViewModel.Layouts.Add (new MillerColumnsLayout (new SystemRootNode ()));
        }
    }
}
