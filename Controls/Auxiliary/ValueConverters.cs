using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FSOps;
using Microsoft.WindowsAPICodePack.Shell;


namespace Controls.Auxiliary {
    [ValueConversion (typeof (string), typeof (BitmapSource))]
    public class FSNodeToBitmapSourceConverter : IValueConverter {
        #region Implementation of IValueConverter
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
            var filePath = value as string;
            if (filePath != null) {
                return ShellFile.FromFilePath (filePath).Thumbnail.BitmapSource;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException ();
        }
        #endregion
    }

    [ValueConversion (typeof (FileLikeFSNode), typeof (string))]
    public class FileLikeFSNodeToDateConverter : IValueConverter {
        #region Implementation of IValueConverter
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
            var fileLikeFSNode = value as FileLikeFSNode;
            if (fileLikeFSNode != null) {
                var param = parameter as string;
                if (parameter != null) {
                    switch (param) {
                        case "created": return fileLikeFSNode.FileSystemInfo.CreationTimeUtc.ToString (CultureInfo.InvariantCulture);
                        case "modified": return fileLikeFSNode.FileSystemInfo.LastWriteTimeUtc.ToString (CultureInfo.InvariantCulture);
                        case "accessed": return fileLikeFSNode.FileSystemInfo.LastAccessTimeUtc.ToString (CultureInfo.InvariantCulture);
                    }   
                }
            } 
            
            return null;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException ();
        }
        #endregion
    }

    [ValueConversion (typeof (FileLikeFSNode), typeof (string))]
    public class FileLikeFSNodeToInfoConverter : IValueConverter {
        #region Implementation of IValueConverter
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
            var fileLikeFSNode = value as FileLikeFSNode;
            if (fileLikeFSNode != null) {
                var param = parameter as string;
                if (param != null) {
                    switch (param) {
                        case "attr": return fileLikeFSNode.FileSystemInfo.Attributes.ToString ();
                        case "size": return FSOps.FSOps.StrFormatByteSize (new FileInfo (fileLikeFSNode.FullPath).Length);
                    }
                }                                                                                      
            }

            return null;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException ();
        }
        #endregion
    }
}
