using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Untitled.Annotations;
using Untitled.Auxilliary;


namespace Untitled.Models {
    public sealed class ColumnViewModel : INotifyPropertyChanged {
        private ObservableCollection<FSNodeView> _childFsNodesViews;
        private ObservableCollection<FSNodeView> _parentFsNodesViews;

        public ObservableCollection<FSNodeView> ChildFsNodesViews {
            get { return _childFsNodesViews; }
            set {
                if (Equals (value, _childFsNodesViews)) {
                    return;
                }
                _childFsNodesViews = value;
                OnPropertyChanged ("ChildFsNodesViews");
            }
        }

        public ObservableCollection<FSNodeView> ParentFsNodesViews {
            get { return _parentFsNodesViews; }
            set {
                if (Equals (value, _parentFsNodesViews)) {
                    return;
                }
                _parentFsNodesViews = value;
                OnPropertyChanged ("ParentFsNodesViews");
            }
        }

        public ColumnViewModel () {
            ChildFsNodesViews = new ObservableCollection<FSNodeView> ();
            ParentFsNodesViews = new ObservableCollection<FSNodeView> ();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}