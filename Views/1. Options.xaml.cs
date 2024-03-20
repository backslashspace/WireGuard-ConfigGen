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


        private void CheckToggled(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            String tag = checkBox.Tag as String;

            if (tag == null) { return; }

            switch (tag)
            {
                case "PrivateKeyBox":
                    PrivateKeyBox();
                    break;

                case "ServerKeepAlive":
                    ServerKeepAlive();
                    break;

                case "ClientKeepAlive":
                    ClientKeepAlive();
                    break;
            }

            //

            void PrivateKeyBox()
            {
                UseUserServerPrivateKeyValue.IsEnabled = !UseUserServerPrivateKeyValue.IsEnabled;

                SetFrontColor(ref UseUserServerPrivateKeyValue);
            }

            void ServerKeepAlive()
            {
                ServerKeepAliveValue.IsEnabled = !ServerKeepAliveValue.IsEnabled;

                SetFrontColor(ref ServerKeepAliveValue);
            }

            void ClientKeepAlive()
            {
                ClientKeepAliveValue.IsEnabled = !ClientKeepAliveValue.IsEnabled;

                SetFrontColor(ref ClientKeepAliveValue);
            }

            //

            void SetFrontColor(ref TextBox textBox)
            {
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
        }

        private void TextChanged_ValidateNew(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            String[] tagParts;
            try { tagParts = textBox.Tag.ToString().Split(':'); } catch { return; }

            textBox.Text = textBox.Text.Trim();

            Boolean isValid = tagParts[0] switch
            {
                "WireGuardKey" => textBox.Text.Length == 44,
                "KeepAliveTime" => UInt16.TryParse(textBox.Text, out _),
                "NumberOfClients" => UInt32.TryParse(textBox.Text, out _),
                _ => true,
            };

            if (isValid)
            {
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
            else
            {
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ae3127"));
            }

            textBox.Tag = $"{tagParts[0]}:{isValid}";
        }

        private void TextBoxFocusChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            String[] tagParts;
            try { tagParts = textBox.Tag.ToString().Split(':'); } catch { return; }

            if (textBox.IsKeyboardFocused && !Boolean.Parse(tagParts[1]))
            {
                textBox.Text = "";
            }
            else if (!textBox.IsKeyboardFocused && !Boolean.Parse(tagParts[1]))
            {
                textBox.Text = "<time in ms>";

                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#747474"));
            }
        }
    }
}