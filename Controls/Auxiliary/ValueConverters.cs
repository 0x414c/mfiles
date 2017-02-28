using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FSOps;
using Microsoft.WindowsAPICodePack.Shell;


namespace Controls.Auxiliary {
    [ValueConversion (typeof (string), typeof (BitmapSource))]
    public class FilePathToBitmapSourceConverter : IValueConverter {
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
                return ShellFile.FromFilePath(filePath).Thumbnail.ExtraLargeBitmapSource;
                //return null;
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

    [ValueConversion (typeof (FileFSNode), typeof (string))]
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
            var fileLikeFSNode = value as FileFSNode;
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

    [ValueConversion (typeof (FileFSNode), typeof (string))]
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
            var fileLikeFSNode = value as FileFSNode;
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

    [ValueConversion (typeof (FSNode), typeof (string))]
    public class FSNodeTypeTagToIconSourcePathConverter : IValueConverter {
        #region Implementation of IValueConverter
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
            var fsNode = value as FSNode;
            if (fsNode != null) {  
                switch (fsNode.TypeTag) {
                    case TypeTag.Leaf: return "pack://application:,,,/Controls;component/Graphics/Textfile_818_32x.png";
                    case TypeTag.Internal: return "pack://application:,,,/Controls;component/Graphics/folder_Closed_32xMD.png";
                    case TypeTag.SubRoot: return "pack://application:,,,/Controls;component/Graphics/Hardrive_v_5169_16xLG.png";
                    case TypeTag.Root: return "pack://application:,,,/Controls;component/Graphics/computersystemproduct.png";
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
