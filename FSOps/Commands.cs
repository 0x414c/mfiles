using System.Windows.Input;


namespace FSOps {
    public static class Commands {
        //public static readonly RoutedUICommand BrowseUpCommand = 
        //    new RoutedUICommand (
        //        "Navigate to one level up", "Up",
        //        typeof (Commands),
        //        new InputGestureCollection { new KeyGesture (Key.Back, ModifierKeys.Control) }
        //    );

        public static readonly RoutedUICommand RenameCommand =
            new RoutedUICommand (
                "Rename selected item", "Rename",
                typeof (Commands),
                new InputGestureCollection { new KeyGesture (Key.Enter, ModifierKeys.Alt) }
            );

        public static readonly RoutedUICommand NewDirectoryCommand =
            new RoutedUICommand (
                "Make new directory", "New directory",
                typeof (Commands),
                new InputGestureCollection { new KeyGesture (Key.N, ModifierKeys.Shift | ModifierKeys.Control) }
            );
    }
}
