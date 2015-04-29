using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;


namespace Untitled.Auxilliary {
    static class FSOps {
        public static IEnumerable<DriveNode> EnumerateLocalDrives () {
            return DriveInfo.GetDrives ().Select (
                _ => new DriveNode (_)
            );
        }

        public static T TryGetConcreteNode<T> (FSNode fsNode) where T : FSNode {
            T node = fsNode as T;

            return node;
        }
    }
}
