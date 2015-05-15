using System;
using System.IO;
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
        #region props
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

        public ColumnView LastColumnView {
            get { return ViewModel.ColumnViews.Last (); }
        }

        public ColumnView LastButOneColumnView {
            get {
                if (ViewsCounter > 1) {
                    return ViewModel.ColumnViews.ElementAt (ViewModel.ColumnViews.Count - 2);
                } else {
                    return ViewModel.ColumnViews.Last ();
                }
            }
        }

        public string CurrentTitle {
            get { return LastColumnView.Title; }
        }
        #endregion


        #region depprops
        private string Title {
            get { return (string) GetValue (TitleProperty); }
            set { SetValue (TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register (
                "Title", typeof (string),
                typeof (MillerColumnsLayout), new PropertyMetadata ("<untitled>")
            );

        
        private string CurrentItemInfo {
            get { return (string) GetValue (CurrentItemInfoProperty); }
            set { SetValue (CurrentItemInfoProperty, value); }
        }

        public static readonly DependencyProperty CurrentItemInfoProperty =
            DependencyProperty.Register (
                "CurrentItemInfo", typeof (string),
                typeof (MillerColumnsLayout), new PropertyMetadata ("<current item info is n/a>")
            );


        private string Status {
            get { return (string) GetValue (StatusProperty); }
            set { SetValue (StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register (
                "Status", typeof (string),
                typeof (MillerColumnsLayout),new PropertyMetadata ("<status is unknown>")
            );

                                     
        public string ExtraStatus {
            get { return (string) GetValue (ExtraStatusProperty); }
            set { SetValue (ExtraStatusProperty, value); }
        }

        public static readonly DependencyProperty ExtraStatusProperty =
            DependencyProperty.Register (
                "ExtraStatus", typeof (string),
                typeof (MillerColumnsLayout), new PropertyMetadata ("<extra status is unknown>")
            );
        #endregion


        #region ctors
        public MillerColumnsLayout () {
            InitializeComponent ();

            ViewModel = new MillerColumnsLayoutViewModel ();
            DataContext = ViewModel;
        }

        public MillerColumnsLayout (FSNode parentFSNode) : this () {
            NavigateTo (parentFSNode, 0);
        }
        #endregion

                          
        #region navigation
        private void AddColumnAfter (FSNode fsNodeToAdd, int columnViewId) {
            if (columnViewId == ViewsCounter) {
                ViewModel.ColumnViews.Add (new ColumnView (fsNodeToAdd, ViewsCounter + 1));
            } else {
                if (columnViewId < ViewsCounter) {
                    ViewModel.ColumnViews.Remove (_ => _.ViewId > columnViewId + 1 && _.ViewId < ViewsCounter + 1);
                    LastColumnView.ViewModel.ParentFSNode = fsNodeToAdd;
                }
            }

            FinalizeLayoutReflow ();
        }

        public bool DeleteColumnsAfter (int columnViewId) {
            if (ViewsCounter > 0) {
                ViewModel.ColumnViews.Remove (_ => _.ViewId > columnViewId && _.ViewId < ViewsCounter + 1);
                LastColumnView.ClearSelection ();
                FinalizeLayoutReflow ();

                return true;
            } else {
                return false;
            }
        }

        private void FinalizeLayoutReflow () {
            RefreshDepProps ();
        }

        private void RefreshDepProps () {
            Title = CurrentTitle;

            ExtraStatus = string.Format ("{0} selected", LastButOneColumnView.SelectionSize);

            if (LastColumnView.ViewModel.ParentFSNode.Is (TypeTag.Root | TypeTag.SubRoot | TypeTag.Internal)) {
                Status = string.Format (
                    "{0}: {1} items total",
                    CurrentTitle, LastColumnView.ItemsCount
                );
            } else {
                if (LastColumnView.ViewModel.ParentFSNode.TypeTag == TypeTag.Leaf) {
                    var asFileLikeFSNode = LastColumnView.ViewModel.ParentFSNode as FileLikeFSNode;
                    if (asFileLikeFSNode != null) {
                        Status = string.Format (
                            "{0}: {1}",
                            CurrentTitle,
                            FSOps.FSOps.StrFormatByteSize (new FileInfo (asFileLikeFSNode.FullPath).Length)
                        );
                    }
                }
            }
            
            if (LastColumnView.ViewModel.ParentFSNode.Is (TypeTag.SubRoot | TypeTag.Internal)) {
                var asDirectoryLike = FSOps.FSOps.TryGetConcreteFSNode<DirectoryLikeFSNode> (LastColumnView.ViewModel.ParentFSNode);
                if (asDirectoryLike != null) {
                    CurrentItemInfo = string.Format (
                        "{0}: {1} of {2} available", asDirectoryLike.RootDrive,
                        FSOps.FSOps.StrFormatByteSize (asDirectoryLike.RootDrive.DriveInfo.AvailableFreeSpace),
                        FSOps.FSOps.StrFormatByteSize (asDirectoryLike.RootDrive.DriveInfo.TotalSize)
                    );
                }
            } else {
                if (LastColumnView.ViewModel.ParentFSNode.TypeTag == TypeTag.Leaf) {
                    var asFileLike = FSOps.FSOps.TryGetConcreteFSNode<FileLikeFSNode> (LastColumnView.ViewModel.ParentFSNode);
                    if (asFileLike != null) {
                        CurrentItemInfo = string.Format (
                            "{0}: {1} of {2} available", asFileLike.RootDrive,
                            FSOps.FSOps.StrFormatByteSize (asFileLike.RootDrive.DriveInfo.AvailableFreeSpace),
                            FSOps.FSOps.StrFormatByteSize (asFileLike.RootDrive.DriveInfo.TotalSize)
                        );
                    }   
                } else {
                    if (LastColumnView.ViewModel.ParentFSNode.TypeTag == TypeTag.Root) {
                        CurrentItemInfo = CurrentTitle;
                    }
                }                                  
            }
        }

        public bool NavigateTo (FSNode fsNodeToAdd, int originColumnViewId) {
            if (fsNodeToAdd.TypeTag == TypeTag.Leaf) {
                var asFile = FSOps.FSOps.TryGetConcreteFSNode<FileNode> (fsNodeToAdd);
                if (asFile != null) {
                    if (!asFile.IsAccessible) {
                        DeleteColumnsAfter (originColumnViewId);

                        MessageBox.Show (
                            "File \"" + asFile + "\" isn't accessible!",
                            "File access error", MessageBoxButton.OK, MessageBoxImage.Error
                        );

                        return false;
                    }
                }
            } else {
                if (fsNodeToAdd.TypeTag == TypeTag.SubRoot) {
                    var asDrive = FSOps.FSOps.TryGetConcreteFSNode<DriveNode> (fsNodeToAdd);
                    if (asDrive != null) {
                        if (!(asDrive.IsReady && asDrive.IsTraversable)) {
                            DeleteColumnsAfter (originColumnViewId);

                            MessageBox.Show (
                                "Drive \"" + asDrive + "\" isn't ready or isn't accessible now.",
                                "Drive access error", MessageBoxButton.OK, MessageBoxImage.Error
                            );

                            return false;
                        }
                    }
                } else {
                    if (fsNodeToAdd.TypeTag == TypeTag.Internal) {
                        var asDirectory = FSOps.FSOps.TryGetConcreteFSNode<DirectoryNode> (fsNodeToAdd);
                        if (asDirectory != null) {
                            if (!(asDirectory.IsAccessible && asDirectory.IsTraversable)) {
                                DeleteColumnsAfter (originColumnViewId);

                                MessageBox.Show (
                                    "Directory \"" + asDirectory + "\" isn't accessible.",
                                    "Directory access error", MessageBoxButton.OK, MessageBoxImage.Error
                                );

                                return false;
                            }
                        }
                    }
                }
            }

            AddColumnAfter (fsNodeToAdd, originColumnViewId);

            return true;
        }
        #endregion
    }
}
