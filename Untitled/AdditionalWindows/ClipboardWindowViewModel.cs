using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;
using FSOps;


namespace Files {
    public sealed class ClipboardWindowViewModel : INotifyPropertyChanged {
        private ClipboardStack<FileFSNode> _clipboardStack;

        public ClipboardStack<FileFSNode> ClipboardStack {
            get { return _clipboardStack; }
            set {
                if (Equals (value, _clipboardStack)) {
                    return;
                } else {
                    _clipboardStack = value;
                    OnPropertyChanged ();
                }
            }
        }


        public ClipboardWindowViewModel () {
            ClipboardStack = new ClipboardStack<FileFSNode> ();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
