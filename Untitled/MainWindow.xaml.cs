using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Untitled.Auxilliary;


/*
     Требования к зачетной работе по программированию на C# для .NET
    +•	Допускается использование любого типа интерфейса (консольный, WPF, Windows Forms). Предпочтительным является вариант WPF.
    •	Программа должна состоять из нескольких компонентов, т.е. иметь как минимум одну сборку в виде файла dll.
    +•	В программе должно быть разработано несколько классов. На оценке работы сказывается развитость их функциональности. Рекомендуемые элементы перечисляются ниже:
    o	Наличие собственного хранилища данных (сущностей) в виде массива или коллекции (лучше типизированной);
    o	Наличие методов, свойств, индексаторов для работы с этим хранилищем;
    +o	Использование интерфейсов и/или абстрактных классов при проектировании иерархии классов;
    o	Использование механизма исключений для работы с нештатными ситуациями;
    +o	Предпочтительно использовать свойства (возможно, автоматические), а не поля для хранения данных;
    o	Переопределение операций.
    •	Использование конструкций LINQ для работы с данными.
    •	Работа с файлами, использование стандартных диалогов для их открытия.
    •	Работа любым типом базы данных: SQL-сервер, файл SQL, XML и т.п.       
*/

namespace Untitled {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {
        public MainWindow () {
            InitializeComponent ();
            Bootstrap ();
        }

        public void Bootstrap () {
            pane_1_LayoutViewRoot.AddColumnForFSNode (new SystemRoot (), null);
        }
    }
}
