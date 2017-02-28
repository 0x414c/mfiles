using FSOps;


namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for FSNodeView.xaml
    /// </summary>
    public partial class FSNodeView {
        #region props
        public FSNodeViewModel ViewModel { get; }
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
