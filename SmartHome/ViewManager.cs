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
    internal abstract class ViewManager
    {
        public static ObservableCollection<Unit> CurrentNodes { get; } = new ObservableCollection<Unit>();
        public static Dictionary<string, DataElement> ActualDatas { get; } = new Dictionary<string, DataElement>();

        public static ObservableCollection<string> UnitsToLocalize { get; } = new ObservableCollection<string>();

        public static ObservableCollection<Room> Rooms { get; } = new ObservableCollection<Room>();


        //  public static event RefreshSenderCollection RefreshSenderCollectionEvent;
        public static event RefreshSenderCollection ResreshSenderDataEvent;
        public static void AddToNodeList(Unit newSender)
        {
            CurrentNodes.Add(newSender);
            // RefreshSenderCollectionEvent?.Invoke(newSenderId);
        }
        public static void AddToActualDatas(DataElement newSender)
        {
            if (ViewManager.ActualDatas.ContainsKey(newSender.Id))
            {
                RefreshActualDatas(newSender);
            }
            else
            {
                ActualDatas.Add(newSender.Id, newSender);
            }               
            ResreshSenderDataEvent?.Invoke(newSender.Id);
        }
        public static void RefreshActualDatas(DataElement newSender)
        {
            ActualDatas[newSender.Id] = newSender;           
        }
    }
}
