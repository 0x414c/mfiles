using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FSOps {
    public static class FileManagement {
        public static IEnumerable<DriveNode> EnumerateLocalDrives () {
            return DriveInfo.GetDrives ().Select (
                _ => new DriveNode (_)
            );
        }

        public static T TryGetConcreteFSNode<T> (FSNode fsNode) where T : FSNode {
            T node = fsNode as T;

            return node;
        }
    }
}
