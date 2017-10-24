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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartHome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LineChart : Page
    {

        private readonly List<Spot> spots = new List<Spot>();
        public LineChart()
        {
            this.InitializeComponent();
            GetChartData();
            DataContext = this;
        }

        internal List<Spot> Spots => spots;

        private void GetChartData()
        {
            Spots.Add(new Spot()
            {
                Temperature = 20,
                Time = "2015.05.06"

            });
          
           // (Line1.Series[0] as BubbleSeries).ItemsSource = Spots;
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate((typeof(MainPage)));
        }
    }


    class Spot
    {
        public double Temperature { get; set; }
        public string Time { get; set; }
    }


}
