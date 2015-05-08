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
    public partial class MillerColumnsLayout : UserControl {
        /**
         \property  public MillerColumnsLayoutManager Model        
         \brief Gets or sets the model that acts as backing store for children Views.        
         \return The model.
         */
        public MillerColumnsLayoutManager Model { get; set; }

        /**
         \property  public int ViewsCounter        
         \brief Gets the views counter.            
         \return The views counter.
         */
        public int ViewsCounter { get { return Model.ColumnViews.Count; } }

        public string GetCurrentTitle {
            get { return Model.ColumnViews.Last ().Model.ParentFSNode.ToString (); }
        }

        public string Title {
            get { return (string) GetValue (TitleProperty); }
            set { SetValue (TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register (
                "Title", typeof (string),
                typeof (MillerColumnsLayout), new PropertyMetadata ("Untitled Tab")
            );

        public MillerColumnsLayout () {
            InitializeComponent ();

            Model = new MillerColumnsLayoutManager ();
            DataContext = Model;
        }

        public MillerColumnsLayout (FSNode parentFSNode) : this () {
            TryAddColumnForFSNode (parentFSNode, 0);
        }

        public void TryLevelUp () {
            if (ViewsCounter > 1) {
                Model.ColumnViews.RemoveAt (ViewsCounter - 1);

                Title = GetCurrentTitle;
                millerColumnsLayoutScrollViewer.ScrollToRightEnd ();
            }    
        }

        public void DeleteColumnsAfter (int columnViewId) {
            if (ViewsCounter > 1) {
                Model.ColumnViews.Remove (_ => _.ViewId > columnViewId + 1 && _.ViewId < ViewsCounter + 1);

                Title = GetCurrentTitle;
                millerColumnsLayoutScrollViewer.ScrollToRightEnd ();
            }    
        }

        public void TryAddColumnForFSNode (FSNode fsNodeToAdd, int columnViewId) {
            // Try to determine what Action is needed
            if (fsNodeToAdd.TypeTag == TypeTag.Leaf) {
                // TODO: try to open file
                var asFile = FileManagement.TryGetConcreteFSNode<FileNode> (fsNodeToAdd);
                if (asFile != null) {
                    if (!asFile.IsAccessible) {
                        // TODO: notify user
                        MessageBox.Show ("FileNode: " + asFile + " isn't accessible!");

                        return;
                    } else {
                        MessageBox.Show ("File selected: " + asFile);

                        return;
                    }
                }

            } else {
                if (fsNodeToAdd.TypeTag == TypeTag.SubRoot) {
                    var asDrive = FileManagement.TryGetConcreteFSNode<DriveNode> (fsNodeToAdd);
                    if (asDrive != null) {
                        if (!(asDrive.IsReady && asDrive.IsTraversable)) {
                            // TODO: notify user
                            MessageBox.Show ("DriveNode: " + asDrive + " isn't ready or isn't accessible!");

                            return;
                        }
                    }

                } else {
                    if (fsNodeToAdd.TypeTag == TypeTag.Internal) {
                        var asDirectory = FileManagement.TryGetConcreteFSNode<DirectoryNode> (fsNodeToAdd);
                        if (asDirectory != null) {
                            if (!asDirectory.IsAccessible) {
                                // TODO: notify user
                                MessageBox.Show ("DirectoryNode: " + asDirectory + " isn't accessible!");

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
            
            Title = GetCurrentTitle;
            millerColumnsLayoutScrollViewer.ScrollToRightEnd ();
        }
    }
}
