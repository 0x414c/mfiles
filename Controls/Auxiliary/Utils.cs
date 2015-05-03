using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace Controls.Auxiliary {
    public static class Utils {
        public static T FindParent<T> (DependencyObject child) where T : DependencyObject {
            // get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent (child);
            // if we've reached the end of the tree
            if (parentObject == null) {
                return null;
            }
            // check if the parent matches the type we're looking for
            // otherwise climb down the tree
            T parent = parentObject as T;
            if (parent != null) {
                return parent;
            } else {
                return FindParent<T> (parentObject);
            }
        }

        public static int Remove<T> (this ObservableCollection<T> collection, Func<T, bool> condition) {
            var itemsToRemove = collection.Where (condition).ToList ();

            foreach (var itemToRemove in itemsToRemove) {
                collection.Remove (itemToRemove);
            }

            return itemsToRemove.Count;
        }

        public static void RemoveAll<T> (this ObservableCollection<T> collection, Func<T, bool> condition) {
            for (int i = collection.Count - 1; i >= 0; i--) {
                if (condition (collection[i])) {
                    collection.RemoveAt (i);
                }
            }
        }
    }
}