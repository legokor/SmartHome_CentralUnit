using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class SelectUnit : Page
    {
        private Room chosenPlace;
        private string roomName;

        public SelectUnit()
        {
            this.InitializeComponent();           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var room = e.Parameter as Room;
            if (room != null)
            {
                chosenPlace = room;
                roomName = room.Name;
            }
            TBSelect.IsEnabled = false;
            DataContext = this;
            var units = chosenPlace.GetUnits();
            var ids = new List<string>();
            foreach (var unit in units)
            {
                ids.Add(unit.Id);
            }
            Nodes.ItemsSource = ids;
        }


        private void Open_HomePage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void LBSenders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TBSelect.IsEnabled = true;
            BtDelete.IsEnabled = true;
        }

        private void TBSelect_Click(object sender, RoutedEventArgs e)
        {
            ItemDatas.ChosenUnit = Nodes.SelectedItem.ToString();
            this.Frame.Navigate(typeof(ItemDatas));
        }
       
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RoomSettings), chosenPlace);
        }

        private void Bt_Delete(object sender, RoutedEventArgs e)
        {
            ViewManager.UnitsToLocalize.Add(Nodes.SelectedItem as string);
            chosenPlace.RemoveUnit(Nodes.SelectedItem as string);
            this.Frame.Navigate(typeof(SelectUnit), chosenPlace);
        }
    }
}
