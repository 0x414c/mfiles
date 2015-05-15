using System.Collections.Generic;
using System.Windows;
using Controls.Layouts;
using FSOps;


namespace FilesApplication {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: Application {
        private List<MainWindow> AppWindows { get; set; }

        public App () {
            AppWindows = new List<MainWindow> (1);
        }

        private void Bootstrap (object sender, StartupEventArgs e) {
            AppWindows.Add (new MainWindow ());
            InitWindow (0);
        }

        private void InitWindow (int index) {
            ReloadContents (index);
            AppWindows[index].Show ();
        }

        // TODO: remember last visited dirs in settings
        private void ReloadContents (int index) {
            //AppWindows[index].ViewModel.Layouts.Add (new MillerColumnsLayout (new DirectoryNode (@"C:\Windows\System32")));
            //AppWindows[index].ViewModel.Layouts.Add (new MillerColumnsLayout (new FileNode (@"C:\interbase.log")));
            AppWindows[index].ViewModel.Layouts.Add (new MillerColumnsLayout (new SystemRootNode ()));
            AppWindows[index].ViewModel.Layouts.Add (new MillerColumnsLayout (new SystemRootNode ()));
        }
    }
}
