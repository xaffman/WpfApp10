using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.MVVM.Core
{
    internal class PacketReader
    {
        private readonly BinaryReader _reader;

        public PacketReader(NetworkStream networkStream)
        {
            _reader = new BinaryReader(networkStream, Encoding.UTF8);
        }

        internal byte ReadByte()
        {
            return _reader.ReadByte();
        }

        internal string ReadMessage()
        {
            var length = _reader.ReadInt32();
            var messageBytes = _reader.ReadBytes(length);
            return Encoding.UTF8.GetString(messageBytes);
        }
    }
}
