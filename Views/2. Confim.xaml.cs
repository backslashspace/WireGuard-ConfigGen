using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;
using System.Threading;
using System.Runtime.InteropServices;

namespace ConfigGen.Views
{
    public partial class Confirm : UserControl
    {
        public Confirm()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Boolean toolsPresent = false;

            await Task.Run(() =>
            {
                toolsPresent = File.Exists("C:\\Program Files\\WireGuard\\wg.exe");
            }).ConfigureAwait(true);

            if (toolsPresent)
            {
                WGToolsStatusIcon.Source = new BitmapImage(new Uri(@"\icons\imageres_157.ico", UriKind.Relative));
                WGToolsStatusIcon.Margin = new Thickness(-2, 3, 0, 0);
                WGToolsStatusIcon.Height = 34;
                WGToolsStatusIcon.Width = 34;

                WireGuardToolPath.Text = "C:\\Program Files\\WireGuard\\wg.exe";
                WireGuardToolPath.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#747474"));
                ToolSeletorButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#747474"));
            }
        }



        

        private void SelectToolExe(object sender, RoutedEventArgs e)
        {
            Byte[] privateKey = new Byte[32];
            Byte[] publicKey = new Byte[32];

            WGTool.WGGenKeypair(publicKey, privateKey);

            Console.WriteLine();

            WGTool.WGCalcPublicKey(publicKey, privateKey);


            Console.WriteLine();




        }





    }
}