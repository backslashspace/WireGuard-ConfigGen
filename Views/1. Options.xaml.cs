using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigGen.Views
{
    public partial class Options : UserControl
    {
        public Options()
        {
            InitializeComponent();
        }

        private void ClearUserInputField(object sender, System.Windows.RoutedEventArgs e)
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

            switch (tag.Split(':')[0])
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
            String[] tagParts;

            try
            {
                tagParts = ((String)textBox.Tag).Split(':');

                if (tagParts[0] == "NumberOfClients")
                {
                    return;
                }
            }
            catch
            {
                return;
            }

            Boolean state;

            try
            {
                state = Boolean.Parse(tagParts[1]);
            }
            catch 
            {
                return;
            }

            if (!state)
            {
                textBox.Text = "";
                textBox.Tag = $"{tagParts[0]}:true";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            String tag;

            try
            {
                tag = (String)textBox.Tag;

                textBox.Text.Trim();

                if (tag == null || textBox.Text == "")
                {
                    return;
                }
            }
            catch
            {
                return;
            }

            Boolean valid = tag.Split(':')[0] switch
            {
                "WireGuardKey" => textBox.Text.Length == 44,
                "KeepAliveTime" => UInt16.TryParse(textBox.Text, out _),
                "NumberOfClients" => UInt32.TryParse(textBox.Text, out _),
                _ => false
            };

            if (valid)
            {
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
            else
            {
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ae3127"));
            }
        }
    }
}