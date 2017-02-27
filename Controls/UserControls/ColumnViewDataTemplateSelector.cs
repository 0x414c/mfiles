using System.Windows;
using System.Windows.Controls;
using FSOps;


namespace Controls.UserControls {
    public class ColumnViewDataTemplateSelector : DataTemplateSelector {
        #region Overrides of DataTemplateSelector             
        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"/> based on custom logic.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.
        /// </returns>
        /// <param name="item">The data object for which to select the template.</param><param name="container">The data-bound object.</param>
        public override DataTemplate SelectTemplate (object item, DependencyObject container) {
            FrameworkElement element = container as FrameworkElement;

            if (element != null) {
                var itemAsFSNode = item as FSNode;

                if (itemAsFSNode != null) {
                    if (itemAsFSNode.TypeTag == TypeTag.Leaf) {
                        return element.FindResource ("file") as DataTemplate;
                    } else {
                        return element.FindResource ("directory") as DataTemplate;
                    }
                } else {
                    return null;
                }
            } else {
                return null;
            }
        }        
        #endregion
    }
}
