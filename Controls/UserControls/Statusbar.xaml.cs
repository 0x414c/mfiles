using System.Windows;
using System.Windows.Controls;


namespace Controls.UserControls {
    /// <summary>
    /// Interaction logic for Statusbar.xaml
    /// </summary>
    public partial class Statusbar : UserControl {
        //public StatusBarModel ViewModel { get; set; }
       
        public Statusbar () {             
            InitializeComponent ();

            //ViewModel = new StatusBarModel ();
            //DataContext = ViewModel;
        }

        #region depProps
        public string CurrentItemInfo {
            get { return (string) GetValue (CurrentItemInfoProperty); }
            set { SetValue (CurrentItemInfoProperty, value); }
        }

        public static readonly DependencyProperty CurrentItemInfoProperty =
            DependencyProperty.Register (
                "CurrentItemInfo", typeof (string),
                typeof (Statusbar), new PropertyMetadata ("<no item is currently selected>")
            );

        
        public string Status {
            get { return (string) GetValue (StatusProperty); }
            set { SetValue (StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register (
                "Status", typeof (string), 
                typeof (Statusbar), new PropertyMetadata ("<app is not ready (yet)>")
            );


        public string ExtraStatus {
            get { return (string) GetValue (ExtraStatusProperty); }
            set { SetValue (ExtraStatusProperty, value); }
        }

        public static readonly DependencyProperty ExtraStatusProperty =
            DependencyProperty.Register (
                "ExtraStatus", typeof (string),
                typeof (Statusbar), new PropertyMetadata ("<current status is unknown (yet)>")
            );
        #endregion
    }
}
