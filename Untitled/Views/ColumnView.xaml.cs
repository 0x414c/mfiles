using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Untitled.Models;


namespace Untitled {
    /// <summary>
    /// Interaction logic for ColumnView.xaml
    /// </summary>
    public partial class ColumnView : UserControl {
        private ColumnViewModel Model { get; set; }
        public ColumnView () {
            InitializeComponent ();
            Model = new ColumnViewModel ();
            Populate (); 
            DataContext = Model;
        }
        private void Populate () {
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives ()) {
                Model.FsNodes.Add (new Drive (driveInfo.Name, driveInfo.IsReady));
            }
        }
    }
}
