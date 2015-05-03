using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controls.Annotations;


namespace Controls.UserControls {
    public class StatusBarModel : INotifyPropertyChanged {
        private int _itemsCount;
        private int _availableSpace;

        public int ItemsCount {
            get { return _itemsCount; }
            set {
                if (value == _itemsCount) {
                    return;
                }
                _itemsCount = value;
                OnPropertyChanged ();
            }
        }

        public int AvailableSpace {
            get { return _availableSpace; }
            set {
                if (value == _availableSpace) {
                    return;
                }
                _availableSpace = value;
                OnPropertyChanged ();
            }
        }

        public override string ToString () {
            return "{0} items, {1} available";
            //return String.Format ("{0} items, {1} available", ItemsCount, AvailableSpace);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
