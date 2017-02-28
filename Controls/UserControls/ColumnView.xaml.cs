using System.Windows.Controls;
using System.Windows.Input;
using Controls.Auxiliary;
using Controls.Layouts;
using FSOps;


namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for ColumnView.xaml
    /// </summary>
    public partial class ColumnView {
        #region fields & props
        public ColumnViewModel ViewModel { get; }

        public string Title => ViewModel.ParentFSNode.ToString ();

        public int ItemsCount => ViewModel.ChildFSNodesViews.Count;

        public int ViewId { get; }

        public int SelectionSize {
            get {
                if (ViewModel.ParentFSNode.Is (TypeTag.Root | TypeTag.SubRoot | TypeTag.Internal)) {
                    var childFSNodesListView = Utils.FindVisualChild<ListView> (columnViewContentControl);

                    if (childFSNodesListView != null) {
                        return childFSNodesListView.SelectedItems.Count;
                    } else {
                        return -1;
                    }
                } else {
                    return 1;
                }
            }
        }
        #endregion


        #region ctors
        public ColumnView () {
            InitializeComponent ();

            ViewModel = new ColumnViewModel ();
            DataContext = ViewModel;
        }

        public ColumnView (FSNode parentFSNode, int viewId) : this () {
            ViewId = viewId;
            ViewModel.ParentFSNode = parentFSNode;
        }
        #endregion


        #region utils
        public void ClearSelection () {
            if (ViewModel.ParentFSNode.Is (TypeTag.Root | TypeTag.SubRoot | TypeTag.Internal)) {
                var childFSNodesListView = Utils.FindVisualChild<ListView> (columnViewContentPresenter);
                childFSNodesListView?.UnselectAll ();
            }
        }
        #endregion


        #region events
        private void breadcrumbDockPanel_OnPreviewLeftMouseButtonDown (object sender, MouseButtonEventArgs e) {
            var dockPanel = sender as DockPanel;
            if (dockPanel != null) {
                ClearSelection ();
            }
        }

        // TODO: [?;?].
        private void childFSNodesListViewItem_OnMouseDoubleClick (object sender, MouseButtonEventArgs e) {
            var listViewItem = sender as ListViewItem;
            var fsNodeView = listViewItem?.Content as FSNodeView;
            if (fsNodeView != null) {
                var fsNodeSelected = fsNodeView.ViewModel.FSNode;
                var parentLayoutMgr = Utils.FindVisualParent<MillerColumnsLayout> (this);
                parentLayoutMgr?.NavigateTo (fsNodeSelected, ViewId);
            }
        }

        private void childFSNodesListView_OnSelectionChanged (object sender, SelectionChangedEventArgs e) {
            var listView = sender as ListView;
            if (listView != null) {
                var parentLayoutMgr = Utils.FindVisualParent<MillerColumnsLayout> (this);
                if (parentLayoutMgr != null) {
                    if (listView.SelectedIndex > -1) {
                        var fsNodeView = listView.SelectedItem as FSNodeView;
                        if (fsNodeView != null) {
                            var fsNodeSelected = fsNodeView.ViewModel.FSNode;
                            parentLayoutMgr.NavigateTo (fsNodeSelected, ViewId);
                        }
                    } else {
                        parentLayoutMgr.DeleteColumnsAfter (ViewId);
                    }
                }
            }
        }
        #endregion
    }
}
