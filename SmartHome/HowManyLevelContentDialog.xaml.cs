using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartHome
{
    public sealed partial class HowManyLevelContentDialog : ContentDialog
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(int), typeof(HowManyLevelContentDialog), new PropertyMetadata(default(string)));

        public HowManyLevelContentDialog()
        {
            Text = Map.Stairs;
            InitializeComponent();
            if (Map.Stairs < 1) Down.IsEnabled = false;
            DataContext = this;
        }

        public int Text
        {
            get { return (int)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Map.Stairs = Text;
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            Text++;
            Map.Stairs++;
            Down.IsEnabled = true;
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            if (Text >= 1)
            {
                Text--;
                Map.Stairs--;
            }
            else
            {
                Down.IsEnabled = false;
            }
        }
    }
}
