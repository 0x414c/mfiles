using System.Windows;
using System.Windows.Threading;


namespace Files {
    /// <summary>
    /// Interaction logic for Clipboard.xaml
    /// </summary>
    public partial class ClipboardWindow : Window {
        public ClipboardWindowModel ViewModel { get; set; }
        
        public ClipboardWindow () {
            InitializeComponent ();

            ViewModel = new ClipboardWindowModel ();
            DataContext = ViewModel;
        }

        private void clipboardWindow_OnClosing (object sender, System.ComponentModel.CancelEventArgs e) {
            //Application.Current.Dispatcher.Invoke (Hide);

            //if (Equals (sender, this)) {
            //    e.Cancel = true;
            //}
        }

        private void clearButton_OnClick (object sender, RoutedEventArgs e) {
            ViewModel.ClipboardStack.Clear ();
        }
    }
}
