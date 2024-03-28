using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigGen.Views
{
    public partial class ServerOptions : UserControl
    {
        private struct TagInfo
        {
            internal TagInfo(Object rawTag)
            {
                String[] tagParts = rawTag.ToString().Split(':');

                Message = (TagMessages)Byte.Parse(tagParts[0]);
                IsValid = Boolean.Parse(tagParts[1]);
            }

            internal TagMessages Message;
            internal Boolean IsValid;
        }

        private enum TagMessages : Byte
        {
            PrivateKey = 0,
            NumberOfClients = 1,
            ServerListenPort = 2,
            ServerInterface = 3,
        }

        public ServerOptions()
        {
            InitializeComponent();
        }

        private void UsePrivateKey_CheckToggled(object sender, System.Windows.RoutedEventArgs e)
        {
            UseUserServerPrivateKeyValue.IsEnabled = (Boolean)UseUserServerPrivateKey.IsChecked;

            TagInfo tagInfo = new(UseUserServerPrivateKeyValue.Tag);

            if ((Boolean)UseUserServerPrivateKey.IsChecked && !tagInfo.IsValid)
            {
                UseUserServerPrivateKeyValue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#747474"));
                return;
            }
    
            UseUserServerPrivateKeyValue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
        }

        private void TextBoxFocusChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Tag == null) { return; }
            TagInfo tagInfo = new(textBox.Tag);

            if (tagInfo.IsValid)
            {
                return;
            }

            if (textBox.IsKeyboardFocused)
            {
                textBox.Text = "";
                return;
            }

            switch (tagInfo.Message)
            {
                case TagMessages.PrivateKey:
                    textBox.Text = "<privateKey>";
                    break;
                case TagMessages.NumberOfClients:
                    textBox.Text = "<numberOfClients>";
                    break;

                case TagMessages.ServerListenPort:
                    textBox.Text = "<port>";
                    break;

                case TagMessages.ServerInterface:
                    textBox.Text = "<ip/mask>";
                    break;
            }

            textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#747474"));
        }

        private void TextChanged_ValidateNew(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Tag == null) { return; }
            TagInfo tagInfo = new(textBox.Tag);
            
            textBox.Text = textBox.Text.Trim();

            switch (tagInfo.Message)
            {
                case TagMessages.PrivateKey:
                    tagInfo.IsValid = textBox.Text.Length == 44;
                    break;
                case TagMessages.NumberOfClients:
                    tagInfo.IsValid = UInt16.TryParse(textBox.Text, out _);
                    break;

                case TagMessages.ServerListenPort:
                    tagInfo.IsValid = UInt16.TryParse(textBox.Text, out _);
                    break;

                case TagMessages.ServerInterface:
                    tagInfo.IsValid = ValidateCIDR(textBox.Text);
                    break;
            }

            if (tagInfo.IsValid)
            {
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
            else
            {
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ae3127"));
            }

            textBox.Tag = $"{(Byte)tagInfo.Message}:{tagInfo.IsValid}";
        }

        private static Boolean ValidateCIDR(String text)
        {
            String[] parts = text.Split('/');

            if (parts.Length != 2)
            {
                return false;
            }

            if (!IPAddress.TryParse(parts[0], out _))
            {
                return false;
            }

            if (Byte.TryParse(parts[1], out Byte mask))
            {
                if (mask > 32)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}