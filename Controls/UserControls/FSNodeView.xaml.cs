using System.Windows.Controls;
using FSOps;


namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for FSNodeView.xaml
    /// </summary>
    public partial class FSNodeView : ContentControl {
        #region props
        public FSNodeViewModel ViewModel { get; private set; }
        #endregion

        #region ctors
        private FSNodeView () {
            InitializeComponent ();

            ViewModel = new FSNodeViewModel ();
            DataContext = ViewModel;
        }

        public FSNodeView (FSNode fsNode) : this () {
            ViewModel.FSNode = fsNode;
        }
        #endregion
    }
}
