using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public static class ServerHandleData
    {
        public delegate void Packet(int connectionID, byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        public static void InitializePackets()
        {
            packets.Add((int)ClientPackets.CHelloServer, DataReceiver.HandleHelloServer);
        }
        public static void HandleData(int connectionID, byte[] data)
        {
            if (data.Length == 0)
                return;

            byte[] buffer = (byte[])data.Clone();
            int packetLength = 0;
            ByteBuffer clientBuffer = ClientManager.clients[connectionID].buffer;

            if(clientBuffer == null)
            {
                clientBuffer = new ByteBuffer();
            }

            clientBuffer.WriteBytes(buffer);

            if(clientBuffer.GetBufferSize() >= 4)
            {
                packetLength = clientBuffer.ReadInt(false);
                if(packetLength <= 0)
                {
                    clientBuffer.ClearBuffer();
                    return;
                }
            }

            while(packetLength > 0 & packetLength <= clientBuffer.GetBufferSize() - 4)
            {
                if (packetLength <= clientBuffer.GetBufferSize() - 4)
                {
                    clientBuffer.ReadInt();
                    data = clientBuffer.ReadBytes(packetLength);
                    HandleDataPackets(connectionID, data);
                }

                packetLength = 0;
                if(clientBuffer.GetBufferSize() >= 4)
                {
                    packetLength = clientBuffer.ReadInt(false);
                    if (packetLength <= 0)
                    {
                        clientBuffer.ClearBuffer();
                        return;
                    }
                }
            }
        }
        private static void HandleDataPackets(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInt();
            buffer.Dispose();
            if(packets.TryGetValue(connectionID, out Packet packet))
            {
                packet.Invoke(connectionID, data);
            }
        }
    }
}
