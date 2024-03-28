using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
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

        //## ## ## ## ## ## ## ## ## ## ## ## ## ## ## ## ## ## ##

        private void NextPage(object sender, RoutedEventArgs e)
        {
            switch (Pin.CurrentPage)
            {
                case Page.Options:
                    if (!VerifyOptions()) { return; }
                    OptionsView.Visibility = Visibility.Collapsed;
                    ConfirmView.Visibility = Visibility.Visible;
                    BackButton.IsEnabled = true;
                    Pin.CurrentPage = Page.Confirm;
                    NextButton.Content = "Build";
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

                NextButton.Content = "Next";

                Pin.CurrentPage = Page.Options;
            }
        }

        //

        private Boolean VerifyOptions()
        {
            Boolean valid = true;

            if ((Boolean)OptionsView.UseUserServerPrivateKey.IsChecked)
            {
                if (OptionsView.UseUserServerPrivateKeyValue.Text.Length != 44)
                {
                    valid = false;

                    goto SKIP;
                }
            }

            if ((Boolean)OptionsView.ServerKeepAlive.IsChecked)
            {
                if (!UInt16.TryParse(OptionsView.ServerKeepAliveValue.Text, out _))
                {
                    valid = false;

                    goto SKIP;
                }
            }

            if ((Boolean)OptionsView.ClientKeepAlive.IsChecked)
            {
                if (!UInt16.TryParse(OptionsView.ClientKeepAliveValue.Text, out _))
                {
                    valid = false;

                    goto SKIP;
                }
            }

            if (!UInt32.TryParse(OptionsView.NumberOfClients.Text, out _))
            {
                valid = false;
            }

        SKIP:;

            if (!valid)
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show(
                "At least one field was invalid,\nplease make sure the input has the correct format.",
                "WG-Gen",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }
    }
}