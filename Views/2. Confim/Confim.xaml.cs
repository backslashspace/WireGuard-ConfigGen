using System.Windows;
using System;
using System.Windows.Controls;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace ConfigGen.Views
{
    public partial class Confirm : UserControl
    {
        private enum DependencyState
        {
            Valid = 0,
            WasInvalid = 1,
            Missing = 2,
            WasMissing = 3,
        }

        private struct Dependencies
        {
            public Dependencies()
            {
                Curve_DLL = DependencyState.Missing;
                Enc_DLL = DependencyState.Missing;
                BC_FIPS_DLL = DependencyState.Missing;
            }

            internal DependencyState Curve_DLL;
            internal DependencyState Enc_DLL;
            internal DependencyState BC_FIPS_DLL;
        }

        //

        public Confirm()
        {
            InitializeComponent();

            IsVisibleChanged += UpdateView;
        }

        private void UpdateView(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as UserControl).Visibility != Visibility.Visible)
            {
                return;
            }

            Dependencies dependencies = ValidateDependencies();

            SetUI(ref dependencies);
        }

        //

        private Dependencies ValidateDependencies()
        {
            String directory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Dependencies dependencies = new();

            try
            {
                if (File.Exists("curve.dll"))
                {
                    if (xHash.CompareHash("curve.dll", xHash.KnownHashes.Curve_DLL))
                    {
                        dependencies.Curve_DLL = DependencyState.Valid;
                        goto SKIPCURVE;
                    }

                    dependencies.Curve_DLL = DependencyState.WasInvalid;
                }

                xExtractor.SaveResourceToDisk($"{directory}\\curve.dll", "ConfigGen.libs.curve.dll", Assembly.GetExecutingAssembly());
                dependencies.Curve_DLL = DependencyState.WasMissing;
            
            SKIPCURVE:

                if (File.Exists("BSS.Encryption.dll"))
                {
                    if (xHash.CompareHash("BSS.Encryption.dll", xHash.KnownHashes.Enc_DLL))
                    {
                        dependencies.Enc_DLL = DependencyState.Valid;
                        goto SKIPENC;
                    }

                    dependencies.Enc_DLL = DependencyState.WasInvalid;
                }

                xExtractor.SaveResourceToDisk($"{directory}\\BSS.Encryption.dll", "ConfigGen.libs.BSS.Encryption.dll", Assembly.GetExecutingAssembly());
                dependencies.Enc_DLL = DependencyState.WasMissing;
            
            SKIPENC:

                if (File.Exists("bc-fips-1.0.2.dll"))
                {
                    if (xHash.CompareHash("bc-fips-1.0.2.dll", xHash.KnownHashes.BC_FIPS_DLL))
                    {
                        dependencies.BC_FIPS_DLL = DependencyState.Valid;
                        goto BCDLL;
                    }

                    dependencies.BC_FIPS_DLL = DependencyState.WasInvalid;
                }

                xExtractor.SaveResourceToDisk($"{directory}\\bc-fips-1.0.2.dll", "ConfigGen.libs.bc-fips-1.0.2.dll", Assembly.GetExecutingAssembly());
                dependencies.BC_FIPS_DLL = DependencyState.WasMissing;

            BCDLL:

                WGToolsStatusIcon.Source = new BitmapImage(new Uri(@"\icons\imageres_157.ico", UriKind.Relative));
                WGToolsStatusIcon.Margin = new Thickness(WGToolsStatusIcon.Margin.Left - 18, WGToolsStatusIcon.Margin.Top - 18, 0, 0);
                WGToolsStatusIcon.Height = 34;
                WGToolsStatusIcon.Width = 34;
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;

                Pin.MainWindow.NextButton.IsEnabled = false;
            }

            return dependencies;
        }

        private void SetUI(ref Dependencies dependencies)
        {
            if (dependencies.Curve_DLL == DependencyState.Missing || dependencies.Enc_DLL == DependencyState.Missing || dependencies.BC_FIPS_DLL == DependencyState.Missing)
            {
                DependenciesValue.Text = "error";
            }
            else if (dependencies.Curve_DLL == DependencyState.WasInvalid || dependencies.Enc_DLL == DependencyState.WasInvalid || dependencies.BC_FIPS_DLL == DependencyState.WasInvalid)
            {
                DependenciesValue.Text = "Re-Extracted files";
            }
            else if (dependencies.Curve_DLL == DependencyState.WasMissing || dependencies.Enc_DLL == DependencyState.WasMissing || dependencies.BC_FIPS_DLL == DependencyState.WasMissing)
            {
                DependenciesValue.Text = "Extracted files";
            }
            else
            {
                DependenciesValue.Text = "Present files were valid";
            }

            //

            if ((Boolean)Pin.MainWindow.ServerOptionsView.UseUserServerPrivateKey.IsChecked)
            {
                Mode.Text = "append";
            }
            else
            {
                Mode.Text = "new";
            }

            //

            if ((Boolean)Pin.MainWindow.ServerOptionsView.UsePresharedKeys.IsChecked)
            {
                PSK.Text = "yes";
            }
            else
            {
                PSK.Text = "no";
            }

            //

            //if ((Boolean)Pin.MainWindow.OptionsView.ServerKeepAlive.IsChecked)
            //{
            //    ServerKeepAlive.Text = Pin.MainWindow.OptionsView.ServerKeepAliveValue.Text + "ms";
            //}
            //else
            //{
            //    ServerKeepAlive.Text = "no";
            //}

            //

            //if ((Boolean)Pin.MainWindow.OptionsView.ClientKeepAlive.IsChecked)
            //{
            //    ClientKeepAlive.Text = Pin.MainWindow.OptionsView.ClientKeepAliveValue.Text + "ms";
            //}
            //else
            //{
            //    ClientKeepAlive.Text = "no";
            //}

            //

            NumOfCLients.Text = Pin.MainWindow.ServerOptionsView.NumberOfClients.Text;
        }
    }
}