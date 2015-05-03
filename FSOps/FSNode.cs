using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;


namespace FSOps {
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
        public NodeLevel NodeLevel { get; set; }

        public string Name { get; set; }

        public string FullPath { get; set; }

        public override string ToString () {
            return Name;
        }
    }


    /**
     \class FileLikeFSNode
     \brief A file like file system node: DriveNode, DirectoryNode or FileNode.      
     */
    public abstract class FileLikeFSNode : FSNode {
        public FileSystemInfo FileSystemInfo { get; set; }

        public string Name {
            get { return FileSystemInfo.Name; }
        }

        public string FullPath {
            get { return FileSystemInfo.FullName; }
        }

        public override string ToString () {
            return Name;
        }

        public TraversableFSNode Parent {
            get {
                var asDirectoryInfo = FileSystemInfo as DirectoryInfo;
                if (asDirectoryInfo != null) {
                    return new DirectoryNode (asDirectoryInfo.Parent);
                } else {
                    var asFileInfo = FileSystemInfo as FileInfo;
                    if (asFileInfo != null) {
                        if (asFileInfo.Directory != null) {
                            return new DirectoryNode (asFileInfo.Directory.Parent);
                        } else {
                            return null;
                        }
                    } else {
                        return null;
                    }
                }
            }
        }
    }


    /**
     \class TraversableFSNode    
     \brief A directory-like file system node: DriveNode or DirectoryNode that can have Children
     */
    public abstract class TraversableFSNode : FileLikeFSNode {
        // TODO:
        public bool IsAccessible {
            get {
                var asDirectoryInfo = FileSystemInfo as DirectoryInfo;
                
                if (asDirectoryInfo != null) {
                    try {
                        asDirectoryInfo.GetFileSystemInfos ();
                    } catch (Exception exception) {
                        MessageBox.Show (exception.Message);

                        return false;
                    }
                }

                return true;
            }
        }

        public LinkedList<FSNode> Children {
            get {
                var children = new LinkedList<FSNode> ();

                if (IsAccessible) {
                    foreach (string directory in Directory.GetDirectories (FullPath)) {
                        DirectoryInfo directoryInfo = new DirectoryInfo (directory);
                        children.AddLast (new DirectoryNode (directoryInfo));
                    }

                    foreach (string file in Directory.GetFiles (FullPath)) {
                        FileInfo fileInfo = new FileInfo (file);
                        children.AddLast (new FileNode (fileInfo));
                    }

                    return children;
                } else {
                    return null;
                }
            }
        }
    }


    public class SystemRootNode : FSNode {
        public List<FSNode> Children {
            get { return new List<FSNode> (FileManagement.EnumerateLocalDrives ()); }
        }

        public SystemRootNode () : this (
            "This PC",
            Networking.GetFQDN ()
        ) { }

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
        public DriveInfo DriveInfo { get; set; }

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
                    return null;
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

        public DirectoryNode (string fullPath) : this (new DirectoryInfo (fullPath)) { }
    }


    public class FileNode : FileLikeFSNode {
        public FileNode (FileInfo fileInfo) {
            NodeLevel = NodeLevel.Leaf;
            FileSystemInfo = fileInfo;
        }

        public FileNode (string fullPath) : this (new FileInfo (fullPath)) { }
    }
}