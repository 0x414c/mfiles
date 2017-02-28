using System;
using System.Reflection;
using System.Windows;


namespace Files {
    public static class ReflectionUtils {

        // the dpName parameter is the actual DependencyProperty name (ContentProperty) and not the property name (Content)
        public static DependencyProperty GetDependencyPropertyByName (DependencyObject dependencyObject, string dpName) {
            return GetDependencyPropertyByName (dependencyObject.GetType (), dpName);
        }

        public static DependencyProperty GetDependencyPropertyByName (Type dependencyObjectType, string dpName) {
            DependencyProperty dp = null;

            var fieldInfo = dependencyObjectType.GetField (
                dpName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
            );

            if (fieldInfo != null) {
                dp = fieldInfo.GetValue (null) as DependencyProperty;
            }

            return dp;
        }
    }
}
