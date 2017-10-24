using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Windows.UI.Xaml.Controls;
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Storage.Streams;
using Windows.UI.Core;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using Windows.Networking.Connectivity;

namespace SmartHome
{
    class UdpServer
    {
        private readonly static SemaphoreSlim semaphore = new SemaphoreSlim(1);
        public static bool IsRunning { get; set; } = false;
        private HostName myIpAddress;
        public async void RecMessage()
        {
            foreach (HostName localHostName in NetworkInformation.GetHostNames()) //Get the LocalIP address
            {
                if (localHostName.IPInformation != null)
                {
                    if (localHostName.Type == HostNameType.Ipv4)
                    {
                       myIpAddress = localHostName;
                        break;
                    }
                }
            }
           
            try
            {
                await Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        await semaphore.WaitAsync();
                        DatagramSocket socket;
                        {
                            socket = new DatagramSocket();
                            socket.MessageReceived += MessageReceived;
                            var alma = socket.BindServiceNameAsync("1080");
                            semaphore.Release();
                        }
                    }
                });
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async void MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            await semaphore.WaitAsync();
            try
            {
                if (args.RemoteAddress.CanonicalName != myIpAddress.CanonicalName)
                {
                    IInputStream inputstream = args.GetDataStream();
                    Stream stream = inputstream.AsStreamForRead(1024);

                    using (StreamReader streamreader = new StreamReader(stream))
                    {
                        var recmsg = await streamreader.ReadToEndAsync();
                        DataProcessor.Process(recmsg.ToString());
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async static void SendMessage(string message, int port)
        {
            await semaphore.WaitAsync();
            try
            {
                DatagramSocket socket = new DatagramSocket();
                using (IOutputStream stream =
                    await socket.GetOutputStreamAsync(new HostName("255.255.255.255"), port.ToString()))
                {
                    using (DataWriter writer = new DataWriter(stream))
                    {
                        byte[] data = Encoding.ASCII.GetBytes(message);
                        writer.WriteBytes(data);
                        await writer.StoreAsync();
                    }

                }
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
