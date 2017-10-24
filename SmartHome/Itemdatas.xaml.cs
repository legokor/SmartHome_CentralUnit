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
    
    public sealed partial class ItemDatas : Page
    {
        public static readonly DependencyProperty TemperatureDependencyProperty =
            DependencyProperty.Register("Temperature", typeof(string), typeof(ItemDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty HumidityDependencyProperty =
            DependencyProperty.Register("Humidity", typeof(string), typeof(ItemDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty MovementDependencyProperty =
            DependencyProperty.Register("Movement", typeof(string), typeof(ItemDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty CoDependencyProperty =
            DependencyProperty.Register("Co", typeof(string), typeof(ItemDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty LpgDependencyProperty =
            DependencyProperty.Register("Lpg", typeof(string), typeof(ItemDatas), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SmokeDependencyProperty =
            DependencyProperty.Register("Smoke", typeof(string), typeof(ItemDatas), new PropertyMetadata(string.Empty));
        public static string ChosenUnit { get; set; }

        public string Temperature
        {
            get { return (string)GetValue(TemperatureDependencyProperty); }
            set { SetValue(TemperatureDependencyProperty, value); }
        }
        public string Humidity
        {
            get { return (string)GetValue(HumidityDependencyProperty); }
            set { SetValue(HumidityDependencyProperty, value); }
        }
        public string Movement
        {
            get { return (string)GetValue(MovementDependencyProperty); }
            set { SetValue(MovementDependencyProperty, value); }
        }
        public string Co
        {
            get { return (string)GetValue(CoDependencyProperty); }
            set { SetValue(CoDependencyProperty, value); }
        }
        public string Lpg
        {
            get { return (string)GetValue(LpgDependencyProperty); }
            set { SetValue(LpgDependencyProperty, value); }
        }
        public string Smoke
        {
            get { return (string)GetValue(SmokeDependencyProperty); }
            set { SetValue(SmokeDependencyProperty, value); }
        }



        public void RefreshTextBlocks(string newSenderId)
        {
            if (newSenderId==ChosenUnit && ViewManager.ActualDatas.ContainsKey(ChosenUnit) && ViewManager.ActualDatas[ChosenUnit] is DataElement)
            {
                var unit = ViewManager.ActualDatas[ChosenUnit] as DataElement;
                Temperature =unit.Temperature;
                Humidity = unit.Humidity;
                Co = unit.Co;
                Lpg = unit.Lpg;
                Smoke = unit.Smoke;
                TbMovement.Visibility = unit.Movement == "1" ? Visibility.Visible : Visibility.Collapsed;
            }
            
        }
        public ItemDatas()
        {
            this.InitializeComponent();
            DataContext = this;
            RefreshTextBlocks(ChosenUnit);
            ViewManager.ResreshSenderDataEvent += RefreshTextBlocks;
        }

        private void TBBack_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.ResreshSenderDataEvent -= RefreshTextBlocks;
            this.Frame.Navigate(typeof(Map), "Localizing");
        }
    }
}
