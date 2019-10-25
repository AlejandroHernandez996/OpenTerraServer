using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public enum ClientPackets
    {
        CHelloServer = 1,
    }
    class DataReceiver
    {
        public static void HandleHelloServer(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInt();
            string msg = buffer.ReadString();
            buffer.Dispose();
        }

    }
}
