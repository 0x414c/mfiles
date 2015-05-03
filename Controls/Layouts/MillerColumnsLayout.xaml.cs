using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Controls.Annotations;
using Controls.Auxiliary;
using Controls.UserControls;
using FSOps;


namespace Controls.Layouts {
    /// <summary>
    /// Interaction logic for MillerColumnsLayout.xaml
    /// </summary>
    public partial class MillerColumnsLayout : UserControl, INotifyPropertyChanged {
        /**
         \property  public MillerColumnsLayoutManager Model        
         \brief Gets or sets the model that acts as backing store for children Views.        
         \return    The model.
         */
        public MillerColumnsLayoutManager Model { get; set; }

        /**
         \property  public int ViewsCounter        
         \brief Gets the views counter.            
         \return    The views counter.
         */
        public int ViewsCounter { get { return Model.ColumnViews.Count; } }

        public string Header { get { return Model.ColumnViews.Last ().Model.ParentFSNode.Name; } }

        public MillerColumnsLayout () {
            InitializeComponent ();
                        
            Model = new MillerColumnsLayoutManager ();
            DataContext = Model;
        }

        public MillerColumnsLayout (FSNode parentFSNode): this () {
            TryAddColumnForFSNode (parentFSNode, 0);
        }

        public void TryAddColumnForFSNode (FSNode fsNodeToAdd, int columnViewId) {
            // Try to determine what Action is needed
            if (fsNodeToAdd.NodeLevel == NodeLevel.Leaf) {
                // TODO: try to open file
                var asFile = FileManagement.TryGetConcreteNode<FileNode> (fsNodeToAdd);
                if (asFile != null) {
                    MessageBox.Show ("File: " + asFile);

                    return;
                }

            } else {
                if (fsNodeToAdd.NodeLevel == NodeLevel.SubRoot) {
                    var asDrive = FileManagement.TryGetConcreteNode<DriveNode> (fsNodeToAdd);
                    if (asDrive != null) {
                        if (!asDrive.IsReady) {
                            // TODO: notify user
                            MessageBox.Show ("DriveNode: " + asDrive + " isn't ready!");

                            return;
                        }
                    }

                } else {
                    if (fsNodeToAdd.NodeLevel == NodeLevel.Internal) {
                        var asDirectory = FileManagement.TryGetConcreteNode<DirectoryNode> (fsNodeToAdd);
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler (this, new PropertyChangedEventArgs (propertyName));
            }
        }
    }
}
