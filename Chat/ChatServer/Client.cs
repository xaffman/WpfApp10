using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        public string UserName { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;
        public Client(TcpClient tcpClient)
        {
            ClientSocket = tcpClient;
            UID = Guid.NewGuid();

            _packetReader = new PacketReader(ClientSocket.GetStream());
            var opCode = _packetReader.ReadByte();
            UserName = _packetReader.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}] : {UserName} подключен!");
            Task.Run(() => Process());
        }
        void Process()
        {
            while (true)
            {
                try
                {
                    var opCode = _packetReader.ReadByte();
                    switch (opCode)
                    {
                        case 5:
                            var message = _packetReader.ReadMessage();
                            Console.WriteLine($"{DateTime.Now}: Сообщение отправлено! {message}");
                            Program.BroadcastMessage($"[{DateTime.Now}] [{UserName}]: {message}");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{UID}]: Разъеденился");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}