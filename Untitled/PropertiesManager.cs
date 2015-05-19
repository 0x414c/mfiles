using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Xml;


namespace Files {
    public sealed class PropertiesManager : IDisposable {
        private const string settingsFilePath = "settings.xml";
        private const string settingsFolderPath = "Settings";

        private readonly string _fullPath;

        private IsolatedStorageFile _isolatedStorageFile;
        private XmlDocument _xmlDocument;
        private XmlTextWriter _xmlTextWriter;


        public PropertiesManager () {
            _fullPath = Path.Combine (settingsFolderPath, settingsFilePath);
            try {
                _isolatedStorageFile = IsolatedStorageFile.GetUserStoreForDomain ();
                InitStorage ();
            } catch (Exception ex) {
                MessageBox.Show (ex.Message);
            }
            _xmlDocument = new XmlDocument ();
        }

        public void Dispose () {
            _xmlTextWriter.Close ();
            _isolatedStorageFile.Close ();
        }

        public void InitStorage () {
            if (!_isolatedStorageFile.DirectoryExists (settingsFolderPath)) {
                _isolatedStorageFile.CreateDirectory (settingsFolderPath);
            }
            if (!_isolatedStorageFile.FileExists (_fullPath)) {
                //_isolatedStorageFile.CreateFile (_fullPath);
                ResetSettingsFile ();
            }
        }

        public void ResetSettingsFile () {
            using (_xmlTextWriter = new XmlTextWriter (
                _isolatedStorageFile.OpenFile (_fullPath, FileMode.OpenOrCreate, FileAccess.Write),
                Encoding.UTF8
                )
            ) 
            {
                _xmlTextWriter.WriteStartDocument ();
                _xmlTextWriter.WriteStartElement ("head");
                _xmlTextWriter.WriteFullEndElement ();
                _xmlTextWriter.WriteEndDocument ();
                _xmlTextWriter.Close ();
            }
        }

        public void Restore (ref DependencyObject[] dependencyObjects, int objIdx, string cfgSection, IEnumerable<string> dpPropsNames) {
            try {
                using (var file = _isolatedStorageFile.OpenFile (_fullPath, FileMode.Open, FileAccess.Read)) {
                    _xmlDocument.Load (file);

                    if (_xmlDocument.DocumentElement != null) {
                        foreach (XmlNode cfgSectionNode in _xmlDocument.DocumentElement) {
                            if (cfgSectionNode.Name == cfgSection) {
                                foreach (XmlNode cfgSectionEntry in cfgSectionNode) {
                                    foreach (string propertyName in dpPropsNames) {
                                        if (cfgSectionEntry.Name == propertyName) {
                                            var value = cfgSectionEntry.FirstChild.Value;
                                            var property = dependencyObjects[objIdx].GetType ().GetProperty (propertyName);
                                            var valueType = property.GetMethod.ReturnType;
                                            var valueAsValueType = Convert.ChangeType (value, valueType);
                                            property.SetValue (dependencyObjects[objIdx], valueAsValueType);

                                            //var property = ReflectionUtils.GetDependencyPropertyByName (dependencyObject, propertyName);
                                            //dependencyObject.SetValue (property, value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) { }
        }

        public void Save (ref DependencyObject[] dependencyObjects, int objIdx, string cfgSectionName, IEnumerable<string> dpPropsNames) {
            try {
                if (_xmlDocument.DocumentElement != null) {
                    foreach (XmlNode section in _xmlDocument.DocumentElement) {
                        if (section.Name == cfgSectionName) {
                            section.RemoveAll ();
                            foreach (string propertyName in dpPropsNames) {
                                var property = dependencyObjects[objIdx].GetType ().GetProperty (propertyName);
                                var value = property.GetValue (dependencyObjects[objIdx]);

                                XmlNode newPropertyNode = _xmlDocument.CreateElement (propertyName);
                                newPropertyNode.InnerText = value.ToString ();
                                section.AppendChild (newPropertyNode);
                            }
                        }
                    }
                    using (_xmlTextWriter = new XmlTextWriter (
                        _isolatedStorageFile.OpenFile (_fullPath, FileMode.Create, FileAccess.Write),
                        Encoding.UTF8
                        )
                    ) 
                    {
                        _xmlDocument.Save (_xmlTextWriter);
                    }
                }
            } catch (Exception ex) { }
        }
    }
}
