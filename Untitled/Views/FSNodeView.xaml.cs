using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Files.Auxilliary;
using Files.Models;
using Files.Annotations;


namespace Files {
    /// <summary>
    /// Interaction logic for FSNodeView.xaml
    /// </summary>
    public partial class FSNodeView : ContentControl {
        public FSNodeViewModel Model { get; private set; }

        private FSNodeView () {
            InitializeComponent ();
        }

        public FSNodeView (FSNode fsNode) : this () {
            Model = new FSNodeViewModel (fsNode);
            DataContext = Model;
        }
    }
}
