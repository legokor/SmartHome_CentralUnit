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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartHome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Localizer : Page
    {
        public static bool saidYes { set; get; } = false;
        public Localizer()
        {
            this.InitializeComponent();
            Nodes.ItemsSource = Collections.UnitsToLocalize;
        }
        private void Bt_Cancel(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void LBSenders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtDelete.IsEnabled = true;
        }

        private void Bt_Delete(object sender, RoutedEventArgs e)
        {          
            Collections.UnitsToLocalize.Remove(Nodes.SelectedItem.ToString()); 
        }

        private async void Bt_AddNew(object sender, RoutedEventArgs e)
        {
            var locationDialog = new ContentDialogWithText();
            await locationDialog.ShowAsync();
            if(saidYes==true)
            {
                saidYes = false;
                Frame.Navigate(typeof(RoomCreater));
            }        

        }

        private async void Bt_AddExisting(object sender, RoutedEventArgs e)
        {
            if (Collections.Rooms.Any())
            {
                Frame.Navigate(typeof(Map), "Localizing");
            }
            else
            {
                ContentDialogWithOK dialog = new ContentDialogWithOK
                {
                    DataContext = new
                    {
                        MsgText = "You must create atleast one room at first!"
                    }
                };
                await dialog.ShowAsync();
            }
        }

    }
}
