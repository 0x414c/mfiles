using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Untitled.Auxilliary;
using Untitled.LayoutManagers;
using Untitled.Models;


namespace Untitled {
    /// <summary>
    /// Interaction logic for MillerColumnsLayout.xaml
    /// </summary>
    public partial class MillerColumnsLayout : UserControl {
        public MillerColumnsLayout () {
            InitializeComponent ();
            AddColumn ();
        }

        private void AddColumn () {
            //columnsStack.Children.Add (new ColumnView ());
        }
    }
}
