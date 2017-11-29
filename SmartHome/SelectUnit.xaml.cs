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

        public static readonly DependencyProperty TempDependencyProperty =
           DependencyProperty.Register("Temp", typeof(double), typeof(SelectUnit), new PropertyMetadata(string.Empty));
        private Room chosenPlace;
        public string RoomName { get; set; }

        public double Temp
        {
            get { return (double)GetValue(TempDependencyProperty); }
            set { SetValue(TempDependencyProperty, value); }
        }

        public SelectUnit()
        {
            DataContext = this;
            this.InitializeComponent();           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var room = e.Parameter as Room;
            if (room != null)
            {
                chosenPlace = room;
                RoomName = room.Name;
            }
            TBSelect.IsEnabled = false;          
            var units = chosenPlace.GetUnits();
            var ids = new List<string>();
            foreach (var unit in units)
            {
                ids.Add(unit.Id);
            }
            Temp = 0;
            Nodes.ItemsSource = ids;
            Collections.ResreshSenderDataEvent += RefreshTemp;
            RefreshTemp("init");
        }

        public void RefreshTemp(string newSenderId)
        {
            int i = 0;
            foreach(var element in chosenPlace.GetDataSamples())
            {
                i++;
                Temp += element.Temperature;
            }
            if (i != 0) Temp = Temp / i;
            if (Temp < 10)
            {
                TemperatureBox.Text = String.Format("{0:0.00}", Temp) + "°C";
            }
            else
            {
                TemperatureBox.Text = String.Format("{0:00.00}", Temp) + "°C";
            }
        }


        private void Open_HomePage(object sender, RoutedEventArgs e)
        {
            Collections.ResreshSenderDataEvent -= RefreshTemp;
            this.Frame.Navigate(typeof(MainPage));
        }

        private void LBSenders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TBSelect.IsEnabled = true;
            BtDelete.IsEnabled = true;
        }

        private void TBSelect_Click(object sender, RoutedEventArgs e)
        {
            Collections.ResreshSenderDataEvent -= RefreshTemp;
            UnitDatas.ChosenUnit = Nodes.SelectedItem.ToString();
            this.Frame.Navigate(typeof(UnitDatas));
        }
       
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Collections.ResreshSenderDataEvent -= RefreshTemp;
            this.Frame.Navigate(typeof(RoomSettings), chosenPlace);
        }

        private void Bt_Delete(object sender, RoutedEventArgs e)
        {
            Collections.ResreshSenderDataEvent -= RefreshTemp;
            Collections.UnitsToLocalize.Add(Nodes.SelectedItem.ToString());
            chosenPlace.RemoveUnit(Nodes.SelectedItem.ToString());
            this.Frame.Navigate(typeof(SelectUnit), chosenPlace);
        }
    }
}
