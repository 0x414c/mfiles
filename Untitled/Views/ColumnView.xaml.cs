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
        public ColumnViewModel Model { get; set; }
        public int ViewId { get; set; }
        
        public ColumnView () {
            InitializeComponent ();
        }

        public ColumnView (BasicFSNode parentBasicFsNode, int viewId): this () {
            Model = new ColumnViewModel ();
            DataContext = Model;
            PopulateModel (parentBasicFsNode);
            ViewId = viewId;
        }

        private void PopulateModel (BasicFSNode parentBasicFsNode) {
            Model.ParentFsNodesViews.Add (new FSNodeView (parentBasicFsNode));
            parentFsNodesComboBox.SelectedIndex = 0;
            if (parentBasicFsNode.NodeType == NodeType.Root) {
                var asSystemRoot = parentBasicFsNode as SystemRoot;
                if (asSystemRoot != null) {
                    foreach (var childNode in asSystemRoot.Children) {
                        Model.ChildFsNodesViews.Add (new FSNodeView (childNode));
                    }
                    //FSOps.EnumerateDrives ().ForEach (_ => Model.ChildFsNodesViews.Add (new FSNodeView (_)));    
                }
            } else {
                if (parentBasicFsNode.NodeType == NodeType.SubRoot) {
                    var asDrive = parentBasicFsNode as Drive;
                    if (asDrive != null) {
                        foreach (var childNode in asDrive.Children) {
                                Model.ChildFsNodesViews.Add (new FSNodeView (childNode));
                            }
                    }
                } else {
                    if (parentBasicFsNode.NodeType == NodeType.Internal) {
                        var asInternal = parentBasicFsNode as InternalFSNode;
                        if (asInternal != null) {
                            foreach (var childNode in asInternal.Children) {
                                Model.ChildFsNodesViews.Add (new FSNodeView (childNode));
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
                    BasicFSNode basicFsNodeSelected = fsNodeView.Model.BasicFsNode;
            
                    MillerColumnsLayout parentLayoutMgr = Utils.FindParent<MillerColumnsLayout> (this);
                    if (parentLayoutMgr != null) {
                        parentLayoutMgr.AddColumnForFSNode (basicFsNodeSelected, this);
                    }
                }
            }
        }
    }
}
