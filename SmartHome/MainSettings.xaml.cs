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
    public sealed partial class MainSettings : Page
    {
        public static bool Auto { get; set; } = true;
        public MainSettings()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
          
            Title.Text = "Main settings";
            Binding myBinding = new Binding();
            myBinding.Source = this;
            myBinding.Path = new PropertyPath("Auto");
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(IsAuto, ToggleSwitch.IsOnProperty, myBinding);
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RoomCreater.SaveLisToFile();
            this.Frame.Navigate(typeof(MainPage));
        }
    

        private void IsAuto_Changed(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            foreach(var room in ViewManager.Rooms)
            {
                room.SetAuto(toggle.IsOn);
            } 
        }

        private void ModifyPassword_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ModifyPassword));
        }

        private void Users_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SetAcces));
        }
    }
}
