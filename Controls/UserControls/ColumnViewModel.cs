using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Controls.Annotations;
using FSOps;


namespace Controls.UserControls {
    public sealed class ColumnViewModel : INotifyPropertyChanged {
        private FSNode _parentFsNode;

        public FSNode ParentFSNode {
            get { return _parentFsNode; }
            set {
                if (Equals (value, _parentFsNode)) {
                    return;
                } else {
                    _parentFsNode = value;
                    InitFileSystemWatcher (_parentFsNode);
                    RefreshChildrenViews (_parentFsNode);
                    OnPropertyChanged ();
                }
            }
        }

        private ObservableCollection<FSNodeView> _childFSNodesViews;

        public ObservableCollection<FSNodeView> ChildFSNodesViews {
            get { return _childFSNodesViews; }
            private set {
                if (Equals (value, _childFSNodesViews)) {
                    return;
                } else {
                    _childFSNodesViews = value;
                    OnPropertyChanged ();
                }
            }
        }

        private FileSystemWatcher FileSystemWatcher { get; set; }


        public ColumnViewModel () {
            ChildFSNodesViews = new ObservableCollection<FSNodeView> ();
        }


        private void InitFileSystemWatcher (FSNode parentFSNode) {
            if (parentFSNode.Is (TypeTag.SubRoot | TypeTag.Internal)) {
                if (parentFSNode.TypeTag == TypeTag.SubRoot) {
                    var asDrive = FSOps.FSOps.TryGetConcreteFSNode<DriveNode> (parentFSNode);
                    if (asDrive.IsReady && asDrive.IsAccessible && asDrive.IsTraversable) {
                        FileSystemWatcher = new FileSystemWatcher (asDrive.FullPath);
                    } else {
                        return;
                    }
                } else {
                    var asDirectory = FSOps.FSOps.TryGetConcreteFSNode<DirectoryNode> (parentFSNode);
                    if (asDirectory.IsAccessible && asDirectory.IsTraversable) {
                        FileSystemWatcher = new FileSystemWatcher (asDirectory.FullPath);
                    } else {
                        return;
                    }
                }

                FileSystemWatcher.NotifyFilter =
                    NotifyFilters.FileName |
                    NotifyFilters.DirectoryName;
                    //NotifyFilters.LastAccess |
                    //NotifyFilters.LastWrite;
                FileSystemWatcher.Changed += OnChildrenModelsChanged;
                FileSystemWatcher.Created += OnChildrenModelsChanged;
                FileSystemWatcher.Deleted += OnChildrenModelsChanged;
                FileSystemWatcher.Renamed += OnChildrenModelsChanged;
                FileSystemWatcher.EnableRaisingEvents = true;
                FileSystemWatcher.IncludeSubdirectories = false;
            }
        }

        // TODO: [1;2] Creating UserControls at runtime can be an expensive operation in WPF.
        private void RefreshChildrenViews (FSNode parentFsNode) {
            ChildFSNodesViews.Clear ();

            var children = new List<FSNodeView> ();

            if (parentFsNode.TypeTag == TypeTag.Root) {
                var asSystemRoot = FSOps.FSOps.TryGetConcreteFSNode<SystemRootNode> (parentFsNode);
                if (asSystemRoot != null) {
                    children.AddRange (asSystemRoot.Children.Select (childNode => new FSNodeView (childNode)));
                    ChildFSNodesViews = new ObservableCollection<FSNodeView> (children);
                }
            } else {
                if (parentFsNode.TypeTag == TypeTag.SubRoot) {
                    var asDrive = FSOps.FSOps.TryGetConcreteFSNode<DriveNode> (parentFsNode);
                    if (asDrive != null) {
                        children.AddRange (asDrive.Children.Select (childNode => new FSNodeView (childNode)));
                        ChildFSNodesViews = new ObservableCollection<FSNodeView> (children);
                    }
                } else {
                    if (parentFsNode.TypeTag == TypeTag.Internal) {
                        var asDirectory = FSOps.FSOps.TryGetConcreteFSNode<DirectoryFSNode> (parentFsNode);
                        if (asDirectory != null) {
                            children.AddRange (asDirectory.Children.Select (childNode => new FSNodeView (childNode)));
                            ChildFSNodesViews = new ObservableCollection<FSNodeView> (children);
                        }
                    }
                }
            }
        }

        // TODO: [1;?] Temporary (?) solution.
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
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}