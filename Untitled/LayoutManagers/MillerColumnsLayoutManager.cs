using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Untitled.Annotations;
using Untitled.Auxilliary;
using Untitled.Models;


namespace Untitled.LayoutManagers {
    public class MillerColumnsLayoutManager : INotifyPropertyChanged {
        private ObservableCollection<ColumnView> _columnViews;

        public ObservableCollection<ColumnView> ColumnViews {
            get { return _columnViews; }
            set {
                if (Equals (value, _columnViews)) {
                    return;
                }
                _columnViews = value;
                OnPropertyChanged ("ColumnViews");
            }
        }

        public MillerColumnsLayoutManager () {
            ColumnViews = new ObservableCollection<ColumnView> ();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }

    public static class Utils {
        public static T FindParent<T> (DependencyObject child) where T : DependencyObject {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent (child);
            //we've reached the end of the tree
            if (parentObject == null) {
                return null;
            }
            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null) {
                return parent;
            } else {
                return FindParent<T> (parentObject);
            }
        }
    }
}