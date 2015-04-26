using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Untitled.Annotations;
using Untitled.Auxilliary;


namespace Untitled.Models {
    public sealed class ColumnViewModel: INotifyPropertyChanged {
        public ObservableCollection<FSNodeView> ChildFsNodesViews { get; set; }
        public ObservableCollection<FSNodeView> ParentFsNodesViews { get; set; }
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