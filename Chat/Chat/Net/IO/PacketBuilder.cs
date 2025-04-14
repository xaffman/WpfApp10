using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _memoryStream;

        public PacketBuilder()
        {
            _memoryStream = new MemoryStream();
        }

        public void WriteOpCode(byte opCode)
        {
            _memoryStream.WriteByte(opCode);
        }

        public void WriteMessage(string message)
        {
            var messageLength = message.Length;
            var lengthBytes = BitConverter.GetBytes(messageLength);
            _memoryStream.Write(lengthBytes, 0, lengthBytes.Length);

            var messageBytes = Encoding.UTF8.GetBytes(message);
            _memoryStream.Write(messageBytes, 0, messageBytes.Length);
        }

        public byte[] GetPacketBytes()
        {
            return _memoryStream.ToArray();
        }
    }
}

        
    

