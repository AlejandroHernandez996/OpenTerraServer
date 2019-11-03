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
            packets.Add((int)ClientPackets.CNameDeckServer, DataReceiver.HandleNameAndDeck);
            packets.Add((int)ClientPackets.CMulligan, DataReceiver.HandleMulligan);
            packets.Add((int)ClientPackets.CHandToBench, DataReceiver.HandleHandToBench );
            packets.Add((int)ClientPackets.CPass, DataReceiver.HandlePass);
            packets.Add((int)ClientPackets.CBenchToAttack, DataReceiver.HandleBenchToAttack);
            packets.Add((int)ClientPackets.CAttack, DataReceiver.HandleAttack);
            packets.Add((int)ClientPackets.CDefend, DataReceiver.HandleDefend);
            packets.Add((int)ClientPackets.CBattle, DataReceiver.HandleBattle);
            packets.Add((int)ClientPackets.CChallenge, DataReceiver.HandleChallenge);
            packets.Add((int)ClientPackets.CBattlecryTarget, DataReceiver.HandleBattlecryTarget);

        }
        public static void HandleData(int connectionID, byte[] data)
        {
            byte[] buffer = (byte[])data.Clone();
            int packetLength = 0;

            if(ClientManager.clients[connectionID].buffer == null)
            {
                ClientManager.clients[connectionID].buffer = new ByteBuffer();
            }

            ByteBuffer clientBuffer = ClientManager.clients[connectionID].buffer;

            clientBuffer.WriteBytes(buffer);
            if(clientBuffer.GetBufferSize() == 0)
            {

                clientBuffer.ClearBuffer();
                return;

            }
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
                if(clientBuffer.GetRemainingBufferLength() >= 4)
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
            if(packets.TryGetValue(packetID, out Packet packet))
            {
                packet.Invoke(connectionID, data);
            }
        }
    }
}
