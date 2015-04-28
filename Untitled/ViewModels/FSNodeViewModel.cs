using System.ComponentModel;
using System.Runtime.CompilerServices;
using Untitled.Annotations;
using Untitled.Auxilliary;


namespace Untitled.Models {
    public class FSNodeViewModel: INotifyPropertyChanged {
        private FSNode _FSNode;

        public FSNode FSNode {
            get { return _FSNode; }
            set {
                if (Equals (value, _FSNode)) {
                    return;
                }
                _FSNode = value;
                OnPropertyChanged ("FSNode");
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