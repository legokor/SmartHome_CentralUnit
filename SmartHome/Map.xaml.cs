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
    public sealed partial class Map : Page
    {
        private bool isDelete = false;
        private List<Button> buttons = new List<Button>();
        private RoutedEventHandler function;
        public Map()
        {          
            this.InitializeComponent();          
        }

        private void InitMap()
        {
            foreach (var rooms in ViewManager.Rooms)
            {

                Button newbtn = new Button();
                buttons.Add(newbtn);
                newbtn.Height = rooms.Height;
                newbtn.Width = rooms.Width;
                Canvas.SetLeft(newbtn, rooms.CanvasLeft);
                Canvas.SetTop(newbtn, rooms.CanvasTop);
                map.Children.Add(newbtn);
                Grid.SetRow(newbtn, 1);
                Grid.SetColumn(newbtn, 1);
                newbtn.IsEnabled = true;
                newbtn.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                newbtn.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
                newbtn.Content = rooms.Name;
                newbtn.Click += function;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
            string text = e.Parameter as string;
           switch(text)
            {
                case "Selecting": function = OpenRoom; break;
                case "Localizing": function = AddUnitsToRoom; break;
                case "Switch": function = TurnLightFunction;  break;
                default: break;
            }
            InitMap();
            if(text == "Switch")
            {
                TurnLightsMode();
            }
        }
        private void OpenRoom(object sender, RoutedEventArgs e)
        {
            var room = ViewManager.Rooms.Where(s => s.Name == (sender as Button).Content).First();
            if (isDelete)
            {
                (sender as Button).Visibility = Visibility.Collapsed;                          
                room.DeleteFromList();
                foreach (var node in room.GetUnits())
                {                  
                        node.Location = "";
                        ViewManager.UnitsToLocalize.Add(node.Id); 
                }              
            }
            else
            {
            this.Frame.Navigate(typeof(SelectUnit), room);
            }
        }

        private void TurnLightsMode()
        {
            foreach(var button in buttons)
            {
                var room = ViewManager.Rooms.Where(s => s.Name == button.Content as string).FirstOrDefault();
                if (room.Auto)
                {

                    button.Background = this.Resources["AutoON"] as SolidColorBrush;
                }
                else
                {
                    if (room.Light == "Default" || room.Light == "OFF")
                    {
                        button.Background = this.Resources["LightOFF"] as SolidColorBrush;
                    }
                    else
                    {
                        button.Background = this.Resources["LightON"] as SolidColorBrush;
                    }
                }
               
            }
            IsDelete.IsEnabled = false;
        }

        private void TurnLightFunction(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            {
                var room = ViewManager.Rooms.Where(s => s.Name == button.Content as string).FirstOrDefault();

                if(room.Auto)
                {
                    room.SetAuto(false);
                    button.Background = this.Resources["LightON"] as SolidColorBrush;
                    room.TurnLightOn();
                }
                else
                {
                    if (room.Light == "Default" || room.Light == "OFF")
                    {
                        button.Background = this.Resources["LightON"] as SolidColorBrush;
                        room.TurnLightOn();
                    }
                    else
                    {
                        button.Background = this.Resources["LightOFF"] as SolidColorBrush;
                        room.TurnLightOff();
                    }
                }
            }
        }

        private void AddUnitsToRoom(object sender, RoutedEventArgs e)
        {
            var room = ViewManager.Rooms.Where(s => s.Name == (sender as Button).Content).First();
            if (isDelete)
            {
                (sender as Button).Visibility = Visibility.Collapsed;              
                room.DeleteFromList();
                foreach (var node in room.GetUnits())
                {
                    node.Location = "";
                    ViewManager.UnitsToLocalize.Add(node.Id);
                }             
            }
            else
            {
                foreach (var id in ViewManager.UnitsToLocalize)
                {
                    room.AddUnit(id);
                }
                ViewManager.UnitsToLocalize.Clear();
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RoomCreater.SaveLisToFile();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void IsDelete_Toggled(object sender, RoutedEventArgs e)
        {
            isDelete = IsDelete.IsOn;
        }
    }
}
