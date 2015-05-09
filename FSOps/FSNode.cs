using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;


namespace FSOps {
    /**
     \enum  TypeTag
     \brief Represents filesystem node: drive, directory or file.
      Root for an entire Machine, SubRoot for particular Drives,
      Internal for Directories, Leaf for Files
     */
    [Flags]
    public enum TypeTag {
        Leaf = 1,
        Internal = 2,
        SubRoot = 4,
        Root = 8
    }


    /**
     \class FSNode    
     \brief A file system node.
     */
    public abstract class FSNode {
        public TypeTag TypeTag { get; set; }

        public string Name { get; set; }

        public string FullPath { get; set; }

        public bool Is (TypeTag compareTo) {
            return (TypeTag & compareTo) == TypeTag;
        }

        public override string ToString () {
            return Name;
        }
    }


    /**
     \class FileLikeFSNode
     \brief A file-like file system node: DriveNode, DirectoryNode or FileNode.      
     */
    public abstract class FileLikeFSNode : FSNode {
        public virtual FileSystemInfo FileSystemInfo { get; set; }

        public new string Name {
            get { return FileSystemInfo.Name; }
        }

        public new string FullPath {
            get { return FileSystemInfo.FullName; }
        }

        public override string ToString () {
            return Name;
        }

        public bool IsAccessible {
            get { return FileSystemInfo.Exists && FileManagement.HasRights (FileSystemInfo as FileInfo, FileSystemRights.ReadData); }
        }

        // TODO:
        public DirectoryLikeFSNode Parent {
            get {
                throw new NotImplementedException ();

                //var asDirectoryInfo = FileSystemInfo as DirectoryInfo;
                //if (asDirectoryInfo != null) {
                //    return new DirectoryNode (asDirectoryInfo.Parent);
                //} else {
                //    var asFileInfo = FileSystemInfo as FileInfo;
                //    if (asFileInfo != null) {
                //        if (asFileInfo.Directory != null) {
                //            return new DirectoryNode (asFileInfo.Directory.Parent);
                //        } else {
                //            return null;
                //        }
                //    } else {
                //        return null;
                //    }
                //}
            }
        }
    }


    /**
     \class DirectoryLikeFSNode    
     \brief A directory-like file system node: DriveNode or DirectoryNode that can have Children
     */
    public abstract class DirectoryLikeFSNode : FileLikeFSNode {
        public new bool IsAccessible {
            get { return FileSystemInfo.Exists && FileManagement.HasRights (FileSystemInfo as DirectoryInfo, FileSystemRights.ListDirectory); }
        }

        public bool IsTraversable {
            get { return FileSystemInfo.Exists && FileManagement.HasRights (FileSystemInfo as DirectoryInfo, FileSystemRights.Traverse); }
        }

        public virtual IEnumerable<FileLikeFSNode> Children {
            get {
                if (IsAccessible && IsTraversable) {
                    var asDirectoryInfo = FileSystemInfo as DirectoryInfo;
                    if (asDirectoryInfo != null) {
                        var children = new LinkedList<FileLikeFSNode> ();

                        foreach (var directoryInfo in asDirectoryInfo.EnumerateDirectories ()) {
                            children.AddLast (new DirectoryNode (directoryInfo));
                        }

                        foreach (var fileInfo in asDirectoryInfo.EnumerateFiles ()) {
                            children.AddLast (new FileNode (fileInfo));
                        }

                        return children;
                    } else {
                        return null;
                    }
                } else {
                    return null;
                }
            }
        }
    }


    /**
     \class SystemRootNode    
     \brief A system root node.
     */
    public sealed class SystemRootNode : FSNode {
        public IEnumerable<FSNode> Children {
            get { return new List<FSNode> (FileManagement.EnumerateLocalDrives ()); }
        }

        public SystemRootNode () : this ("This PC", Networking.GetLocalFQDN ()) { }

        // For future use
        private SystemRootNode (string fullPath, string name) {
            Name = name;
            TypeTag = TypeTag.Root;
            FullPath = fullPath;
        }

        public override string ToString () {
            return String.Format ("{0} ({1})", Name, FullPath);
        }
    }


    /// <summary>
    /// Represents particular Drive
    /// </summary>
    public sealed class DriveNode : DirectoryLikeFSNode {
        public DriveInfo DriveInfo { get; set; }

        public override FileSystemInfo FileSystemInfo {
            get { return DriveInfo.RootDirectory; }
        }

        public new string Name {
            get { return DriveInfo.IsReady ? DriveInfo.VolumeLabel : "<no label>"; }
        }

        public new string FullPath {
            get { return DriveInfo.Name; }
        }

        public bool IsReady {
            get { return DriveInfo.IsReady; }
        }

        public override IEnumerable<FileLikeFSNode> Children {
            get { return IsReady ? base.Children : null; }
        }

        public DriveNode (DriveInfo driveInfo) {
            TypeTag = TypeTag.SubRoot;
            DriveInfo = driveInfo;
        }

        public DriveNode (string fullPath) : this (new DriveInfo (fullPath.ToUpperInvariant ())) { }

        public override string ToString () {
            return String.Format ("{0} ({1})", Name, FullPath);
        }
    }

    
    public sealed class DirectoryNode : DirectoryLikeFSNode {
        public DirectoryNode (DirectoryInfo directoryInfo) {
            TypeTag = TypeTag.Internal;
            FileSystemInfo = directoryInfo;
        }

        public DirectoryNode (string fullPath) : this (new DirectoryInfo (fullPath)) { }
    }


    public sealed class FileNode : FileLikeFSNode {
        public FileNode (FileInfo fileInfo) {
            TypeTag = TypeTag.Leaf;
            FileSystemInfo = fileInfo;
        }

        public FileNode (string fullPath) : this (new FileInfo (fullPath)) { }
    }
}               
