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
                }                            
                _parentFsNode = value;
                InitFSWatcher (_parentFsNode);
                RefreshChildrenViews (_parentFsNode);
                OnPropertyChanged ();
            }
        }

        private ObservableCollection<FSNodeView> _childFSNodesViews;
       
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

        private FileSystemWatcher FileSystemWatcher { get; set; }

        public ColumnViewModel () {
            ChildFSNodesViews = new ObservableCollection<FSNodeView> ();
        }

        private void InitFSWatcher (FSNode parentFSNode) {
            if (parentFSNode.Is (TypeTag.SubRoot | TypeTag.Internal)) {
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
                    if (asDirectory.IsAccessible && asDirectory.IsTraversable) {
                        FileSystemWatcher = new FileSystemWatcher (asDirectory.FullPath);
                    } else {
                        return;
                    }
                }

                FileSystemWatcher.NotifyFilter =
                      NotifyFilters.FileName
                    | NotifyFilters.DirectoryName
                    | NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite;
                FileSystemWatcher.Changed += OnChildrenModelsChanged;
                FileSystemWatcher.Created += OnChildrenModelsChanged;
                FileSystemWatcher.Deleted += OnChildrenModelsChanged;
                FileSystemWatcher.Renamed += OnChildrenModelsChanged;
                FileSystemWatcher.EnableRaisingEvents = true;
                FileSystemWatcher.IncludeSubdirectories = false;
            }
        }

        // TODO: creating UserControls in runtime is a _very_ expensive operation in WPF :C
        // InitializeComponent () and LoadComponent (object component, Uri resourceLocator) is a major bottleneck in WPF
        // We can use Caching, or DataTemplates 
        // (I dunno how to add events handlers for templates, but they are very fast compared to Controls)
        // >>> To improve performance, please try to replace UserControl with CustomControl
        private void RefreshChildrenViews (FSNode parentFsNode) {          
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
                        var asDirectory = FileManagement.TryGetConcreteFSNode<DirectoryLikeFSNode> (parentFsNode);
                        if (asDirectory != null) {
                            foreach (var childNode in asDirectory.Children.AsParallel ()) {
                                ChildFSNodesViews.Add (new FSNodeView (childNode));
                            }
                        }
                    }
                }
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