using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Untitled.Annotations;
using Untitled.Auxilliary;


namespace Untitled.Models {
    public sealed class ColumnViewModel : INotifyPropertyChanged {
        private ObservableCollection<FSNodeView> _childFSNodesViews;
        private ObservableCollection<FSNodeView> _parentFSNodesViews;

        public ObservableCollection<FSNodeView> ChildFSNodesViews {
            get { return _childFSNodesViews; }
            set {
                if (Equals (value, _childFSNodesViews)) {
                    return;
                }
                _childFSNodesViews = value;
                OnPropertyChanged ("ChildFSNodesViews");
            }
        }

        public ObservableCollection<FSNodeView> ParentFSNodesViews {
            get { return _parentFSNodesViews; }
            set {
                if (Equals (value, _parentFSNodesViews)) {
                    return;
                }
                _parentFSNodesViews = value;
                OnPropertyChanged ("ParentFSNodesViews");
            }
        }

        public ColumnViewModel () {
            _childFSNodesViews = new ObservableCollection<FSNodeView> ();
            _parentFSNodesViews = new ObservableCollection<FSNodeView> ();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}