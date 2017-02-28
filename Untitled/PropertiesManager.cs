using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;


namespace Files {
    public sealed class PropertiesManager : IDisposable {
        private const string SettingsFilePath = "settings.xml";
        private const string SettingsFolderPath = "Settings";


        private readonly string _fullPath;

        private readonly IsolatedStorageFile _isolatedStorageFile;
        private readonly XmlDocument _xmlDocument;
        private readonly XmlTextWriter _xmlTextWriter;
        private readonly IsolatedStorageFileStream _settingsFileStream;


        public PropertiesManager () {
            _fullPath = Path.Combine (SettingsFolderPath, SettingsFilePath);
            try {
                _isolatedStorageFile = IsolatedStorageFile.GetUserStoreForDomain ();
                var isEmpty = InitStorageContents ();
                _settingsFileStream = _isolatedStorageFile.OpenFile (_fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                _xmlTextWriter = new XmlTextWriter (_settingsFileStream, Encoding.UTF8) {
                    Formatting = Formatting.Indented,
                    Indentation = 2
                };
                if (isEmpty) {
                    ResetSettingsFileContents ();
                }
                _xmlDocument = new XmlDocument ();
            } catch (Exception ex) {
                MessageBox.Show (ex.Message);
            }
        }


        private bool InitStorageContents () {
            var isEmpty = false;
            if (!_isolatedStorageFile.DirectoryExists (SettingsFolderPath)) {
                _isolatedStorageFile.CreateDirectory (SettingsFolderPath);
                isEmpty = true;
            }
            if (!_isolatedStorageFile.FileExists (_fullPath)) {
                //_isolatedStorageFile.CreateFile (_fullPath);
                isEmpty = true;
            }

            return isEmpty;
        }

        private void ResetSettingsFileContents () {
            _xmlTextWriter.WriteStartDocument ();
            _xmlTextWriter.WriteStartElement ("head");
            _xmlTextWriter.WriteFullEndElement ();
            _xmlTextWriter.WriteEndDocument ();
            _xmlTextWriter.Flush ();
            _settingsFileStream.Position = 0;
        }

        public void Restore (ref DependencyObject[] dependencyObjects, int objIdx, string cfgSection, IEnumerable<string> dpPropsNames) {
            try {
                _xmlDocument.Load (_settingsFileStream);

                if (_xmlDocument.DocumentElement != null) {
                    if (_xmlDocument.HasChildNodes) {
                        foreach (XmlNode cfgSectionNode in _xmlDocument.DocumentElement) {
                            var propertyNames = dpPropsNames as IList<string> ?? dpPropsNames.ToList ();
                            if (cfgSectionNode.Name == cfgSection) {
                                foreach (XmlNode cfgSectionEntry in cfgSectionNode) {
                                    foreach (var propertyName in propertyNames) {
                                        if (cfgSectionEntry.Name == propertyName) {
                                            var value = cfgSectionEntry.FirstChild.Value;
                                            SetValue (ref dependencyObjects, objIdx, propertyName, value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                _settingsFileStream.Position = 0;
            } catch (Exception ex) {
                MessageBox.Show (ex.Message);
            }
        }

        public void Save (ref DependencyObject[] dependencyObjects, int objIdx, string cfgSectionName, IEnumerable<string> dpPropsNames) {
            try {
                if (_xmlDocument.DocumentElement != null) {
                    bool sectionExists = false;
                    //if (_xmlDocument.DocumentElement.HasChildNodes) {
                        var propertyNames = dpPropsNames as IList<string> ?? dpPropsNames.ToList ();
                        foreach (XmlNode sectionNode in _xmlDocument.DocumentElement) {
                            if (sectionNode.Name == cfgSectionName) {
                                sectionExists = true;
                                sectionNode.RemoveAll ();
                                foreach (var propertyName in propertyNames) {
                                    var value = GetValue (ref dependencyObjects, objIdx, propertyName);

                                    XmlNode newPropertyNode = _xmlDocument.CreateElement (propertyName);
                                    newPropertyNode.InnerText = value.ToString ();
                                    sectionNode.AppendChild (newPropertyNode);
                                }
                            }
                        }
                    //} else {
                        if (!sectionExists) {
                            XmlNode sectionNode = _xmlDocument.CreateElement (cfgSectionName);
                            foreach (var propertyName in propertyNames) {
                                var value = GetValue (ref dependencyObjects, objIdx, propertyName);

                                XmlNode newPropertyNode = _xmlDocument.CreateElement (propertyName);
                                newPropertyNode.InnerText = value.ToString ();
                                sectionNode.AppendChild (newPropertyNode);
                            }
                            _xmlDocument.DocumentElement.AppendChild (sectionNode);
                        }
                    //}

                    _settingsFileStream.SetLength (0);
                    _settingsFileStream.Position = 0;
                    _xmlDocument.Save (_xmlTextWriter);
                }
            } catch (Exception ex) {
                MessageBox.Show (ex.Message);
            }
        }


        public void Dispose ()
        {
            _xmlTextWriter.Close ();
            _settingsFileStream.Close ();
            _isolatedStorageFile.Close ();
        }


        private static object GetValue (ref DependencyObject[] dependencyObjects, int objectIdx, string propertyName) {
            var propertyInfo = dependencyObjects[objectIdx].GetType ().GetProperty (propertyName);
            var value = propertyInfo.GetValue (dependencyObjects[objectIdx]);
            return value;
        }

        private static void SetValue (ref DependencyObject[] dependencyObjects, int objectIdx, string propertyName, string propertyValue) {
            var propertyInfo = dependencyObjects[objectIdx].GetType ().GetProperty (propertyName);
            var returnType = propertyInfo.GetMethod.ReturnType;
            object propertyValueAsReturnType;
            if (returnType.BaseType == typeof (Enum)) {
                propertyValueAsReturnType = Enum.Parse (returnType, propertyValue, false);
            } else {
                propertyValueAsReturnType = Convert.ChangeType (propertyValue, returnType);
            }
            propertyInfo.SetValue (dependencyObjects[objectIdx], propertyValueAsReturnType);
        }
    }
}
