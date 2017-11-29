﻿using System;
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
        public int Level { get; set; } = (int)UserManager.Level;
        public string theme { get; set; } = ThemeManager.CurrentTheme;

        public int Stairs
        {
            get { return (int)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        } 
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
           "Stairs", typeof(int), typeof(MainSettings), new PropertyMetadata(default(string)));

        public MainSettings()
        {
            DataContext = this;
            this.InitializeComponent();
            ThemeSelector.ItemsSource = ThemeManager.Themes.Keys;
            ThemeSelector.SelectedValue = ThemeManager.CurrentTheme;
            Stairs = Map.Stairs;
            if (Map.Stairs < 1) Down.IsEnabled = false;
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

        private void IsThemeChanged_Changed(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
                ThemeManager.SetTheme(combobox.SelectedValue.ToString());          
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            Stairs++;
            Map.Stairs++;
            Down.IsEnabled = true;
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            if (Stairs >= 1)
            {
                Stairs--;
                Map.Stairs--;
            }
            else
            {
                Down.IsEnabled = false;
            }
        }
    }
}
