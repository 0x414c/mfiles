using System.Windows;
using System.Windows.Controls.Primitives;


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

        private void clearButton_OnClick (object sender, RoutedEventArgs e) {
            ViewModel.ClipboardStack.Clear ();
        }

        private void headerThumb_OnDragDelta (object sender, DragDeltaEventArgs e) {
            Left = Left + e.HorizontalChange;
            Top = Top + e.VerticalChange;
        }
    }
}
