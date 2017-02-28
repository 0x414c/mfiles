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
        public TypeTag TypeTag { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual string FullPath { get; protected set; }


        public bool Is (TypeTag compareTo) => (TypeTag & compareTo) == TypeTag;


        public override string ToString () => Name;
    }


    /**
     \class FileFSNode
     \brief A file-like file system node: DriveNode, DirectoryNode or FileNode.      
     */
    public abstract class FileFSNode : FSNode {
        public override string Name => FileSystemInfo.Name;

        public override string FullPath => FileSystemInfo.FullName;

        public virtual FileSystemInfo FileSystemInfo { get; protected set; }

        public virtual bool IsAccessible => FileSystemInfo.Exists && FSOps.HasRights (FileSystemInfo as FileInfo, FileSystemRights.ReadData);

        public DriveNode RootDrive => new DriveNode (Path.GetPathRoot (FileSystemInfo.FullName));

        public DirectoryFSNode Parent {
            get {
                var asDirectoryInfo = FileSystemInfo as DirectoryInfo;
                if (asDirectoryInfo != null) {
                    return new DirectoryNode (asDirectoryInfo.Parent);
                } else {
                    var asFileInfo = FileSystemInfo as FileInfo;
                    if (asFileInfo != null) {
                        if (asFileInfo.Directory != null) {
                            return new DirectoryNode (asFileInfo.Directory);
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
     \class DirectoryFSNode    
     \brief A directory-like file system node: DriveNode or DirectoryNode that can have Children
     */
    public abstract class DirectoryFSNode : FileFSNode {
        public override bool IsAccessible => FileSystemInfo.Exists && FSOps.HasRights (FileSystemInfo as DirectoryInfo, FileSystemRights.ListDirectory);

        public bool IsTraversable => FileSystemInfo.Exists && FSOps.HasRights (FileSystemInfo as DirectoryInfo, FileSystemRights.Traverse);

        public virtual IEnumerable<FileFSNode> Children {
            get {
                if (IsAccessible && IsTraversable) {
                    var asDirectoryInfo = FileSystemInfo as DirectoryInfo;
                    
                    if (asDirectoryInfo != null) {
                        var children = new LinkedList<FileFSNode> ();

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
        // TODO: [0;0] Derive from DirectoryNode? 
        public IEnumerable<FSNode> Children => new List<FSNode> (FSOps.EnumerateLocalDrives ());

        public SystemRootNode () : this ("This PC", Networking.GetLocalFQDN ()) { }


        // TODO: [?;?] For future use.
        private SystemRootNode (string fullPath, string name) {
            Name = name;
            TypeTag = TypeTag.Root;
            FullPath = fullPath;
        }


        public override string ToString () => $"{Name} ({FullPath})";
    }


    /// <summary>
    /// Represents particular Drive
    /// </summary>
    public sealed class DriveNode : DirectoryFSNode {
        public DriveInfo DriveInfo { get; }

        public override FileSystemInfo FileSystemInfo => DriveInfo.RootDirectory;

        public override string Name => DriveInfo.IsReady ? DriveInfo.VolumeLabel : "<no label>";

        public override string FullPath => DriveInfo.Name;

        public bool IsReady => DriveInfo.IsReady;

        public override IEnumerable<FileFSNode> Children => IsReady ? base.Children : null;


        public DriveNode (DriveInfo driveInfo) {
            TypeTag = TypeTag.SubRoot;
            DriveInfo = driveInfo;
        }

        public DriveNode (string fullPath) : this (new DriveInfo (fullPath.ToUpperInvariant ())) { }


        public override string ToString () => $"{Name} ({FullPath})";
    }

    
    public sealed class DirectoryNode : DirectoryFSNode {
        public DirectoryNode (DirectoryInfo directoryInfo) {
            TypeTag = TypeTag.Internal;
            FileSystemInfo = directoryInfo;
        }

        public DirectoryNode (string fullPath) : this (new DirectoryInfo (fullPath)) { }
    }


    public sealed class FileNode : FileFSNode {
        public FileNode (FileInfo fileInfo) {
            TypeTag = TypeTag.Leaf;
            FileSystemInfo = fileInfo;
        }

        public FileNode (string fullPath) : this (new FileInfo (fullPath)) { }
    }
}               
