using System.Windows.Controls;


namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for Statusbar.xaml
    /// </summary>
    public partial class Statusbar : UserControl {
        public StatusBarModel Model { get; set; }
       
        public Statusbar () {             
            InitializeComponent ();

            Model = new StatusBarModel ();
            DataContext = Model;
        }
    }
}
