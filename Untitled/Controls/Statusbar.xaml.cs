using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Files.Controls;


namespace Files {
    /// <summary>
    /// Interaction logic for Statusbar.xaml
    /// </summary>
    public partial class Statusbar : UserControl {
        public StatusBarModel Model { get; set; }
       
        public Statusbar () {
            Model = new StatusBarModel ();
            InitializeComponent ();
            DataContext = Model;
        }
    }
}
