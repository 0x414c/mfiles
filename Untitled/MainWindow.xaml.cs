using System.Windows;
using System.Windows.Input;
using Controls.Layouts;
using Controls.UserControls;
using FSOps;
using Shell32Interop;


/*
     Требования к зачетной работе по программированию на C# для .NET
    +•	Допускается использование любого типа интерфейса (консольный, WPF, Windows Forms). Предпочтительным является вариант WPF.
    +•	Программа должна состоять из нескольких компонентов, т.е. иметь как минимум одну сборку в виде файла dll.
    +•	В программе должно быть разработано несколько классов. На оценке работы сказывается развитость их функциональности. Рекомендуемые элементы перечисляются ниже:
    o	Наличие собственного хранилища данных (сущностей) в виде массива или коллекции (лучше типизированной);
    o	Наличие методов, свойств, индексаторов для работы с этим хранилищем;
    +o	Использование интерфейсов и/или абстрактных классов при проектировании иерархии классов;
    +o	Использование механизма исключений для работы с нештатными ситуациями;
    +o	Предпочтительно использовать свойства (возможно, автоматические), а не поля для хранения данных;
    o	Переопределение операций.
    +•	Использование конструкций LINQ для работы с данными.
    +-•	Работа с файлами, использование стандартных диалогов для их открытия.
    •	Работа любым типом базы данных: SQL-сервер, файл SQL, XML и т.п.       
*/


namespace FilesApplication {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindowModel ViewModel { get; private set; }

        public MainWindow () {
            InitializeComponent ();

            ViewModel = new MainWindowModel ();
            DataContext = ViewModel;
        }

        // TODO: for future use
        public MainWindow (FSNode startupFSNode) : this () {
            ViewModel.Layouts.Add (new MillerColumnsLayout (startupFSNode));
        }

        private static void ShellExecuteExOnFSNode (ExecutedRoutedEventArgs e, string lpVerb) {
            var fsNodeView = e.Parameter as FSNodeView;
            if (fsNodeView != null) {
                var fsNode = FileManagement.TryGetConcreteFSNode<FileLikeFSNode> (fsNodeView.ViewModel.FSNode);
                if (fsNode != null) {
                    Shell.ShellExecuteEx (fsNode.FullPath, lpVerb);
                }
            }
        }

        private void propertiesCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            ShellExecuteExOnFSNode (e, "properties");
        }

        private void propertiesCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void openCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            ShellExecuteExOnFSNode (e, "open");
        }

        private void openCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void cutCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            throw new System.NotImplementedException ();
        }

        private void cutCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            var fsNodeView = e.Parameter as FSNodeView;
            if (fsNodeView != null) {
                var fsNode = FileManagement.TryGetConcreteFSNode<FileLikeFSNode> (fsNodeView.ViewModel.FSNode); 
                if (fsNode != null) {
                    e.CanExecute = fsNode.Is (TypeTag.Internal | TypeTag.Leaf);
                }
            }
        }

        private void copyCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            throw new System.NotImplementedException ();
        }

        private void copyCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void pasteCommandBinding_OnExecuted (object sender, ExecutedRoutedEventArgs e) {
            throw new System.NotImplementedException ();
        }

        private void pasteCommandBinding_OnCanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
    }
}
