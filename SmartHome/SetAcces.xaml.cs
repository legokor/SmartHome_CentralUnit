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
    public sealed partial class SetAcces : Page
    {
        private List<User> units = UserManager.GetAllUser();
        private User selectedUser;
        private AccesLevel accesLevel;
        public SetAcces()
        {
            this.InitializeComponent();
        }      
      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TBSelect.IsEnabled = false;
            DataContext = this;
            List<string> list = new List<string>();
            foreach(var unit in units)
            {
                list.Add(unit.Email);
            }
            Nodes.ItemsSource = list;
        }

        private void Open_HomePage(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null) UserManager.SetAccesLevel(selectedUser.Email, accesLevel);
            this.Frame.Navigate(typeof(MainSettings));
        }

        private void LBSenders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedUser != null) UserManager.SetAccesLevel(selectedUser.Email, accesLevel);
            selectedUser = units.Where(u => u.Email == Nodes.SelectedItem.ToString()).FirstOrDefault();
            TBSelect.Content = selectedUser.Level.ToString();
            TBSelect.IsEnabled = true;
            BtDelete.IsEnabled = true;
            accesLevel = selectedUser.Level;
        }

        private void TBSelect_Click(object sender, RoutedEventArgs e)
        {
            switch(TBSelect.Content.ToString())
            {
                case "Minimal":  TBSelect.Content = selectedUser.Level = AccesLevel.Normal; accesLevel = AccesLevel.Normal; break;
                case "Normal": TBSelect.Content = selectedUser.Level = AccesLevel.Admin; accesLevel = AccesLevel.Admin; break;
                case "Admin": TBSelect.Content = selectedUser.Level = AccesLevel.Minimal; accesLevel = AccesLevel.Minimal; break;
            }
        }

        private void Bt_Delete(object sender, RoutedEventArgs e)
        {
            DBInstance.RemoveUser(selectedUser);
            this.Frame.Navigate(typeof(SetAcces));
        }
    }
} 
