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

        public int ViewId { get; set; }

        private ColumnView () {
            InitializeComponent ();
        }

        // TODO: use DataTemplate, determine between File List, File Preview, Error Notif.
        public ColumnView (FSNode parentFsNode, int viewId) : this () {
            Model = new ColumnViewModel (parentFsNode);
            DataContext = Model;
            ViewId = viewId;
        }

        private void listViewItem_MouseDoubleClick (object sender, MouseButtonEventArgs e) {
            var listViewItem = sender as ListViewItem;
            if (listViewItem != null) {
                MessageBox.Show ("dc");
                // TODO: open in new tab for Traversable / run for File
                if (Equals (childFsNodesListView.SelectedItem as ListViewItem, listViewItem)) {
                    var fsNodeView = listViewItem.Content as FSNodeView;
                    if (fsNodeView != null) {
                        var fsNodeSelected = fsNodeView.Model.FSNode;
                        var parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
                        if (parentLayoutMgr != null) {
                            parentLayoutMgr.TryAddColumnForFSNode (fsNodeSelected, ViewId);
                        }
                    }
                }
            }
        }

        private void listView_onSelectionChanged (object sender, SelectionChangedEventArgs e) {
            //MessageBox.Show ("sel");
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

        // TODO:
        private void listView_OnPreviewMouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
            var asListView = sender as ListView;
            if (asListView != null) {
                asListView.UnselectAll ();
            }
            //var parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
            //if (parentLayoutMgr != null) {
            //    parentLayoutMgr.TryAddColumnForFSNode (fsNodeSelected, ViewId);
            //}    
        }
    }
}
