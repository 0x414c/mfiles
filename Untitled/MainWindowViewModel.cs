using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;
using Controls.Layouts;


namespace FilesApplication {
    [Serializable]
    public sealed class MainWindowModel : INotifyPropertyChanged {
        private ObservableCollection<MillerColumnsLayout> _layouts;

        public ObservableCollection<MillerColumnsLayout> Layouts {
            get { return _layouts; }
            private set {
                if (Equals (value, _layouts)) {
                    return;
                } else {
                    _layouts = value;
                    OnPropertyChanged ();
                }
            }
        }


        public MainWindowModel () {
            Layouts = new ObservableCollection<MillerColumnsLayout> ();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
