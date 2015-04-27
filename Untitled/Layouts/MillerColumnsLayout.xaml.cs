using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MillerColumnsLayout.xaml
    /// </summary>
    public partial class MillerColumnsLayout : UserControl {
        public MillerColumnsLayoutManager Model { get; set; }

        public MillerColumnsLayout () {
            InitializeComponent ();
            Model = new MillerColumnsLayoutManager ();
            DataContext = Model;
        }

        public void AddColumnForFSNode (BasicFSNode parentBasicFsNode, ColumnView callerView) {
            if (parentBasicFsNode.NodeType == NodeType.Leaf) {
                // TODO: try to open file
                return;
            } else {
                if (parentBasicFsNode.NodeType == NodeType.SubRoot) {
                    var asDrive = parentBasicFsNode as Drive;
                    if (asDrive != null) {
                        if (!asDrive.IsReady) {
                            // TODO: notify user
                            MessageBox.Show ("Drive isn't ready!");
                            return;
                        }
                    }
                }
            }
            // If caller is actual view not Bootstrapper
            // TODO: repl. Bottstrapper with smth. else :)
            if (callerView != null) {
                // Case of sequential browsing
                if (callerView.ViewId == Model.ColumnViews.Count) {
                    //MessageBox.Show (callerView.ViewId + "==" + Model.ColumnViews.Count);
                } else {
                    // If user selects previous column we need to reflow the layout
                    // (for Views following the Caller)
                    // TODO: if distance is 1, only change the following View next to the Caller
                    // No need!
                    if (callerView.ViewId < Model.ColumnViews.Count) {
                        //MessageBox.Show (callerView.ViewId + "<" + Model.ColumnViews.Count);
                        Model.ColumnViews =
                            new ObservableCollection<ColumnView> (Model.ColumnViews.Take (callerView.ViewId));
                    } else {
                        // WTF???
                        if (callerView.ViewId > Model.ColumnViews.Count) {
                            MessageBox.Show (callerView.ViewId + ">" + Model.ColumnViews.Count);
                        }
                    }
                }
            }
            Model.ColumnViews.Add (new ColumnView (parentBasicFsNode, Model.ColumnViews.Count + 1));                
            layoutScroller.ScrollToRightEnd ();
        }
    }
}
