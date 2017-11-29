using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows;
using Windows.Security.Cryptography.Core;

namespace SmartHome
{
    public delegate void RefreshSenderCollection(string newSenderId);
    internal abstract class Collections
    {
        public static ObservableCollection<Unit> CurrentNodes { get; } = new ObservableCollection<Unit>();
        public static Dictionary<string, DataSample> ActualDatas { get; } = new Dictionary<string, DataSample>();

        public static ObservableCollection<string> UnitsToLocalize { get; } = new ObservableCollection<string>();

        public static ObservableCollection<Room> Rooms { get; } = new ObservableCollection<Room>();


        //  public static event RefreshSenderCollection RefreshSenderCollectionEvent;
        public static event RefreshSenderCollection ResreshSenderDataEvent;
        public static void AddToNodeList(Unit newSender)
        {
            CurrentNodes.Add(newSender);
            // RefreshSenderCollectionEvent?.Invoke(newSenderId);
        }
        public static void AddToActualDatas(DataSample newSender)
        {
            if (Collections.ActualDatas.ContainsKey(newSender.SenderId))
            {
                RefreshActualDatas(newSender);
            }
            else
            {
                ActualDatas.Add(newSender.SenderId, newSender);
            }               
            ResreshSenderDataEvent?.Invoke(newSender.SenderId);
        }
        public static void RefreshActualDatas(DataSample newSender)
        {
            ActualDatas[newSender.SenderId] = newSender;           
        }
    }
}
