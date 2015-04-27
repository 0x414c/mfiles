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
using Untitled.Annotations;
using Untitled.Auxilliary;
using Untitled.Models;


namespace Untitled {
    /// <summary>
    /// Interaction logic for FSNodeView.xaml
    /// </summary>
    public partial class FSNodeView : UserControl {
        public FSNodeViewModel Model { get; set; }
        
        public FSNodeView () {
            InitializeComponent ();
        }
        
        public FSNodeView (BasicFSNode basicFsNode) : this () {
            Model = new FSNodeViewModel { BasicFsNode = basicFsNode };
            DataContext = Model;
        }
    }
}
