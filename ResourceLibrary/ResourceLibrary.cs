using System;
using System.Windows;
using WPFExtensions;


namespace ResourceLibrary {
    public class MyStyleRefExtension : StyleRefExtension {
        static MyStyleRefExtension () {
            ResourceDictionary = new ResourceDictionary {
                Source = new Uri ("pack://application:,,,/ResourceLibrary;component/General.xaml")
            };
        }
    }
}
