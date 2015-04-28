using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;


namespace Untitled.Auxilliary {
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
     \brief A file like file system node: Drive, Directory or File.
      Can be instanced using FileSystemInfo only.
     */
    public abstract class FileLikeFSNode : FSNode {
        public FileSystemInfo FileSystemInfo { get; protected set; }

        public new string Name {
            get { return FileSystemInfo.Name; }
        }

        public new string FullPath {
            get { return FileSystemInfo.FullName; }
        }

        public override string ToString () {
            return Name;
        }
    }


    /**
     \class TraversableFSNode    
     \brief A directory like file system node: Drive or Directory
      that can have Children
     */
    public abstract class TraversableFSNode : FileLikeFSNode {
        public List<FSNode> Children {
            get {
                //var children = new List<FSNode> ();
                //foreach (string directory in System.IO.Directory.GetDirectories (FullPath)) {
                //    DirectoryInfo directoryInfo = new DirectoryInfo (directory);
                //    children.Add (new Directory (directoryInfo));
                //    //children.Add (new Directory (directory, directoryInfo.Name));
                //}
                //foreach (string file in System.IO.Directory.GetFiles (FullPath)) {
                //    FileInfo fileInfo = new FileInfo (file);
                //    children.Add (new File (fileInfo));
                //    //children.Add (new File (file, fileInfo.Name));
                //}

                var children = System.IO.Directory.GetDirectories (FullPath)
                    .Select (directory => new DirectoryInfo (directory))
                    .Select (directoryInfo => new Directory (directoryInfo)).Cast<FSNode> ().ToList ();
                children.AddRange (System.IO.Directory.GetFiles (FullPath)
                    .Select (file => new FileInfo (file))
                    .Select (fileInfo => new File (fileInfo)).Cast<FSNode> ());

                return children;
            }
        }
    }


    public class SystemRoot : FSNode {
        public List<FSNode> Children {
            get { return new List<FSNode> (FSOps.EnumerateLocalDrives ()); }
        }

        public SystemRoot () : this (System.Net.Dns.GetHostEntry ("localhost").HostName, Environment.MachineName) { }

        public SystemRoot (string fullPath, string name) {
            Name = name;
            NodeLevel = NodeLevel.Root;
            FullPath = fullPath;
        }

        public override string ToString () {
            return String.Format ("{0} ({1})", base.ToString (), FullPath);
        }
    }


    public class Drive : TraversableFSNode {
        public DriveInfo DriveInfo { get; private set; }

        public new string Name {
            get { return DriveInfo.IsReady ? DriveInfo.VolumeLabel : "<no label>"; }
        }

        public new string FullPath {
            get { return DriveInfo.Name; }
        }

        public bool IsReady {
            get { return DriveInfo.IsReady; }
        }

        public Drive (DriveInfo driveInfo) {
            NodeLevel = NodeLevel.SubRoot;
            FileSystemInfo = driveInfo.RootDirectory;
            DriveInfo = driveInfo;
        }

        public override string ToString () {
            return String.Format ("{0} ({1})", Name, FullPath);
        }
    }


    public class Directory : TraversableFSNode {
        public Directory (DirectoryInfo directoryInfo) {
            NodeLevel = NodeLevel.Internal;
            FileSystemInfo = directoryInfo;
        }

        public Directory (string fullPath, string name) : this (new DirectoryInfo (fullPath)) { }

        //public override string ToString () {
        //    return String.Format ("{0} ({1})", Name, FullPath);
        //}
    }


    public class File : FileLikeFSNode {
        public File (FileInfo fileInfo) {
            NodeLevel = NodeLevel.Leaf;
            FileSystemInfo = fileInfo;
        }

        public File (string fullPath, string name) : this (new FileInfo (fullPath)) { }
    }


    static class FSOps {
        public static IEnumerable<Drive> EnumerateLocalDrives () {
            return DriveInfo.GetDrives ().Select (
                _ => new Drive (_)
            );
        }
    }
}
