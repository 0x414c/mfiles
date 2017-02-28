using System.Windows;
using System.Windows.Input;


namespace Files {
    /// <summary>
    /// Interaction logic for TextInputDialog.xaml
    /// </summary>
    public partial class TextInputDialog {
        private string Result {
            get { return (string) GetValue (ResultProperty); }
            set { SetValue (ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register (
                "Result", typeof (string),
                typeof (TextInputDialog), new PropertyMetadata ("<result is n/a>")
            );


        public TextInputDialog () {
            InitializeComponent ();
        }


        public TextInputDialog (string defaultText, string windowTitle) : this () {
            textInputDialogWindow.Title = windowTitle;
            Result = defaultText;
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
