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
        public int Level { get; set; } = (int)UserManager.Level;
        public string AvgTemp
        {
            get { return (string)GetValue(AvgTempDependencyProperty); }
            set { SetValue(AvgTempDependencyProperty, value); }
        }

       

        public void RefreshAvgTemp(string useless)
        {
            double temp = 0.0;
            int count = 0;
            foreach (var o in Collections.ActualDatas)
            {
                if (o.Value is DataSample)
                {
                      //var number = (o.Value as DataSample).Temperature == "nan"? "0.00": (o.Value as DataSample).Temperature);
                    temp += (o.Value as DataSample).Temperature;
                    count++;
                }
            }
            if (count != 0)
            {
                temp = temp / count;
            }
            if (temp < 10)
            {
                AvgTemp = String.Format("{0:0.00}", temp) + "°C";
            }
            else
            {
                AvgTemp = String.Format("{0:00.00}", temp) + "°C";
            }
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

        

        public MainPage()
        {
            if (UdpServer.IsRunning == false)
            {
                ThemeManager.Init();
                StartServer();
                RoomCreater.LoadListFromFile();

                DataSample newel = new DataSample
                {
                    SenderId = "21000000",
                    Temperature = 35,
                    Humidity =20,
                    Movement = 1,
                    CoLevel =12,
                    LpgLevel = 0,
                    SmokeLevel = 0
                };
                


                Unit newUnit = new Unit
                {
                    Id = "21000000",
                    Type = "Switcer",
                    Auto = true
                };
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Collections.AddToActualDatas(newel);
                    Collections.UnitsToLocalize.Add(newel.SenderId);
                    Collections.AddToNodeList(newUnit);
                });
            }
            this.InitializeComponent();
            DataContext = this;
            InitClock();
            

            RefreshAvgTemp("init");
            Collections.ResreshSenderDataEvent += RefreshAvgTemp;

           
        }
        private void StartServer()
        {
            // if (UdpServer.IsRunning == false)
            {
                UdpServer.IsRunning = true;
                new UdpServer().RecMessage();
            }
        }

        private async void Open_Config(object sender, RoutedEventArgs e)
        {
            if (Collections.Rooms.Any())
            {
                Collections.ResreshSenderDataEvent -= RefreshAvgTemp;
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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
           
                Collections.ResreshSenderDataEvent -= RefreshAvgTemp;
                StopClock();
                this.Frame.Navigate(typeof(LoginScreen));            
        }

        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            UserManager.Logout();
            Collections.ResreshSenderDataEvent -= RefreshAvgTemp;
            StopClock();
            ContentDialogWithOK dialog = new ContentDialogWithOK
            {
                DataContext = new
                {
                    MsgText = "Succesful logout!"
                } 

            };
            await dialog.ShowAsync();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Bt_Locate(object sender, RoutedEventArgs e)
        {
            Collections.ResreshSenderDataEvent -= RefreshAvgTemp;
            StopClock();
            this.Frame.Navigate((typeof(Localizer)));
        }
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Collections.ResreshSenderDataEvent -= RefreshAvgTemp;
            StopClock();
            this.Frame.Navigate(typeof(MainSettings));
        }

        private void Open_SelectUnit(object sender, RoutedEventArgs e)
        {

        }

        private async void Open_Control(object sender, RoutedEventArgs e)
        {
            if (Collections.Rooms.Any())
            {
                Collections.ResreshSenderDataEvent -= RefreshAvgTemp;
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

