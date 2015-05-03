using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;
using FSOps;


namespace Controls.UserControls {
    public class FSNodeViewModel : INotifyPropertyChanged {
        private FSNode _fsNode;

        public FSNode FSNode {
            get { return _fsNode; }
            set {
                if (Equals (value, _fsNode)) {
                    return;
                }
                _fsNode = value;
                OnPropertyChanged ();
            }
        }

        public FSNodeViewModel (FSNode fsNode) {
            _fsNode = fsNode;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}