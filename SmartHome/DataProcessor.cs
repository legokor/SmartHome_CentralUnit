using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;

namespace SmartHome
{
    internal abstract class DataProcessor
    {
        public static List<Unit> Units = new List<Unit>();
        public static int GivenId { get; set; } = 10000000;

        public static ComboBox ComboBoxPtr { get; set; }

        public static string Cmd { get; set; }

        public static string CurrentSelected { get; set; } = "10000000";

        public static bool ReceivePause { get; set; } = false;

        //Bejövő adatok szortírozása, a Process hívja meg
        public async static void SortString(string[] data)
        {
            Cmd = data[0];
            if (Cmd == "SND")
            {
                DataElement newel = new DataElement
                {
                    Id = data[1],
                    Temperature = data[6],
                    Humidity = data[5],
                    Movement = data[7],
                    Co = data[3],
                    Lpg = data[2],
                    Smoke = data[4]
                };
                DataProcessor.LoadList(newel);             //Beöltés a a dataelement listába
            }
            if( Cmd == "JND")
            {
               
                Unit newUnit = new Unit
                {
                    Id = data[1],
                    Type = data[2]
                };
                Units.Add(newUnit);
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ViewManager.AddToNodeList(newUnit);
                });
            }

            if (Cmd == "REQIP")
            {

                Unit newUnit = new Unit
                {
                    Type = data[1]
                };
                newUnit.Id = GivenId.ToString();
                GivenId += 10000000;
                Units.Add(newUnit);
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ViewManager.AddToNodeList(newUnit);
                });
            }

            if (Cmd == "BTN")
            {              
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (!ViewManager.UnitsToLocalize.Contains(data[1]))
                    {
                        ViewManager.UnitsToLocalize.Add(data[1]);
                    }
                });

            }

        }

        //Listába felvenni az új elemet
        public static async void LoadList(DataElement newdata)
        {
           // DBInstance.SaveData(newdata);
          //  var dbList = DBInstance.LoadData(newdata.Id);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                AutoLightControl(newdata);
                ViewManager.AddToActualDatas(newdata);
            });

        }

        public static void AutoLightControl(DataElement newdata)
        {
            var unit = ViewManager.CurrentNodes.Where(u => u.Id == newdata.Id).FirstOrDefault();
            if (unit.Type == "PIR")
            {
                Room room = ViewManager.Rooms.Where(r => r.Name == unit.Location).FirstOrDefault();
                if (room.Auto)
                {
                    if (newdata.Movement == "1")
                    {
                        room.TurnLightOn();
                    }
                    else
                    {
                        room.TurnLightOff();
                    }
                }
            }
        }


        //Üzenet elkapásakor hívódik meg, elvégzi a bejövő adatok feldolgozásást
        public static void Process(string indata)
        {
            string[] chopped = indata.Split('|');                           //Az üzenet szédarabolása
            DataProcessor.SortString(chopped);                                      //Az adatok tagváltozókba szortírozása       
            if (Cmd == "REQIP")                                               //Ha ip kérés van
            {
                string currmsg = ("SVD|" + DataProcessor.GivenId.ToString() + "|" + "192.168.43.182" + "|\0");         //Ip kiküldése 
                UdpServer.SendMessage(currmsg, 1080);                                                      
            }
            else
            {
                UdpServer.SendMessage("ACK\0", 1080);
            }

        }
    }
}
