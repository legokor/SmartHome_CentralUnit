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
        public static int GivenId { get; set; } = 10000000;

        //Bejövő adatok szortírozása, a Process hívja meg
        private async static Task<string> SortString(string[] data)
        {
            string cmd = data[0];
            if (cmd == "SND")
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
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    AutoLightControl(newel);
                    ViewManager.AddToActualDatas(newel);
                });
                DBInstance.SaveData(newel);
            }
            if( cmd == "JND")
            {
               
                Unit newUnit = new Unit
                {
                    Id = data[1],
                    Type = data[2]
                };
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ViewManager.AddToNodeList(newUnit);
                });
            }

            if (cmd == "REQIP")
            {

                Unit newUnit = new Unit
                {
                    Type = data[1]
                };
                newUnit.Id = GivenId.ToString();
                GivenId += 10000000;
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ViewManager.AddToNodeList(newUnit);
                });
            }

            if (cmd == "SEL")
            {              
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (!ViewManager.UnitsToLocalize.Contains(data[1]))
                    {
                        ViewManager.UnitsToLocalize.Add(data[1]);
                    }
                });

            }
            return cmd;
        }

        public static void AutoLightControl(DataElement newdata)
        {
            var unit = ViewManager.CurrentNodes.Where(u => u.Id == newdata.Id).FirstOrDefault();
            if ( unit != null && unit.Type == "PIR")
            {
                Room room = ViewManager.Rooms.Where(r => r.Name == unit.Location).FirstOrDefault();
                if (room.Auto)
                {
                    if (newdata.Movement == "1")
                    {
                        room.TurnLightOn(true);
                    }
                    else
                    {
                        room.TurnLightOff(true);
                    }
                }
            }
        }


        //Üzenet elkapásakor hívódik meg, elvégzi a bejövő adatok feldolgozásást
        public async static void Process(string indata)
        {
            string[] chopped = indata.Split('|');                           //Az üzenet szédarabolása
            string cmd = await DataProcessor.SortString(chopped);                                      //Az adatok tagváltozókba szortírozása       
            if (cmd == "REQIP")                                               //Ha ip kérés van
            {
                string currmsg = ("SVD|" + DataProcessor.GivenId.ToString() + "|" + "192.168.43.182" + "|\0");         //Ip kiküldése 
                UdpServer.SendMessage(currmsg, 1080);                                                      
            }
            else
            {
                UdpServer.SendMessage("ACK|\0", 1080);
            }

        }
    }
}
