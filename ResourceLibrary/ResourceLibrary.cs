using System;
using System.Windows;
using WPFExtensions;


namespace ResourceLibrary {
    public class RLStyleRefExtension : StyleRefExtension {
        static RLStyleRefExtension () {
            ResourceDictionary = new ResourceDictionary {
                Source = new Uri ("pack://application:,,,/ResourceLibrary;component/General.xaml")
            };
        }
    }
}
