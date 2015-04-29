using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Untitled.Auxilliary;
using Untitled.LayoutManagers;
using Untitled.Models;


namespace Untitled {
    /// <summary>
    /// Interaction logic for ColumnView.xaml
    /// </summary>
    public partial class ColumnView : UserControl {
        public ColumnViewModel Model { get; private set; }

        public int ViewId { get; private set; }

        public ColumnView () {
            InitializeComponent ();
        }

        public ColumnView (FSNode parentFsNode, int viewId) : this () {
            Model = new ColumnViewModel ();
            DataContext = Model;
            ViewId = viewId;
        }

        public void RefreshChildrenViews (FSNode parentFsNode) {
            Model.ParentFSNodesViews.Add (new FSNodeView (parentFsNode));
            parentFsNodesComboBox.SelectedIndex = 0;

            if (parentFsNode.NodeLevel == NodeLevel.Root) {
                var asSystemRoot = FSOps.TryGetConcreteNode<SystemRootNode> (parentFsNode);
                if (asSystemRoot != null) {
                    foreach (var childNode in asSystemRoot.Children) {
                        Model.ChildFSNodesViews.Add (new FSNodeView (childNode));
                    }
                }
            } else {
                if (parentFsNode.NodeLevel == NodeLevel.SubRoot) {
                    var asDrive = FSOps.TryGetConcreteNode<DriveNode> (parentFsNode);
                    if (asDrive != null) {
                        foreach (var childNode in asDrive.Children) {
                            Model.ChildFSNodesViews.Add (new FSNodeView (childNode));
                        }
                    }
                } else {
                    if (parentFsNode.NodeLevel == NodeLevel.Internal) {
                        var asInternal = FSOps.TryGetConcreteNode<TraversableFSNode> (parentFsNode);
                        if (asInternal != null) {
                            foreach (var childNode in asInternal.Children) {
                                Model.ChildFSNodesViews.Add (new FSNodeView (childNode));
                            }
                        }
                    }
                }
            }
        }

        private void listViewItem_MouseDoubleClick (object sender, MouseButtonEventArgs e) {
            ListViewItem listViewItem = sender as ListViewItem;
            if (listViewItem != null) {
                FSNodeView fsNodeView = listViewItem.Content as FSNodeView;
                if (fsNodeView != null) {
                    FSNode fsNodeSelected = fsNodeView.Model.FSNode;
                    MillerColumnsLayout parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
                    if (parentLayoutMgr != null) {
                        parentLayoutMgr.TryAddColumnForFSNode (fsNodeSelected, this.ViewId);
                    }
                }
            }
        }
    }
}
