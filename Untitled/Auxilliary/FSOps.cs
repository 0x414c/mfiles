using System.Collections.ObjectModel;
using System.IO;


namespace Untitled.Auxilliary {
    public abstract class FSNode {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public override string ToString () {
            return Name;
        }
    }
    public class File : FSNode {
        public File (string fullPath, string name) {
            FullPath = fullPath;
            Name = name;
        }
    }
    public class Directory : FSNode {
        public Directory (string fullPath, string name) {
            FullPath = fullPath;
            Name = name;
            Children = new ObservableCollection<object> ();
        }
        public ObservableCollection<object> Children { get; private set; }
    }
    public class Drive : FSNode {
        public Drive (string name, bool isReady) {
            Name = name;
            IsReady = isReady;
            Children = new ObservableCollection<object> ();
        }
        public bool IsReady { get; set; }
        public ObservableCollection<object> Children { get; private set; }
    }
    sealed class ServiceFacade {
        private static ServiceFacade instance;
        public static ServiceFacade Instance {
            get {
                if (instance == null) {
                    instance = new ServiceFacade ();
                    instance.Initialize ();
                }
                return instance;
            }
        }
        public ObservableCollection<Drive> Drives { get; private set; }
        private void Initialize () {
            Drives = new ObservableCollection<Drive> ();
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives ()) {
                Drives.Add (new Drive (driveInfo.Name, driveInfo.IsReady));
            }
        }
    }
    class FSOps {

    }
}
