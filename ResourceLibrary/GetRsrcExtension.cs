using System;
using System.Windows;


namespace ResourceLibrary {
    public class GetRsrcExtension : GetRsrcMarkupExtension {
        static GetRsrcExtension () {
            ResourceDictionary = new ResourceDictionary {
                Source = new Uri ("pack://application:,,,/ResourceLibrary;component/General.xaml")
            };
        }
    }
}
