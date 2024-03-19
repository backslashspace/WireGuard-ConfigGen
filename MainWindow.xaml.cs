using ConfigGen.Views;
using System;
using System.Windows;
using System.Windows.Threading;

namespace ConfigGen
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Pin.MainWindow = this;
            Pin.Dispatcher = Dispatcher.CurrentDispatcher;

            Pin.CurrentPage = Page.Options;
        }

        

        private void NextPage(object sender, RoutedEventArgs e)
        {
            switch (Pin.CurrentPage)
            {
                case Page.Options:
                    OptionsView.Visibility = Visibility.Collapsed;
                    ConfirmView.Visibility = Visibility.Visible;
                    BackButton.IsEnabled = true;
                    Pin.CurrentPage = Page.Confirm;
                    break;

                case Page.Confirm:
                    ConfirmView.Visibility = Visibility.Collapsed;
                    WorkingView.Visibility = Visibility.Visible;
                    BackButton.IsEnabled = false;
                    NextButton.IsEnabled = false;
                    Pin.CurrentPage = Page.Working;
                    break;

                case Page.Final:
                    Environment.Exit(0);
                    break;
            }
        }

        private void PageBack(object sender, RoutedEventArgs e)
        {
            if (Pin.CurrentPage == Page.Confirm)
            {
                OptionsView.Visibility = Visibility.Visible;
                ConfirmView.Visibility = Visibility.Collapsed;

                BackButton.IsEnabled = false;

                Pin.CurrentPage = Page.Options;
            }
        }
    }
}