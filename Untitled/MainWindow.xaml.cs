using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Controls.Layouts;
using Controls.UserControls;
using FileOperationInterop;
using Files;
using FSOps;
using Shell32Interop;


/*
     Требования к зачетной работе по программированию на C# для .NET
    +•	Допускается использование любого типа интерфейса (консольный, WPF, Windows Forms). Предпочтительным является вариант WPF.
    +•	Программа должна состоять из нескольких компонентов, т.е. иметь как минимум одну сборку в виде файла dll.
    +•	В программе должно быть разработано несколько классов. На оценке работы сказывается развитость их функциональности. Рекомендуемые элементы перечисляются ниже:
    +o	Наличие собственного хранилища данных (сущностей) в виде массива или коллекции (лучше типизированной);
    +o	Наличие методов, свойств, индексаторов для работы с этим хранилищем;
    +o	Использование интерфейсов и/или абстрактных классов при проектировании иерархии классов;
    +o	Использование механизма исключений для работы с нештатными ситуациями;
    +o	Предпочтительно использовать свойства (возможно, автоматические), а не поля для хранения данных;
    +o	Переопределение операций.
    +•	Использование конструкций LINQ для работы с данными.
    +-•	Работа с файлами, использование стандартных диалогов для их открытия.
    •	Работа любым типом базы данных: SQL-сервер, файл SQL, XML и т.п.       
*/


// TODO: to clpbrd -> for each item in selection

namespace FilesApplication {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        #region props
        public MainWindowModel ViewModel { get; private set; }

        public ClipboardWindow ClipboardWindow { get; private set; }
        #endregion


        #region ctors
        public MainWindow () {
            InitializeComponent ();

            ViewModel = new MainWindowModel ();
            DataContext = ViewModel;

            ClipboardWindow = new ClipboardWindow ();
        }

        // TODO: for future use
        public MainWindow (IEnumerable<FSNode> startupFSNodes) : this () {
            foreach (var node in startupFSNodes) {
                ViewModel.Layouts.Add (new MillerColumnsLayout (node));    
            }                                                          
        }
        #endregion


        #region events
        private void mainWindow_OnClosing (object sender, System.ComponentModel.CancelEventArgs e) {
            ClipboardWindow.Close ();
        }

        private static void ShellExecuteExOnFSNode (ExecutedRoutedEventArgs e, string lpVerb) {
            var fsNodeView = e.Parameter as FSNodeView;
            if (fsNodeView != null) {
                var fsNode = FSOps.FSOps.TryGetConcreteFSNode<FileLikeFSNode> (fsNodeView.ViewModel.FSNode);
                if (fsNode != null) {
                    Shell32.ShellExecuteEx (fsNode.FullPath, lpVerb);
                }
            }
        }

        private void UseFSNodeFor (ExecutedRoutedEventArgs e, Func<FSNode, bool> f) {
            var fsNodeView = e.Parameter as FSNodeView;
            if (fsNodeView != null) {
                var fsNode = FSOps.FSOps.TryGetConcreteFSNode<FileLikeFSNode> (fsNodeView.ViewModel.FSNode);
                if (fsNode != null) {
                    f (fsNode);
                }
            }
        }

        private void AddFSNodeToClipboard (ExecutedRoutedEventArgs e, ActionTag actionTag) {
            var fsNodeView = e.Parameter as FSNodeView;
            if (fsNodeView != null) {
                var fsNode = FSOps.FSOps.TryGetConcreteFSNode<FileLikeFSNode> (fsNodeView.ViewModel.FSNode);
                if (fsNode != null) {
                    ClipboardWindow.ViewModel.ClipboardStack += new ClipboardEntry<FileLikeFSNode> (fsNode, actionTag);
                }
            }
        }

        private static void CheckEventArgsType (CanExecuteRoutedEventArgs e, TypeTag tagToCheck) {
            var fsNodeView = e.Parameter as FSNodeView;
            if (fsNodeView != null) {
                var fsNode = FSOps.FSOps.TryGetConcreteFSNode<FileLikeFSNode> (fsNodeView.ViewModel.FSNode);
                if (fsNode != null) {
                    e.CanExecute = fsNode.Is (tagToCheck);
                }
            }
        }


        private void propertiesCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            ShellExecuteExOnFSNode (e, "properties");
        }

        private void propertiesCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.SubRoot | TypeTag.Internal | TypeTag.Leaf);
        }


        private void openCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            ShellExecuteExOnFSNode (e, "open");
        }

        private void openCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.SubRoot | TypeTag.Internal | TypeTag.Leaf);
        }


        private void cutCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            AddFSNodeToClipboard (e, ActionTag.Cut);
        }

        private void cutCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.Internal | TypeTag.Leaf);
        }


        private void copyCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            AddFSNodeToClipboard (e, ActionTag.Copy);
        }

        private void copyCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.Internal | TypeTag.Leaf);
        }


        private void pasteCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            UseFSNodeFor (
                e,
                (fsNode) => {
                    if (ClipboardWindow.ViewModel.ClipboardStack.Count > 0) {
                        using (var fileOperation = new FileOperation (new FileOperationProgressSink ())) {
                            foreach (var clipboardEntry in ClipboardWindow.ViewModel.ClipboardStack) {
                                if (clipboardEntry.Item2 == ActionTag.Copy) {
                                    fileOperation.CopyItem (
                                        clipboardEntry.Item1.FullPath,
                                        fsNode.FullPath,
                                        clipboardEntry.Item1.Name
                                    );
                                } else {
                                    if (clipboardEntry.Item2 == ActionTag.Cut) {
                                        fileOperation.MoveItem (
                                            clipboardEntry.Item1.FullPath,
                                            fsNode.FullPath,
                                            clipboardEntry.Item1.Name
                                        );
                                    }
                                }
                            }
                            fileOperation.PerformOperations ();
                            if (!fileOperation.GetAnyOperationsAborted ()) {
                                ClipboardWindow.ViewModel.ClipboardStack.Clear ();
                            }
                            return true;
                        }
                    }
                    return false;
                }
            );
        }

        private void pasteCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.SubRoot | TypeTag.Internal);
        }


        private void deleteCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            UseFSNodeFor (
                e,
                (fsNode) => {
                    using (var fileOperation = new FileOperation (new FileOperationProgressSink ())) {
                        fileOperation.DeleteItem (fsNode.FullPath);
                        var result = MessageBox.Show (
                            "Are you sure to delete " + fsNode + "?",
                            "Delete",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question
                        );
                        if (result == MessageBoxResult.Yes) {
                            fileOperation.PerformOperations ();
                            return true;
                        }
                        return false;
                    }
                }
            );
        }

        private void deleteCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.Internal | TypeTag.Leaf);
        }


        private void newFileCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            UseFSNodeFor (
                e,
                (fsNode) => {
                    var renameWnd = new TextInputDialog ("new file", "New file") { Owner = this };
                    renameWnd.ShowDialog ();
                    if (renameWnd.DialogResult == true) {
                        using (var fileOperation = new FileOperation (new FileOperationProgressSink ())) {
                            fileOperation.NewItem (fsNode.FullPath, renameWnd.inputTextBox.Text, FileAttributes.Normal);
                            fileOperation.PerformOperations ();
                            return true;
                        }
                    }
                    return false;
                }
            );
        }

        private void newFileCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.SubRoot | TypeTag.Internal);
        }


        private void newDirectoryCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            UseFSNodeFor (
                e, 
                (fsNode) => {
                    var renameWnd = new TextInputDialog ("new directory", "New directory") { Owner = this };
                    renameWnd.ShowDialog ();
                    if (renameWnd.DialogResult == true) {
                        using (var fileOperation = new FileOperation (new FileOperationProgressSink ())) {
                            fileOperation.NewItem (fsNode.FullPath, renameWnd.inputTextBox.Text, FileAttributes.Directory);
                            fileOperation.PerformOperations ();
                            return true;
                        }
                    }
                    return false;
                }
            );
        }

        private void newDirectoryCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.SubRoot | TypeTag.Internal);
        }


        private void renameCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            UseFSNodeFor (
                e, 
                (fsNode) => {
                    var renameWnd = new TextInputDialog (fsNode.Name, "Rename") { Owner = this };
                    renameWnd.ShowDialog ();
                    if (renameWnd.DialogResult == true) {
                        using (var fileOperation = new FileOperation (new FileOperationProgressSink ())) {
                            fileOperation.RenameItem (fsNode.FullPath, renameWnd.inputTextBox.Text);
                            fileOperation.PerformOperations ();
                            return true;
                        }
                    }
                    return false;
                }
            );
        }

        private void renameCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            CheckEventArgsType (e, TypeTag.Internal | TypeTag.Leaf);
        }


        //private void backCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
        //    throw new System.NotImplementedException ();
        //}

        //private void backCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
        //    throw new System.NotImplementedException ();
        //}


        //private void forwardCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
        //    throw new System.NotImplementedException ();
        //}

        //private void forwardCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
        //    throw new System.NotImplementedException ();
        //}


        //private void homeCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
        //    throw new System.NotImplementedException ();
        //}

        //private void homeCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
        //    throw new System.NotImplementedException ();
        //}


        //private void upCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
        //    throw new System.NotImplementedException ();
        //}

        //private void upCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
        //    throw new System.NotImplementedException ();
        //}
        #endregion
    }
}
