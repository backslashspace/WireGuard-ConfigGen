using BSS.Encryption.Fips;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ConfigGen.Views
{
    public partial class Working : UserControl
    {
        private struct ConfigPair
        {
            internal ConfigPair(String client, String server)
            {
                Client = client;
                Server = server;
            }

            internal String Client;
            internal String Server;
        }

        public Working()
        {
            InitializeComponent();

            IsVisibleChanged += StartBuild;
        }

        private async void StartBuild(object sender, DependencyPropertyChangedEventArgs e)
        {
            UInt16 numberOfClients = UInt16.Parse(Pin.MainWindow.ServerOptionsView.NumberOfClients.Text);

            BuildProgressBar.Maximum = numberOfClients;

            Thread configBuilder = new(() => ConfigBuilder(numberOfClients));
            configBuilder.Name = "Config Builder";
            configBuilder.Start();

            while (configBuilder.IsAlive)
            {
                await Task.Delay(256);
            }
            
            //done
        }

        private void ConfigBuilder(UInt16 numberOfClients)
        {
            xFips.SetApprovedOnlyMode(true);

            String outputDirectory = CreateOutputDirectory();

            ServerFileWriter = new(outputDirectory + "\\server.txt", FileMode.Create, FileAccess.Write, FileShare.Read);

            ConfigPair configPair;

            if (numberOfClients > 128) //fast mode
            {
                for (UInt16 i = 0; i != numberOfClients; ++i)
                {
                    BuildConfig(ref i, out configPair);

                    WriteConfig(ref i, ref configPair);

                    if (numberOfClients % 10 == 0)
                    {
                        Pin.Dispatcher.Invoke(() => Pin.MainWindow.WorkingView.BuildProgressBar.Value = i + 1);
                    }
                }

                Pin.Dispatcher.Invoke(() => BuildProgressBar.Value = numberOfClients);
            }
            else
            {
                for (UInt16 i = 0; i != numberOfClients; ++i)
                {
                    BuildConfig(ref i, out configPair);

                    WriteConfig(ref i, ref configPair);

                    Pin.Dispatcher.Invoke(() => Pin.MainWindow.WorkingView.BuildProgressBar.Value = i + 1);
                }
            }
        }

        private static String CreateOutputDirectory()
        {
            String directory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            directory += "\\WG-Out";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}