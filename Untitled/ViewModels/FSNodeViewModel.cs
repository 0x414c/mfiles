using System.ComponentModel;
using System.Runtime.CompilerServices;
using Untitled.Annotations;
using Untitled.Auxilliary;


namespace Untitled.Models {
    public class FSNodeViewModel: INotifyPropertyChanged {
        private BasicFSNode _basicFsNode;

        public BasicFSNode BasicFsNode {
            get { return _basicFsNode; }
            set {
                if (Equals (value, _basicFsNode)) {
                    return;
                }
                _basicFsNode = value;
                OnPropertyChanged ("BasicFsNode");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}