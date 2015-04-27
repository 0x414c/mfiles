using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;


namespace Untitled.Auxilliary {

    /**
     \enum  NodeType
     \brief Represents filesystem node: drive, directory or file.
     * Root for an entire Machine, SubRoot for particular Drives,
     * Internal for Directories, Leaf for Files
     */
    public enum NodeType { Root, SubRoot, Internal, Leaf }

    public abstract class BasicFSNode {
        public string Name { get; set; }
        public NodeType NodeType { get; set; }
        public string FullPath { get; set; }
        public override string ToString () {
            return Name;
        }
    }

    public abstract class InternalFSNode : BasicFSNode {
        private ObservableCollection<BasicFSNode> _children;
        public ObservableCollection<BasicFSNode> Children {
            get {
                foreach (string directory in System.IO.Directory.GetDirectories (FullPath)) {
                    DirectoryInfo directoryInfo = new DirectoryInfo (directory);
                    _children.Add (new Directory (directory, directoryInfo.Name));
                }
                foreach (string file in System.IO.Directory.GetFiles (FullPath)) {
                    FileInfo fileInfo = new FileInfo (file);
                    _children.Add (new File (file, fileInfo.Name));
                }
                return _children;
            }
            set { _children = value; }
        }
    }

    public class SystemRoot: InternalFSNode {
        private ObservableCollection<BasicFSNode> _children;
        public ObservableCollection<BasicFSNode> Children {
            get {
                _children = new ObservableCollection<BasicFSNode> (FSOps.EnumerateDrives ());
                return _children;
            }
            set { _children = value; }
        }
        public SystemRoot () {
            Name = Environment.MachineName;
            NodeType = NodeType.Root;
            FullPath = System.Net.Dns.GetHostEntry ("localhost").HostName;
        }
        public SystemRoot (string fullPath, string name) {
            Name = name;
            NodeType = NodeType.Root;
            FullPath = fullPath;
        }
        public override string ToString () {
            return String.Format ("{0} ({1})", base.ToString (), FullPath);
        }
    }

    public class Drive : InternalFSNode {
        public bool IsReady { get; set; }
        public Drive (string fullPath, string name, bool isReady) {
            Name = name;
            NodeType = NodeType.SubRoot;
            FullPath = fullPath;
            IsReady = isReady;
            Children = new ObservableCollection<BasicFSNode> ();
        }                                
        public override string ToString () {
            return String.Format ("{0} ({1})", base.ToString (), FullPath);
        }
    }

    public class Directory: InternalFSNode {
        public Directory (string fullPath, string name) {
            Name = name;
            NodeType = NodeType.Internal;
            FullPath = fullPath;
            Children = new ObservableCollection<BasicFSNode> ();
        }
    }

    public class File : BasicFSNode {
        public File (string fullPath, string name) {
            Name = name;
            NodeType = NodeType.Leaf;
            FullPath = fullPath;
        }
    }

    class FSOps {
        public static List<Drive> EnumerateDrives () {
            return DriveInfo.GetDrives ().Select (
                _ => new Drive (_.Name, _.IsReady ? _.VolumeLabel: "<no label>", _.IsReady)
            ).ToList ();
        }
    }
}
