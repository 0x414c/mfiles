using System.Windows;

namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for Statusbar.xaml
    /// </summary>
    public partial class Statusbar {
        public Statusbar () {
            InitializeComponent ();
        }


        #region depProps
        public string CurrentItemInfo {
            get { return (string) GetValue (CurrentItemInfoProperty); }
            set { SetValue (CurrentItemInfoProperty, value); }
        }

        public static readonly DependencyProperty CurrentItemInfoProperty =
            DependencyProperty.Register (
                "CurrentItemInfo", typeof (string),
                typeof (Statusbar), new PropertyMetadata ("<current item is n/a>")
            );


        public string Status {
            get { return (string) GetValue (StatusProperty); }
            set { SetValue (StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register (
                "Status", typeof (string),
                typeof (Statusbar), new PropertyMetadata ("<status is n/a>")
            );


        public string ExtraStatus {
            get { return (string) GetValue (ExtraStatusProperty); }
            set { SetValue (ExtraStatusProperty, value); }
        }

        public static readonly DependencyProperty ExtraStatusProperty =
            DependencyProperty.Register (
                "ExtraStatus", typeof (string),
                typeof (Statusbar), new PropertyMetadata ("<extra status is n/a>")
            );
        #endregion
    }
}
