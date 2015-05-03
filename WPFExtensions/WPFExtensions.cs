using System;
using System.Windows;
using System.Windows.Markup;


namespace WPFExtensions {
    public abstract class StyleRefExtension : MarkupExtension {
        /// <summary>
        /// Property for specific resource dictionary
        /// </summary>
        protected static ResourceDictionary ResourceDictionary;

        /// <summary>
        /// Resource key wich we want to extract
        /// </summary>
        public string ResourceKey { private get; set; }

        /// <summary>
        /// Overriding base function wich will return key from _resourceDictionary
        /// </summary>
        /// <param name="serviceProvider">Not used</param>
        /// <returns>Object from _resourceDictionary</returns>
        public override object ProvideValue (IServiceProvider serviceProvider) {
            if (ResourceDictionary == null) {
                throw new Exception (@"You should define ResourceDictionary in static constructor of extending class before usage.");
            } else {
                return ResourceDictionary[ResourceKey];
            }   
        }
    }
}
