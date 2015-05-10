using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;
using FSOps;


namespace Controls.UserControls {
    public sealed class FSNodeViewModel : INotifyPropertyChanged {
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler (this, new PropertyChangedEventArgs (propertyName));
            }
        }
    }
}