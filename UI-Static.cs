using System.Windows.Threading;

namespace ConfigGen
{
    internal struct Pin
    {
        internal static MainWindow MainWindow;

        internal static Dispatcher Dispatcher;

        internal static Page CurrentPage;
    }

    internal enum Page
    {
        Options = 0,
        Confirm = 1,
        Working = 2,
        Final = 3,
    }
}