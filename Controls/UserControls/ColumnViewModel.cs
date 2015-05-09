using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Controls.Annotations;
using FSOps;


namespace Controls.UserControls {
    public sealed class ColumnViewModel : INotifyPropertyChanged {
        private FSNode _parentFsNode;
        private ObservableCollection<FSNodeView> _childFSNodesViews;
        private ObservableCollection<FSNodeView> _parentFSNodesViews;

        public FSNode ParentFSNode {
            get { return _parentFsNode; }
            set {
                if (Equals (value, _parentFsNode)) {
                    return;
                }                            
                _parentFsNode = value;
                ReinitFSWatcher (_parentFsNode);
                RefreshChildrenViews (_parentFsNode);
                OnPropertyChanged ();
            }
        }

        public ObservableCollection<FSNodeView> ChildFSNodesViews {
            get { return _childFSNodesViews; }
            set {
                if (Equals (value, _childFSNodesViews)) {
                    return;
                }
                _childFSNodesViews = value;
                OnPropertyChanged ();
            }
        }

        public ObservableCollection<FSNodeView> ParentFSNodesViews {
            get { return _parentFSNodesViews; }
            set {
                if (Equals (value, _parentFSNodesViews)) {
                    return;
                }
                _parentFSNodesViews = value;
                OnPropertyChanged ();
            }
        }


        private FileSystemWatcher FileSystemWatcher { get; set; }

        //[STAThread]
        public void RefreshChildrenViews (FSNode parentFsNode) {
            //if (!Equals (ViewModel.ParentFSNode, parentFsNode)) {
            
            ParentFSNodesViews.Clear ();
            
            ParentFSNodesViews.Add (new FSNodeView (parentFsNode));

            //parentFsNodesComboBox.SelectedIndex = 0;
            ChildFSNodesViews.Clear ();
            
            if (parentFsNode.TypeTag == TypeTag.Root) {
                var asSystemRoot = FileManagement.TryGetConcreteFSNode<SystemRootNode> (parentFsNode);
                if (asSystemRoot != null) {
                    foreach (var childNode in asSystemRoot.Children) {
                        ChildFSNodesViews.Add (new FSNodeView (childNode));
                    }
                }
            } else {
                if (parentFsNode.TypeTag == TypeTag.SubRoot) {
                    var asDrive = FileManagement.TryGetConcreteFSNode<DriveNode> (parentFsNode);
                    if (asDrive != null) {
                        foreach (var childNode in asDrive.Children) {
                            ChildFSNodesViews.Add (new FSNodeView (childNode));
                        }
                    }
                } else {
                    if (parentFsNode.TypeTag == TypeTag.Internal) {
                        var asInternal = FileManagement.TryGetConcreteFSNode<DirectoryLikeFSNode> (parentFsNode);
                        if (asInternal != null) {
                            foreach (var childNode in asInternal.Children) {
                                ChildFSNodesViews.Add (new FSNodeView (childNode));
                            }
                        }
                    }
                }
            }
            //}
        }

        public ColumnViewModel () {
            ChildFSNodesViews = new ObservableCollection<FSNodeView> ();
            ParentFSNodesViews = new ObservableCollection<FSNodeView> ();
        }

        public ColumnViewModel (FSNode parentFSNode) : this () {
            //ParentFSNode = parentFSNode;
        }

        private void ReinitFSWatcher (FSNode parentFSNode) {
            if (parentFSNode.TypeTag == TypeTag.SubRoot || parentFSNode.TypeTag == TypeTag.Internal) {
                if (parentFSNode.TypeTag == TypeTag.SubRoot) {
                    var asDrive = FileManagement.TryGetConcreteFSNode<DriveNode> (parentFSNode);
                    // TODO:
                    if (asDrive.IsReady) {
                        FileSystemWatcher = new FileSystemWatcher (asDrive.FullPath);
                    } else {
                        return;
                    }          
                } else {
                    var asDirectory = FileManagement.TryGetConcreteFSNode<DirectoryNode> (parentFSNode);
                    if (asDirectory.IsAccessible) {
                        FileSystemWatcher = new FileSystemWatcher (asDirectory.FullPath);
                    } else {
                        return;
                    }
                }

                FileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastAccess | NotifyFilters.LastWrite;
                //FileSystemWatcher.Changed += OnChildrenModelsChanged;
                //FileSystemWatcher.Created += OnChildrenModelsChanged;
                //FileSystemWatcher.Deleted += OnChildrenModelsChanged;
                FileSystemWatcher.Renamed += OnChildrenModelsChanged;
                FileSystemWatcher.EnableRaisingEvents = true;
                FileSystemWatcher.IncludeSubdirectories = false;
            }
        }

        // TODO: temporary solution
        private void OnChildrenModelsChanged (object source, FileSystemEventArgs e) {
            Application.Current.Dispatcher.Invoke (
                delegate {
                    RefreshChildrenViews (ParentFSNode);
                }
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler (this, new PropertyChangedEventArgs (propertyName));
            }
        }
    }
}