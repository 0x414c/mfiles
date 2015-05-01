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
using Files.Auxilliary;
using Files.LayoutManagers;
using Files.Models;


namespace Files {
    /// <summary>
    /// Interaction logic for MillerColumnsLayout.xaml
    /// </summary>
    public partial class MillerColumnsLayout : UserControl {
        /**
         \property  public MillerColumnsLayoutManager Model        
         \brief Gets or sets the model that acts as backing store for children Views.        
         \return    The model.
         */
        public MillerColumnsLayoutManager Model { get; private set; }

        /**
         \property  public int ViewsCounter        
         \brief Gets the views counter.            
         \return    The views counter.
         */
        public int ViewsCounter { get { return Model.ColumnViews.Count; } }

        public MillerColumnsLayout () {
            InitializeComponent ();
                        
            Model = new MillerColumnsLayoutManager ();
            DataContext = Model;
        }

        public void TryAddColumnForFSNode (FSNode fsNodeToAdd, int columnViewId) {
            // Try to determine what Action is needed
            if (fsNodeToAdd.NodeLevel == NodeLevel.Leaf) {
                // TODO: try to open file
                var asFile = FSOps.TryGetConcreteNode<FileNode> (fsNodeToAdd);
                if (asFile != null) {
                    MessageBox.Show ("File: " + asFile);

                    return;
                }

            } else {
                if (fsNodeToAdd.NodeLevel == NodeLevel.SubRoot) {
                    var asDrive = FSOps.TryGetConcreteNode<DriveNode> (fsNodeToAdd);
                    if (asDrive != null) {
                        if (!asDrive.IsReady) {
                            // TODO: notify user
                            MessageBox.Show ("DriveNode: " + asDrive + " isn't ready!");

                            return;
                        }
                    }

                } else {
                    if (fsNodeToAdd.NodeLevel == NodeLevel.Internal) {
                        var asDirectory = FSOps.TryGetConcreteNode<DirectoryNode> (fsNodeToAdd);
                        if (asDirectory != null) {
                            if (!asDirectory.IsAccessible) {
                                // TODO: notify user
                                MessageBox.Show ("DirectoryNode: " + asDirectory + " isn't ready!");

                                return;
                            }
                        }
                    }
                }
            }

            // If user selects previous column we need to reflow the layout
            // (for Views following the Caller)

            // In case of previous column selected
            // Remove columns from end; skip Caller and its Parents
            // and reuse Caller later to display new contents
            if (columnViewId + 1 < ViewsCounter) {
                Model.ColumnViews.Remove (_ => _.ViewId > columnViewId + 1 && _.ViewId < ViewsCounter + 1);
            } else {
                // If we need another column
                if (columnViewId == ViewsCounter) {
                    Model.ColumnViews.Add (new ColumnView (fsNodeToAdd, ViewsCounter + 1));
                }
            }

            Model.ColumnViews.Last ().Model.ParentFSNode = fsNodeToAdd;
            //Model.ColumnViews.Last ().Model.RefreshChildrenViews (fsNodeToAdd);
            
            layoutScroller.ScrollToRightEnd ();
        }
    }
}
