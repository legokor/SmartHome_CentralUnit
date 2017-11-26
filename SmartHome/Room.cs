using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    internal class Room
    {
        public string Name { get; set; }
        public bool Auto { get; set; } = true;

        public int OnLevel { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public double CanvasLeft { get; set; }
        public double CanvasTop { get; set; }

        public string Light { get; set; } = "Default";

        public void GivePosition(double width, double height, double canvasLeft, double canvasTop)
        {
            Width = width;
            Height = height;
            CanvasLeft = canvasLeft;
            CanvasTop = canvasTop;
        }
        public async Task  SaveRoomInToFile(Windows.Storage.CreationCollisionOption openType = Windows.Storage.CreationCollisionOption.OpenIfExists)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile roomFile = await storageFolder.CreateFileAsync("Rooms.txt", openType);
            await Windows.Storage.FileIO.AppendTextAsync(roomFile, Name + "|" + CanvasLeft.ToString() + "|" + CanvasTop.ToString() + "|" + Height.ToString() + "|" + Width.ToString() + "|" + Auto.ToString() +"|"+  OnLevel.ToString() +"|\n");                    
            return;
        }

        public Room(string name)
        {
            Name = name;
        }

        public Room(double width, double height, double canvasLeft, double canvasTop, string name, bool auto, int onLevel)
        {
            Name = name;
            Width = width;
            Height = height;
            CanvasLeft = canvasLeft;
            CanvasTop = canvasTop;
            Auto = auto;
            OnLevel = onLevel;
        }

        public IEnumerable<Unit> GetUnits()
        {
            return ViewManager.CurrentNodes.Where((s => s.Location == Name));
        }

        public IEnumerable<DataElement> GetDataElements()
        {
            var units = ViewManager.CurrentNodes.Where((s => s.Location == Name));
            List<DataElement> list = new List<DataElement>();
            foreach (var unit in units)
            {
                if(ViewManager.ActualDatas.ContainsKey(unit.Id))
                {
                    list.Add(ViewManager.ActualDatas[unit.Id]);
                }                
            }
            return list;
               

        }

        public IEnumerable<Unit> GetPIRs()
        {
            return ViewManager.CurrentNodes.Where((s => s.Location == Name && s.Type == "PIR"));
        }
        public IEnumerable<Unit> GetSwitchers()
        {
            return ViewManager.CurrentNodes.Where((s => s.Location == Name && s.Type == "Switcher"));
        }

        public void AddUnit(string id)
        {
            var unit = ViewManager.CurrentNodes.Where(s => s.Id == id);
            if (unit.Any())
            {
                unit.First().Location = Name;
            }
        }

        public void RemoveUnit(string id)
        {
            var units = GetUnits();
            var unit = units.Where(s => s.Id == id);
            if (unit.Any())
            {
                unit.First().Location = "";
            }
        }

        public void SetAuto(bool auto)
        {
            SetAutoToAllUnits(auto);
                Auto = auto;
        }

        private void SetAutoToAllUnits(bool auto)
        {
            var units = GetUnits();
            foreach (var unit in units)
            {
                unit.Auto = auto;
            }
        }

        public static Room GetRoomByName(string name)
        {
            return ViewManager.Rooms.Where(s => s.Name == name).First();
        }

        public void DeleteFromList()
        {
            ViewManager.Rooms.Remove(this);
        }
        public void AddToList()
        {
            ViewManager.Rooms.Add(this);
        }

        public static void DeleteByRoom(Room room)
        {
            ViewManager.Rooms.Where(s => s.Name == room.Name).First().DeleteFromList();
        }

        public static void AddByRoom(Room room)
        {
            ViewManager.Rooms.Where(s => s.Name == room.Name).First().AddToList();
        }

        public void Rename(string newName)
        {
            var units = GetUnits();
            foreach (var unit in units)
            {
                unit.Location = newName;
            }
            Name = newName;
        }

        public void TurnLightOn(bool autoSwitch)
        {
            foreach (var unit in GetUnits().Where(u => u.Type == "Switcher" && u.Auto == autoSwitch))
            {
                string message = "CMD|" + unit.Id + "|1|\0";
                UdpServer.SendMessage(message, 1080);                
            }
            this.Light = "ON";
        }
        public void TurnLightOff(bool autoSwitch)
        {
            foreach (var unit in GetUnits().Where(u => u.Type == "Switcher" && u.Auto == autoSwitch))
            {
                string message = "CMD|" + unit.Id + "|0|\0";
                UdpServer.SendMessage(message, 1080);               
            }
            this.Light = "OFF";
        }
    }
}
