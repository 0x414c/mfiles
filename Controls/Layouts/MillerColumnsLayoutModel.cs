using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;
using Controls.UserControls;


namespace Controls.Layouts {
    //[Serializable]
    public class MillerColumnsLayoutViewModel : INotifyPropertyChanged {
        private ObservableCollection<ColumnView> _columnViews;

        public ObservableCollection<ColumnView> ColumnViews {
            get { return _columnViews; }
            private set {
                if (Equals (value, _columnViews)) {
                    return;
                }
                _columnViews = value;
                OnPropertyChanged ();
            }
        }

        public MillerColumnsLayoutViewModel () {
            ColumnViews = new ObservableCollection<ColumnView> ();
        }

        //[field: NonSerialized]
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