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
        public int Level { get; set; } = (int)UserManager.Level;
        private bool isDelete = false;
        private List<Button> buttons = new List<Button>();
        private RoutedEventHandler function;
        static public int ActualStair { get; set; } = 1;
        static public int Stairs { get; set; } = 0;
        private string mode;
        public Map()
        {
            DataContext = this;

            this.InitializeComponent();          
        }

        private void InitMap()
        {
            foreach (var rooms in Collections.Rooms)
            {
                if (rooms.OnLevel == ActualStair)
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
                    newbtn.FontSize = 10;
                }
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
             mode = e.Parameter as string;
           switch(mode)
            {
                case "Selecting": function = OpenRoom; Text.Visibility = Visibility.Visible; Text.Text = "Your rooms"; break;
                case "Localizing": function = AddUnitsToRoom; Text.Visibility = Visibility.Visible; Text.Text = "Add unit"; break;
                case "Switch": function = TurnLightFunction;  break;
                default: break;
            }
            InitMap();
            if(mode == "Switch")
            {
                TurnLightsMode();
            }
        }
        private void OpenRoom(object sender, RoutedEventArgs e)
        {
            var room = Collections.Rooms.Where(s => s.Name == (sender as Button).Content).First();
            if (isDelete)
            {
                (sender as Button).Visibility = Visibility.Collapsed;                          
                room.DeleteFromList();
                foreach (var node in room.GetUnits())
                {                  
                        node.Location = "";
                        Collections.UnitsToLocalize.Add(node.Id); 
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
                var room = Collections.Rooms.Where(s => s.Name == button.Content as string).FirstOrDefault();
                if (room.Auto)
                {

                    button.Background = Application.Current.Resources["AutoON"] as SolidColorBrush;
                }
                else
                {
                    if (room.Light == "Default" || room.Light == "OFF")
                    {
                        button.Background = Application.Current.Resources["LightOFF"] as SolidColorBrush;
                    }
                    else
                    {
                        button.Background = Application.Current.Resources["LightON"] as SolidColorBrush;
                    }
                }
               
            }
            Text.Visibility = Visibility.Visible;
            ColorInfo.Visibility = Visibility.Visible;
            IsDelete.IsEnabled = false;
        }

        private void TurnLightFunction(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            {
                var room = Collections.Rooms.Where(s => s.Name == button.Content as string).FirstOrDefault();

                if(room.Auto)
                {
                    room.SetAuto(false);
                    button.Background = Application.Current.Resources["LightON"] as SolidColorBrush;
                    room.TurnLightOn(false);
                }
                else
                {
                    if (room.Light == "Default" || room.Light == "OFF")
                    {
                        button.Background = Application.Current.Resources["AutoON"] as SolidColorBrush;
                        room.SetAuto(true);
                    }
                    else
                    {
                        button.Background = Application.Current.Resources["LightOFF"] as SolidColorBrush;
                        room.TurnLightOff(false);
                    }
                }
            }
        }

        private void AddUnitsToRoom(object sender, RoutedEventArgs e)
        {
            var room = Collections.Rooms.Where(s => s.Name == (sender as Button).Content).First();
            if (isDelete)
            {
                (sender as Button).Visibility = Visibility.Collapsed;              
                room.DeleteFromList();
                foreach (var node in room.GetUnits())
                {
                    node.Location = "";
                    Collections.UnitsToLocalize.Add(node.Id);
                }             
            }
            else
            {
                foreach (var id in Collections.UnitsToLocalize)
                {
                    room.AddUnit(id);
                }
                Collections.UnitsToLocalize.Clear();
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

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            if (ActualStair >= Stairs) return;
            ActualStair++;
            this.Frame.Navigate(typeof(Map), mode);
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            if (ActualStair <= 1) return;
            ActualStair--;
            this.Frame.Navigate(typeof(Map), mode);
        }
    }
}
