using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Untitled.Annotations;


namespace Untitled.Controls {
    public class StatusBarModel {
        public int ItemsCount {
            get { return 99; }
        }

        public int AvailableSpace {
            get { return 42; }
        }

        public override string ToString () {
            return String.Format ("{0} items, {1} available", ItemsCount, AvailableSpace);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
