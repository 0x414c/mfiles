using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controls.Auxiliary;
using Controls.Layouts;
using FSOps;


namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for ColumnView.xaml
    /// </summary>
    public partial class ColumnView : UserControl {
        #region fields & props
        public ColumnViewModel ViewModel { get; private set; }

        public string Title {
            get { return ViewModel.ParentFSNode.ToString (); }
        }

        public int ItemsCount {
            get { return ViewModel.ChildFSNodesViews.Count; }
        }

        public int ViewId { get; private set; }

        public int SelectionSize {
            get { return childFSNodesListView.SelectedItems.Count; }
        }
        #endregion


        #region ctors
        public ColumnView () {
            InitializeComponent ();

            ViewModel = new ColumnViewModel ();
            DataContext = ViewModel;
        }

        // TODO: use DataTemplate, determine between File List, File Preview, Error Notif.
        public ColumnView (FSNode parentFsNode, int viewId) : this () {
            ViewId = viewId;
            ViewModel.ParentFSNode = parentFsNode;
        }
        #endregion


        #region utils
        public void ClearSelection () {
            childFSNodesListView.UnselectAll ();
        }
        #endregion


        #region events
        private void breadcrumbDockPanel_OnPreviewLeftMouseButtonDown (object sender, MouseButtonEventArgs e) {
            var dockPanel = sender as DockPanel;
            if (dockPanel != null) {
                var parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
                if (parentLayoutMgr != null) {
                    parentLayoutMgr.DeleteColumnsAfter (ViewId);
                } 
            }
        }

        private void childFSNodesListViewItem_OnMouseDoubleClick (object sender, MouseButtonEventArgs e) {
            var listViewItem = sender as ListViewItem;
            if (listViewItem != null) {
                // TODO: open in new tab for Traversable / run for File
                MessageBox.Show ("<2x");
                //if (Equals (childFSNodesListView.SelectedItem as ListViewItem, listViewItem)) {
                //    var fsNodeView = listViewItem.Content as FSNodeView;
                //    if (fsNodeView != null) {
                //        var fsNodeSelected = fsNodeView.ViewModel.FSNode;
                //        var parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
                //        if (parentLayoutMgr != null) {
                //            parentLayoutMgr.NavigateTo (fsNodeSelected, ViewId);
                //        }
                //    }
                //}
            }
        }

        //private void childFSNodesListViewItem_OnPreviewRightMouseButtonDown (object sender, MouseButtonEventArgs e) {
        //    var listViewItem = sender as ListViewItem;
        //    if (listViewItem != null) {
        //        var fsNodeView = listViewItem.Content as FSNodeView;
        //        if (fsNodeView != null) {
        //            var fsNodeSelected = fsNodeView.ViewModel.FSNode;

        //            var fsNodeSelectedAsFileLikeFSNode = fsNodeSelected as FileLikeFSNode;
        //            if (fsNodeSelectedAsFileLikeFSNode != null) {
        //                //var ctxMnu = new ShellContextMenu.ShellContextMenu ();
        //                //var arrFI = new FileInfo[1];
        //                //arrFI[0] = new FileInfo (fsNodeSelectedAsFileLikeFSNode.FullPath);
        //                //ctxMnu.ShowContextMenu (arrFI, new System.Drawing.Point (300, 300));    
        //            }                               
        //        }
        //    }    
        //}

        private void childFSNodesListView_OnSelectionChanged (object sender, SelectionChangedEventArgs e) {
            var listView = sender as ListView;
            if (listView != null) {
                var parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
                if (parentLayoutMgr != null) {
                    if (listView.SelectedIndex > -1) {
                        var fsNodeView = listView.SelectedItem as FSNodeView;
                        if (fsNodeView != null) {
                            var fsNodeSelected = fsNodeView.ViewModel.FSNode;
                            //var currentSelectionIndex = listView.SelectedIndex;
                            if (!parentLayoutMgr.NavigateTo (fsNodeSelected, ViewId)) {
                                MessageBox.Show ("!");
                                //listView.SelectedIndex = currentSelectionIndex;
                            }
                        }
                    }
                    //else {
                    //    MessageBox.Show ("-1 :" + ViewId);
                    //    parentLayoutMgr.DeleteColumnsAfter (ViewId);
                    //}
                }
            }
        }

        // TODO: filter scrollbar clicks
        private void childFSNodesListView_OnPreviewMouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
            //ClearSelection ();
            //var asListView = sender as ScrollContentPresenter;
            //if (asListView != null) {
            //    var lv = Utils.FindParent<ListView> (asListView);
            //    lv.UnselectAll ();
            //}
        }
        #endregion
    }
}
