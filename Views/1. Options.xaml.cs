using System;
using System.Windows.Controls;

namespace ConfigGen.Views
{
    public partial class Options : UserControl
    {
        public Options()
        {
            InitializeComponent();
        }

        private void ActivateUserInputField(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            String tag;
            Boolean state;

            try
            {
                tag = (String)checkBox.Tag;
                state = (Boolean)checkBox.IsChecked;
            }
            catch
            {
                return;
            }

            switch (tag)
            {
                case "PrivateKeyBox":
                    UseUserServerPrivateKeyValue.IsEnabled = state;
                    break;

                case "ServerKeepAlive":
                    ServerKeepAliveValue.IsEnabled = state;
                    break;

                case "ClientKeepAlive":
                    ClientKeepAliveValue.IsEnabled = state;
                    break;
            }
        }


        private void TextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            try
            {
                if ((String)textBox.Tag == "ignore")
                {
                    return;
                }
            }
            catch { }

            Boolean state = false;

            try
            {
                state = (Boolean)textBox.Tag;
            }
            catch { }

            if (!state)
            {
                textBox.Text = "";
                textBox.Tag = true;
            }
        }
    }
}