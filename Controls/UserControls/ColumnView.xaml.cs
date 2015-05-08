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
        public ColumnViewModel Model { get; private set; }

        public int ViewId { get; private set; }

        private ColumnView () {
            InitializeComponent ();
        }

        // TODO: use DataTemplate, determine between File List, File Preview, Error Notif.
        public ColumnView (FSNode parentFsNode, int viewId) : this () {
            Model = new ColumnViewModel (parentFsNode);
            DataContext = Model;
            ViewId = viewId;
        }

        private void breadcrumbStackPanel_OnPreviewLeftMouseButtonDown (object sender, MouseButtonEventArgs e) {
            var stackPanel = sender as StackPanel;
            if (stackPanel != null) {
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
                //        var fsNodeSelected = fsNodeView.Model.FSNode;
                //        var parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
                //        if (parentLayoutMgr != null) {
                //            parentLayoutMgr.TryAddColumnForFSNode (fsNodeSelected, ViewId);
                //        }
                //    }
                //}
            }
        }

        private void childFSNodesListViewItem_OnPreviewRightMouseButtonDown (object sender, MouseButtonEventArgs e) {
            var listViewItem = sender as ListViewItem;
            if (listViewItem != null) {
                var fsNodeView = listViewItem.Content as FSNodeView;
                if (fsNodeView != null) {
                    var fsNodeSelected = fsNodeView.Model.FSNode;

                    var fsNodeSelectedAsFileLikeFSNode = fsNodeSelected as FileLikeFSNode;
                    if (fsNodeSelectedAsFileLikeFSNode != null) {
                        var ctxMnu = new ShellContextMenu.ShellContextMenu ();
                        var arrFI = new FileInfo[1];
                        arrFI[0] = new FileInfo (fsNodeSelectedAsFileLikeFSNode.FullPath);
                        ctxMnu.ShowContextMenu (arrFI, new System.Drawing.Point (300, 300));    
                    }                               
                }
            }    
        }

        private void childFSNodesListView_OnSelectionChanged (object sender, SelectionChangedEventArgs e) {
            var listView = sender as ListView;
            if (listView != null) {
                var fsNodeView = listView.SelectedItem as FSNodeView;
                if (fsNodeView != null) {
                    var fsNodeSelected = fsNodeView.Model.FSNode;
                    var parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
                    if (parentLayoutMgr != null) {
                        parentLayoutMgr.TryAddColumnForFSNode (fsNodeSelected, ViewId);
                    }
                }
            }
        }

        // TODO: filter scrollbar clicks
        private void childFSNodesListView_OnPreviewMouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
            var asListView = sender as ScrollContentPresenter;
            if (asListView != null) {
                //var lv = Utils.FindParent<ListView> (sender);
                //lv.UnselectAll ();
            }
        }
    }
}
