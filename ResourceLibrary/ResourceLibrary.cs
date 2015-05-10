using System;
using System.Windows;


namespace ResourceLibrary {
    public class GetStyleExtension : GetStyleMarkupExtension {
        static GetStyleExtension () {
            ResourceDictionary = new ResourceDictionary {
                Source = new Uri ("pack://application:,,,/ResourceLibrary;component/General.xaml")
            };
        }
    }
}
