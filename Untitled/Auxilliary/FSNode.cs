using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Windows;


namespace Files.Auxilliary {
    /**
     \enum  NodeLevel
     \brief Represents filesystem node: drive, directory or file.
      Root for an entire Machine, SubRoot for particular Drives,
      Internal for Directories, Leaf for Files
     */
    public enum NodeLevel { Root, SubRoot, Internal, Leaf }

    /**
     \class FSNode    
     \brief A file system node.
     */
    public abstract class FSNode {
        public NodeLevel NodeLevel { get; protected set; }

        public string Name { get; protected set; }

        public string FullPath { get; protected set; }
    }

    /**
     \class FileLikeFSNode
     \brief A file like file system node: DriveNode, DirectoryNode or FileNode.      
     */
    public abstract class FileLikeFSNode : FSNode {
        public FileSystemInfo FileSystemInfo { get; protected set; }

        public string Name {
            get { return FileSystemInfo.Name; }
        }

        public string FullPath {
            get { return FileSystemInfo.FullName; }
        }

        public override string ToString () {
            return Name;
        }
    }

    /**
     \class TraversableFSNode    
     \brief A directory like file system node: DriveNode or DirectoryNode that can have Children
     */
    public abstract class TraversableFSNode : FileLikeFSNode {
        public bool IsAccessible {
            get {
                return Directory.Exists (FullPath);

                //var asDirectoryInfo = FileSystemInfo as DirectoryInfo;
                //if (asDirectoryInfo != null) {
                //    try {
                //        asDirectoryInfo.EnumerateFileSystemInfos ();
                //    } catch (Exception exception) {
                //        MessageBox.Show (exception.Message);

                //        return false;
                //    }
                //}
                //return true;
            }
        }
        public LinkedList<FSNode> Children {
            get {
                var children = new LinkedList<FSNode> ();
                
                // TODO: UnauthorizedAccessException

                //if (IsAccessible) {
                //    var asDirectoryInfo = FileSystemInfo as DirectoryInfo;
                //    if (asDirectoryInfo != null) {
                //        foreach (var fileSystemInfo in asDirectoryInfo.EnumerateFileSystemInfos ()) {
                //            children.AddLast (new);
                //        }
                //    }
                //}

                try {
                    foreach (string directory in Directory.GetDirectories (FullPath)) {
                        DirectoryInfo directoryInfo = new DirectoryInfo (directory);
                        children.AddLast (new DirectoryNode (directoryInfo));
                    }

                    foreach (string file in Directory.GetFiles (FullPath)) {
                        FileInfo fileInfo = new FileInfo (file);
                        children.AddLast (new FileNode (fileInfo));
                    }
                } catch (Exception exception) {
                    MessageBox.Show (exception.Message);
                }

                return children;

                //var children = System.IO.DirectoryNode.GetDirectories (FullPath)
                //    .Select (directory => new DirectoryInfo (directory))
                //    .Select (directoryInfo => new DirectoryNode (directoryInfo)).Cast<FSNode> ().ToList ();
                //children.AddRange (System.IO.DirectoryNode.GetFiles (FullPath)
                //    .Select (file => new FileInfo (file))
                //    .Select (fileInfo => new FileNode (fileInfo)).Cast<FSNode> ());
            }
        }

        // TODO:
        //public FSNode Parent {
        //    get {
        //        // Directory.GetDirectoryRoot(dir)
        //        // Directory.GetCurrentDirectory()
        //        return new DirectoryNode (new DirectoryInfo (Path.Combine (FullPath + "\\..")));
        //    }
        //}
    }

    public class SystemRootNode : FSNode {
        public List<FSNode> Children {
            get { return new List<FSNode> (FSOps.EnumerateLocalDrives ()); }
        }

        public SystemRootNode () : this (System.Net.Dns.GetHostEntry ("localhost").HostName, Environment.MachineName) { }

        public SystemRootNode (string fullPath, string name) {
            Name = name;
            NodeLevel = NodeLevel.Root;
            FullPath = fullPath;
        }

        public override string ToString () {
            return String.Format ("{0} ({1})", Name, FullPath);
        }
    }

    public class DriveNode : TraversableFSNode {
        public DriveInfo DriveInfo { get; private set; }

        public string Name {
            get { return DriveInfo.IsReady ? DriveInfo.VolumeLabel : "<no label>"; }
        }

        public string FullPath {
            get { return DriveInfo.Name; }
        }

        public bool IsReady {
            get { return DriveInfo.IsReady; }
        }

        public LinkedList<FSNode> Children {
            get {
                if (IsReady) {
                    return base.Children;
                } else {
                    return new LinkedList<FSNode> ();
                }
            }
        }

        public DriveNode (DriveInfo driveInfo) {
            NodeLevel = NodeLevel.SubRoot;
            FileSystemInfo = driveInfo.RootDirectory;
            DriveInfo = driveInfo;
        }

        public DriveNode (string fullPath) : this (new DriveInfo (fullPath.ToUpperInvariant ())) { }

        public override string ToString () {
            return String.Format ("{0} ({1})", Name, FullPath);
        }
    }

    public class DirectoryNode : TraversableFSNode {
        public DirectoryNode (DirectoryInfo directoryInfo) {
            NodeLevel = NodeLevel.Internal;
            FileSystemInfo = directoryInfo;
        }

        public DirectoryNode (string fullPath, string name) : this (new DirectoryInfo (fullPath)) { }
    }

    public class FileNode : FileLikeFSNode {
        public FileNode (FileInfo fileInfo) {
            NodeLevel = NodeLevel.Leaf;
            FileSystemInfo = fileInfo;
        }

        public FileNode (string fullPath, string name) : this (new FileInfo (fullPath)) { }
    }
}