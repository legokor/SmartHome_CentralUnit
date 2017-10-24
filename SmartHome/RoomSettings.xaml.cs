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
    public sealed partial class RoomSettings : Page
    {
        Room room = null;
        public RoomSettings()
        {         
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var text = e.Parameter as Room;
            if (text != null)
            {
                room = text;
                Title.Text = text.Name + "'s settings";
            }
            Binding myBinding = new Binding();
            myBinding.Source = room;
            myBinding.Path = new PropertyPath("Auto");
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(IsAuto, ToggleSwitch.IsOnProperty, myBinding);           
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RoomCreater.SaveLisToFile();
            this.Frame.Navigate(typeof(SelectUnit), room);
        }

        private async void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Localizer.saidYes = false;
            var locationDialog = new ContentDialogWithText();
            await locationDialog.ShowAsync();
            if (Localizer.saidYes == true)
            {
                
               room.Rename(RoomCreater.ActualRoomName);
               Localizer.saidYes = false;              
               Frame.Navigate(typeof(RoomSettings), room);
            }
        }

        private void IsAuto_Changed(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            room.SetAuto(toggle.IsOn);
        }
    }
}
