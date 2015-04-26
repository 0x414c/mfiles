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
        
        public ColumnView () {
            InitializeComponent ();
        }

        public ColumnView (BasicFSNode parentBasicFsNode): this () {
            Model = new ColumnViewModel ();
            DataContext = Model;
            PopulateModel (parentBasicFsNode); 
        }

        private void PopulateModel (BasicFSNode parentBasicFsNode) {
            Model.ParentFsNodesViews.Add (new FSNodeView (parentBasicFsNode));
            parentFsNodesComboBox.SelectedIndex = 0;
            if (parentBasicFsNode.NodeType == NodeType.Root) {
                FSOps.EnumerateDrives ().ForEach (_ => Model.ChildFsNodesViews.Add (new FSNodeView (_)));
            } else {
                if ((parentBasicFsNode.NodeType == NodeType.Internal) || (parentBasicFsNode.NodeType == NodeType.SubRoot)) {
                    if (parentBasicFsNode.NodeType == NodeType.SubRoot) {
                        var asDrive = parentBasicFsNode as Drive;
                        if (!asDrive.IsReady) {
                            return;
                        }   
                    }
                    var internalFsNode = parentBasicFsNode as InternalFSNode;
                    if (internalFsNode != null) {
                        foreach (var childNode in internalFsNode.Children) {
                            Model.ChildFsNodesViews.Add (new FSNodeView (childNode));
                        }
                    }
                }
            }
        }

        private void listViewItem_MouseDoubleClick (object sender, MouseButtonEventArgs e) {
            ListViewItem item = sender as ListViewItem;
            FSNodeView obj = item.Content as FSNodeView;
            BasicFSNode nodeSelected = obj.Model.BasicFsNode;
            
            MessageBox.Show (nodeSelected.ToString ());
            
            MillerColumnsLayout layout = Utils.FindParent<MillerColumnsLayout> (this);
            if (layout != null) {
                if (nodeSelected.NodeType != NodeType.Leaf) {
                    layout.AddColumnForFSNode (nodeSelected);
                }
            }
        }
    }
}
