using System.Windows;
using System.Windows.Controls.Primitives;


namespace Files {
    /// <summary>
    /// Interaction logic for Clipboard.xaml
    /// </summary>
    public partial class ClipboardWindow : Window {
        public ClipboardWindowViewModel ViewModel { get; private set; }
        

        public ClipboardWindow () {
            InitializeComponent ();

            ViewModel = new ClipboardWindowViewModel ();
            DataContext = ViewModel;
        }


        private void clearButton_OnClick (object sender, RoutedEventArgs e) {
            ViewModel.ClipboardStack.Clear ();
        }

        private void headerThumb_OnDragDelta (object sender, DragDeltaEventArgs e) {
            Left += e.HorizontalChange;
            Top += e.VerticalChange;
        }
    }
}
