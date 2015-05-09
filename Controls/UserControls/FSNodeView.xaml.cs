using System.Windows.Controls;
using FSOps;


namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for FSNodeView.xaml
    /// </summary>
    public partial class FSNodeView : ContentControl {
        public FSNodeViewModel ViewModel { get; private set; }

        private FSNodeView () {
            InitializeComponent ();
        }

        public FSNodeView (FSNode fsNode) : this () {
            ViewModel = new FSNodeViewModel (fsNode);
            DataContext = ViewModel;
        }
    }
}
