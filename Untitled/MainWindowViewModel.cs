using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Xml.Serialization;
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
                }
                _layouts = value;
                OnPropertyChanged ();
            }
        }

        public MainWindowModel () {
            Layouts = new ObservableCollection<MillerColumnsLayout> ();
        }

        [field: NonSerialized]
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
