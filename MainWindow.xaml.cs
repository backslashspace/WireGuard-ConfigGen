using System.Windows;

namespace ConfigGen
{
    public partial class MainWindow : Window
    {
        private static Page CurrentPage = Page.Options;

        public MainWindow()
        {
            InitializeComponent();
        }

        private enum Page
        {
            Options = 0,
            Working = 1,
            Final = 2,
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {

        }

        private void PageBack(object sender, RoutedEventArgs e)
        {

        }
    }
}