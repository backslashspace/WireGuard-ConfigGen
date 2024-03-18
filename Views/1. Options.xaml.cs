using System;
using System.Windows;
using System.Windows.Controls;

namespace ConfigGen.Views
{
    public partial class Options : UserControl
    {
        public Options()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Test.Content = $"{TheGrid.ActualHeight}, {TheGrid.ActualWidth}";
        }
    }
}