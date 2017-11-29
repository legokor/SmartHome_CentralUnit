using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Input;
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
    /// 
    

    public sealed partial class RoomCreater : Page
    {
        public static string ActualRoomName;
        private Room CreatedRoom;
        Button btn;
        public RoomCreater()
        {
            if (Map.Stairs == 0) InitStairs();
            CreatedRoom = new Room(ActualRoomName);
            this.InitializeComponent();
            btn = new Button();
            btn.Background = new SolidColorBrush(Windows.UI.Colors.Red);
            btn.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
            btn.BorderThickness = new Thickness(3);
            btn.Margin = new Thickness(0);
            map.Children.Add(btn);
            Grid.SetRow(btn, 1);
            Grid.SetColumn(btn, 1);
            btn.Height = 20;
            btn.Width = 20;
            map.PointerMoved += Map_PointerMoved;
            foreach (var rooms in Collections.Rooms)
            {
                if (rooms.OnLevel == Map.ActualStair)
                {
                    Button newbtn = new Button();
                    newbtn.Height = rooms.Height;
                    newbtn.Width = rooms.Width;
                    Canvas.SetLeft(newbtn, rooms.CanvasLeft);
                    Canvas.SetTop(newbtn, rooms.CanvasTop);
                    map.Children.Add(newbtn);
                    Grid.SetRow(newbtn, 1);
                    Grid.SetColumn(newbtn, 1);
                    newbtn.IsEnabled = false;
                    newbtn.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    newbtn.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
                }
            }
        }
        private async void InitStairs()
        {
            HowManyLevelContentDialog dialog = new HowManyLevelContentDialog();
            await dialog.ShowAsync();
        }

        public static async void LoadListFromFile()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile roomFile = await storageFolder.CreateFileAsync("Rooms.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            string text;
            Collections.Rooms.Clear();
            var lines = await Windows.Storage.FileIO.ReadLinesAsync(roomFile);
            for (int i =0; i<lines.Count() ;i++)
            {
                text = lines[i];
                string[] chopped = text.Split('|');
                if(chopped.Count() >= 6)
                Collections.Rooms.Add(new Room(double.Parse(chopped[4]), double.Parse(chopped[3]), double.Parse(chopped[1]), double.Parse(chopped[2]), chopped[0], bool.Parse(chopped[5]), int.Parse(chopped[6])));
            }

             
            
        }

        public static async void SaveLisToFile()
        {
            bool isFirst = true;
            foreach (var room in Collections.Rooms)
            {
                await room.SaveRoomInToFile(isFirst ? Windows.Storage.CreationCollisionOption.ReplaceExisting : Windows.Storage.CreationCollisionOption.OpenIfExists);
                isFirst = false;
            }
        }

        private void Map_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(map).Position.Y > 10 && e.GetCurrentPoint(map).Position.X > 10 && e.GetCurrentPoint(map).Position.X < (map.ActualWidth-btn.ActualWidth) && e.GetCurrentPoint(map).Position.Y < (map.ActualHeight-btn.ActualHeight)) 
            {
                Canvas.SetTop(btn, e.GetCurrentPoint(map).Position.Y - 30);
                Canvas.SetLeft(btn, e.GetCurrentPoint(map).Position.X - 30);
            }
        }

        private void map_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            //Canvas.SetTop(btn, e.GetPosition(map).Y-btn.Height/2);
            //Canvas.SetLeft(btn, e.GetPosition(map).X-btn.Width/2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Localizer));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            CreatedRoom.GivePosition(btn.Width, btn.Height, Canvas.GetLeft(btn), Canvas.GetTop(btn));
            CreatedRoom.OnLevel = Map.ActualStair;
            Collections.Rooms.Add(CreatedRoom);
            CreatedRoom.SaveRoomInToFile();
            foreach (var id in Collections.UnitsToLocalize)
            {
                CreatedRoom.AddUnit(id);
            }
            Collections.UnitsToLocalize.Clear();
            this.Frame.Navigate(typeof(MainPage));
        }
        

        private async void IncHeight_Holding(object sender, HoldingRoutedEventArgs e)
        {
            while (IncHeight.IsPressed)
            {
                btn.Height += 3;
                await Task.Delay(100);
            }

        }

        private async void DecHeight_Holding(object sender, HoldingRoutedEventArgs e)
        {
            while (DecHeight.IsPressed && btn.Height>15)
            {
                btn.Height -= 3;
                await Task.Delay(100);
            }

        }

        private async void DecWidth_Holding(object sender, HoldingRoutedEventArgs e)
        {
            while (DecWidth.IsPressed && btn.Width>15)
            {
                btn.Width -= 3;
                await Task.Delay(100);
            }

        }

        private async void IncWidth_Holding(object sender, HoldingRoutedEventArgs e)
        {
            while (IncWidth.IsPressed)
            {
                btn.Width += 3;
                await Task.Delay(100);
            }

        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            if (Map.ActualStair >= Map.Stairs) return;
            Map.ActualStair++;
            this.Frame.Navigate(typeof(RoomCreater));
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            if (Map.ActualStair <= 1) return;
            Map.ActualStair--;
            this.Frame.Navigate(typeof(RoomCreater));
        }

    }
}
