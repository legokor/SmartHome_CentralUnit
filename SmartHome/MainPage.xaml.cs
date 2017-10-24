using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartHome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static readonly DependencyProperty AvgTempDependencyProperty =
            DependencyProperty.Register("AvgTemp", typeof(string), typeof(MainPage), new PropertyMetadata(string.Empty));
        
        public string AvgTemp
        {
            get { return (string)GetValue(AvgTempDependencyProperty); }
            set { SetValue(AvgTempDependencyProperty, value); }
        }

       

        public void RefreshAvgTemp(string useless)
        {
            double temp = 0;
            int count = 0;
            foreach (var o in ViewManager.ActualDatas)
            {
                if (o.Value is DataElement)
                {
                    temp += Double.Parse((o.Value as DataElement).Temperature);
                    count++;
                }
            }
            if (count != 0)
            {
                temp = temp / count;
            }
            AvgTemp = temp.ToString()+"°C";
        }

        private readonly DispatcherTimer _clockTimer = new DispatcherTimer();



        public void StopClock()
        {
            _clockTimer.Stop();
        }

        private void InitClock()
        {
            tbDate.Text = DateTime.Now.ToString(("dddd" + (", " + "MM. dd")));
            tbClock.Text = DateTime.Now.ToString(("HH:mm:ss"));
            _clockTimer.Interval = TimeSpan.FromSeconds(1.0);
            _clockTimer.Tick += (o, e) => tbClock.Text = DateTime.Now.ToString(("HH:mm:ss"));
            _clockTimer.Tick += (o, e) => tbDate.Text = DateTime.Now.ToString(("dddd" + (", " + "MM. dd")));
            _clockTimer.Start();
        }

        private void StartServer()
        {
           // if (UdpServer.IsRunning == false)
            {
                UdpServer.IsRunning = true;
               new UdpServer().RecMessage();
            }
        }

        public MainPage()
        {
            
            this.InitializeComponent();
            DataContext = this;
            InitClock();
            if (UdpServer.IsRunning == false)
            {
                StartServer();
                RoomCreater.LoadListFromFile();
            }

            RefreshAvgTemp("init");
            ViewManager.ResreshSenderDataEvent += RefreshAvgTemp;
        }


        private async void Open_Config(object sender, RoutedEventArgs e)
        {
            if (ViewManager.Rooms.Any())
            {
                ViewManager.ResreshSenderDataEvent -= RefreshAvgTemp;
                StopClock();
                this.Frame.Navigate(typeof(Map), "Selecting");
            }
            else
            {
                ContentDialogWithOK dialog = new ContentDialogWithOK
                {
                    DataContext = new
                    {
                       MsgText="You must create atleast one room at first!"
                    }
                };
                await dialog.ShowAsync();
                this.Frame.Navigate(typeof(Localizer));
            }
        }

        private void Bt_Locate(object sender, RoutedEventArgs e)
        {
            ViewManager.ResreshSenderDataEvent -= RefreshAvgTemp;
            StopClock();
            this.Frame.Navigate((typeof(Localizer)));
        }
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.ResreshSenderDataEvent -= RefreshAvgTemp;
            StopClock();
            this.Frame.Navigate(typeof(MainSettings));
        }

        private async void Open_Control(object sender, RoutedEventArgs e)
        {
            if (ViewManager.Rooms.Any())
            {
                ViewManager.ResreshSenderDataEvent -= RefreshAvgTemp;
                StopClock();
                this.Frame.Navigate(typeof(Map), "Switch");
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
                this.Frame.Navigate(typeof(Localizer));
            }
        }
    }
}

