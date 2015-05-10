using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Controls.Auxiliary;
using Controls.UserControls;
using FSOps;


namespace Controls.Layouts {
    /// <summary>
    /// Interaction logic for MillerColumnsLayout.xaml
    /// </summary>
    public partial class MillerColumnsLayout : UserControl {
        /**
         \property  public MillerColumnsLayoutViewModel ViewModel        
         \brief Gets or sets the model that acts as backing store for children Views.        
         \return The model.
         */
        public MillerColumnsLayoutViewModel ViewModel { get; set; }

        /**
         \property  public int ViewsCounter        
         \brief Gets the views counter.            
         \return The views counter.
         */
        public int ViewsCounter {
            get { return ViewModel.ColumnViews.Count; }
        }

        public string GetCurrentTitle {
            get { return ViewModel.ColumnViews.Last ().ViewModel.ParentFSNode.ToString (); }
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

            ViewModel = new MillerColumnsLayoutViewModel ();
            DataContext = ViewModel;
        }

        public MillerColumnsLayout (FSNode parentFSNode) : this () {
            TryAddColumnForFSNode (parentFSNode, 0);
        }

        public void TryLevelUp () {
            if (ViewsCounter > 1) {
                ViewModel.ColumnViews.RemoveAt (ViewsCounter - 1);

                Title = GetCurrentTitle;
                millerColumnsLayoutScrollViewer.ScrollToRightEnd ();
            }    
        }

        public void DeleteColumnsAfter (int columnViewId) {
            if (ViewsCounter > 1) {
                ViewModel.ColumnViews.Remove (_ => _.ViewId > columnViewId + 1 && _.ViewId < ViewsCounter + 1);

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
                            if (!(asDirectory.IsAccessible && asDirectory.IsTraversable)) {
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
            if (columnViewId == ViewsCounter) {
                ViewModel.ColumnViews.Add (new ColumnView (fsNodeToAdd, ViewsCounter + 1));
            } else {
                // In case of previous column(s) selected
                // Remove columns from end; skip Caller and its Parents
                // and reuse Caller later to display new contents
                if (columnViewId < ViewsCounter) {
                    ViewModel.ColumnViews.Remove (_ => _.ViewId > columnViewId + 1 && _.ViewId < ViewsCounter + 1);
                    ViewModel.ColumnViews.Last ().ViewModel.ParentFSNode = fsNodeToAdd;
                }
            }
            
            Title = GetCurrentTitle;
            millerColumnsLayoutScrollViewer.ScrollToRightEnd ();
        }
    }
}
