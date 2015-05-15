using System.Windows;
using System.Windows.Input;


namespace Files {
    /// <summary>
    /// Interaction logic for TextInputDialog.xaml
    /// </summary>
    public partial class TextInputDialog : Window {
        public TextInputDialog () {
            InitializeComponent ();
        }

        public TextInputDialog (string originalName, string windowTitle) : this () {
            textInputDialogWindow.Title = windowTitle;
            inputTextBox.Text = originalName;            
            inputTextBox.SelectAll ();
        }

        private void closeCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            DialogResult = false;
            Close ();
        }

        private void doneButton_OnClick (object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close ();
        }
    }
}
