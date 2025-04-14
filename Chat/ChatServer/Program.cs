using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Net.IO;

namespace ChatServer
{
    class Program
    {
        static List<Client> _users;
        static TcpListener _listener;

        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7892);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                BroadcastConnection(); // Уведомляем всех о новом пользователе
            }
        }

        public static void BroadcastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var u in _users)
                {
                    var packet = new PacketBuilder();
                    packet.WriteOpCode(1);
                    packet.WriteMessage(u.UserName);
                    packet.WriteMessage(u.UID.ToString());
                    user.ClientSocket.Client.Send(packet.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(5);
                messagePacket.WriteMessage(message);
                user.ClientSocket.Client.Send(messagePacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string UID)
        {
            var disconnectedUser = _users.FirstOrDefault(d => d.UID.ToString() == UID);
            if (disconnectedUser != null)
            {
                _users.Remove(disconnectedUser);
                foreach (var user in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(10);
                    broadcastPacket.WriteMessage(UID);
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
                BroadcastMessage($"{disconnectedUser.UserName} disconnected...");
            }
        }
    }
}
