using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            ReloadContents (0);
            AppWindows[0].Show ();
        }

        // TODO: remember last visited dirs in settings
        private void ReloadContents (int index) {
            AppWindows[index].Model.Layouts.Add (new MillerColumnsLayout (new SystemRootNode ()));
            AppWindows[index].Model.Layouts.Add (new MillerColumnsLayout (new SystemRootNode ()));

            AppWindows[index].Model.Data.Add (new TextBlock { Text = "1" });
            AppWindows[index].Model.Data.Add (new TextBlock { Text = "2" });
        }
    }
}
