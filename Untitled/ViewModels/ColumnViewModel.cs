using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Untitled.Annotations;
using Untitled.Auxilliary;


namespace Untitled.Models {
    public sealed class ColumnViewModel: INotifyPropertyChanged {
        public ObservableCollection<FSNode> FsNodes { get; set; }
        public ColumnViewModel () {
            FsNodes = new ObservableCollection<FSNode> ();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}