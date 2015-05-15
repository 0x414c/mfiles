using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;


namespace Controls.Auxiliary {
    public static class Utils {
        public static T FindVisualParent<T> (DependencyObject child) where T : DependencyObject {
            DependencyObject parentObject = VisualTreeHelper.GetParent (child);
            if (parentObject != null) {
                T parent = parentObject as T;
                if (parent != null) {
                    return parent;
                }
                return FindVisualParent<T> (parentObject);
            }
            return null;
        }

        //public static T GetVisualChild<T> (DependencyObject parent) where T : DependencyObject {
        //    T child = default (T);
        //    int numVisuals = VisualTreeHelper.GetChildrenCount (parent);
        //    for (int i = 0; i < numVisuals; i++) {
        //        DependencyObject v = VisualTreeHelper.GetChild (parent, i);
        //        child = v as T;
        //        if (child == null) {
        //            child = GetVisualChild<T> (v);
        //        }            
        //        if (child != null) {
        //            break;
        //        }
        //    }
        //    return child;
        //}

        public static T FindVisualChild<T> (DependencyObject obj) where T : DependencyObject {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount (obj); i++) {
                var child = VisualTreeHelper.GetChild (obj, i);
                if (child != null) {
                    var childAsT = child as T;
                    if (childAsT != null) {
                        return childAsT;
                    }
                } else {
                    var childOfChild = FindVisualChild<T> (child);
                    if (childOfChild != null) {
                        return childOfChild;
                    }
                }
            }
            return null;
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


    public class AutoScrollBehavior : Behavior<ScrollViewer> {
        private ScrollViewer _scrollViewer;
        private double _width;

        protected override void OnAttached () {
            base.OnAttached ();

            _scrollViewer = AssociatedObject;
            _scrollViewer.LayoutUpdated += _scrollViewer_LayoutUpdated;
        }

        private void _scrollViewer_LayoutUpdated (object sender, EventArgs e) {
            if (Math.Abs (_scrollViewer.ExtentWidth - _width) > Double.Epsilon) {
                _scrollViewer.ScrollToHorizontalOffset (_scrollViewer.ExtentWidth);
                _width = _scrollViewer.ExtentWidth;
            }
        }

        protected override void OnDetaching () {
            base.OnDetaching ();

            if (_scrollViewer != null) {
                _scrollViewer.LayoutUpdated -= _scrollViewer_LayoutUpdated;
            }
        }
    }
}