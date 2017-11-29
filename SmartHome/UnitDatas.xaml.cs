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
    /// 
    
    public sealed partial class UnitDatas : Page
    {
        public static readonly DependencyProperty TemperatureDependencyProperty =
            DependencyProperty.Register("Temperature", typeof(double), typeof(UnitDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty HumidityDependencyProperty =
            DependencyProperty.Register("Humidity", typeof(double), typeof(UnitDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty MovementDependencyProperty =
            DependencyProperty.Register("Movement", typeof(int), typeof(UnitDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty CoDependencyProperty =
            DependencyProperty.Register("Co", typeof(double), typeof(UnitDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty LpgDependencyProperty =
            DependencyProperty.Register("Lpg", typeof(double), typeof(UnitDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SmokeDependencyProperty =
            DependencyProperty.Register("Smoke", typeof(double), typeof(UnitDatas), new PropertyMetadata(string.Empty));
        public static string ChosenUnit { get; set; }

        public double Temperature
        {
            get { return (double)GetValue(TemperatureDependencyProperty); }
            set { SetValue(TemperatureDependencyProperty, value); }
        }
        public double Humidity
        {
            get { return (double)GetValue(HumidityDependencyProperty); }
            set { SetValue(HumidityDependencyProperty, value); }
        }
        public int Movement
        {
            get { return (int)GetValue(MovementDependencyProperty); }
            set { SetValue(MovementDependencyProperty, value); }
        }
        public double Co
        {
            get { return (double)GetValue(CoDependencyProperty); }
            set { SetValue(CoDependencyProperty, value); }
        }
        public double Lpg
        {
            get { return (double)GetValue(LpgDependencyProperty); }
            set { SetValue(LpgDependencyProperty, value); }
        }
        public double Smoke
        {
            get { return (double)GetValue(SmokeDependencyProperty); }
            set { SetValue(SmokeDependencyProperty, value); }
        }



        public void RefreshTextBlocks(string newSenderId)
        {
            if (newSenderId==ChosenUnit && Collections.ActualDatas.ContainsKey(ChosenUnit) && Collections.ActualDatas[ChosenUnit] is DataSample)
            {
                var unit = Collections.ActualDatas[ChosenUnit] as DataSample;
                Temperature =unit.Temperature;
                Humidity = unit.Humidity;
                Co = unit.CoLevel;
                Lpg = unit.LpgLevel;
                Smoke = unit.SmokeLevel;
                TbMovement.Visibility = unit.Movement == 1 ? Visibility.Visible : Visibility.Collapsed;
            }
            
        }
        public UnitDatas()
        {
            this.InitializeComponent();
            DataContext = this;
            RefreshTextBlocks(ChosenUnit);
            Collections.ResreshSenderDataEvent += RefreshTextBlocks;
        }

        private void TBBack_Click(object sender, RoutedEventArgs e)
        {
            Collections.ResreshSenderDataEvent -= RefreshTextBlocks;
            this.Frame.Navigate(typeof(Map), "Localizing");
        }
    }
}
