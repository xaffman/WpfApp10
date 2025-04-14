using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.MVVM.Core
{
    class PacketReader
    {
        private NetworkStream networkStream;

        public PacketReader(NetworkStream networkStream)
        {
            this.networkStream = networkStream;
        }

        internal int ReadByte()
        {
            throw new NotImplementedException();
        }

        internal string ReadMessage()
        {
            throw new NotImplementedException();

        }
    }
}
