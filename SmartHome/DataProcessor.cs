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
                DataSample newel = new DataSample
                {
                    SenderId = data[1],
                    Temperature = double.Parse(data[6]),
                    Humidity = double.Parse(data[5]),
                    Movement = int.Parse(data[7]),
                    CoLevel = double.Parse(data[3]),
                    LpgLevel = double.Parse(data[2]),
                    SmokeLevel = double.Parse(data[4])
                };
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    AutoLightControl(newel);
                    Collections.AddToActualDatas(newel);
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
                    Collections.AddToNodeList(newUnit);
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
                    Collections.AddToNodeList(newUnit);
                });
            }

            if (cmd == "SEL")
            {              
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (!Collections.UnitsToLocalize.Contains(data[1]))
                    {
                        Collections.UnitsToLocalize.Add(data[1]);
                    }
                });

            }
            return cmd;
        }

        public static void AutoLightControl(DataSample newdata)
        {
            var unit = Collections.CurrentNodes.Where(u => u.Id == newdata.SenderId).FirstOrDefault();
            if ( unit != null && unit.Type == "PIR")
            {
                Room room = Collections.Rooms.Where(r => r.Name == unit.Location).FirstOrDefault();
                if (room.Auto)
                {
                    if (newdata.Movement == 1)
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
