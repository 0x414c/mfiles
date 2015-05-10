using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;


namespace Controls.UserControls {
    public class StatusBarModel : INotifyPropertyChanged {
        private string _itemInfo;

        public string ItemInfo {
            get { return _itemInfo; }
            set {
                if (value == _itemInfo) {
                    return;
                }
                _itemInfo = value;
                OnPropertyChanged ();
            }
        }

        private string _status;

        public string Status {
            get { return _status; }
            set {
                if (value == _status) {
                    return;
                }
                _status = value;
                OnPropertyChanged ();
            }
        }

        private string _extraStatus;

        public string ExtraStatus {
            get { return _extraStatus; }
            set {
                if (value == _extraStatus) {
                    return;
                }
                _extraStatus = value;
                OnPropertyChanged ();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
