using System.Windows;
using System.Windows.Controls;
using Controls.Layouts;
using FSOps;


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
        public MainWindowModel Model { get; private set; }

        public MainWindow () {
            InitializeComponent ();

            Model = new MainWindowModel ();
            DataContext = Model;
        }

        // TODO: for future use
        public MainWindow (FSNode startupFSNode) : this () {
            Model.Layouts.Add (new MillerColumnsLayout (startupFSNode));
        }

    }
}
