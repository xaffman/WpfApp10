using Chat.MVVM.Core;
using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Net
{
    class Server
    {
        TcpClient _tcpClient;

        public PacketReader PacketReader { get; set; }

        public event Action connectedEvent;
        public event Action messageReceivedEvent;
        public event Action userDisconnectEvent;
        public Server()
        {
            _tcpClient = new TcpClient();
        }
        public void ConnectToServer(string userName)
        {
            if (!_tcpClient.Connected)
            {
                _tcpClient.Connect("127.0.0.1", 7892);
                PacketReader = new PacketReader(_tcpClient.GetStream());

                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteMessage(userName);
                _tcpClient.Client.Send(connectPacket.GetPacketBytes());

                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opCode = PacketReader.ReadByte();
                    switch (opCode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            messageReceivedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("...");
                            break;
                    }
                }
            });
        }
        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _tcpClient.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}