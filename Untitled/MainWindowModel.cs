using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;
using Controls.Layouts;


namespace FilesApplication {
    public sealed class MainWindowModel : INotifyPropertyChanged {
        private ObservableCollection<MillerColumnsLayout> _layouts;

        public ObservableCollection<MillerColumnsLayout> Layouts {
            get { return _layouts; }
            private set {
                if (Equals (value, _layouts)) {
                    return;
                }
                _layouts = value;
                OnPropertyChanged ();
            }
        }

        public MainWindowModel () {
            Layouts = new ObservableCollection<MillerColumnsLayout> ();
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
