using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;
using FSOps;


namespace Files {
    public sealed class ClipboardWindowViewModel : INotifyPropertyChanged {
        private ClipboardStack<FileLikeFSNode> _clipboardStack;

        public ClipboardStack<FileLikeFSNode> ClipboardStack {
            get { return _clipboardStack; }
            set {
                if (Equals (value, _clipboardStack)) {
                    return;
                }
                _clipboardStack = value;
                OnPropertyChanged ();
            }
        }

        public ClipboardWindowViewModel () {
            ClipboardStack = new ClipboardStack<FileLikeFSNode> ();
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
