using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{   
    public enum ServerPackets
    {
        SWelcomeMessage = 1, SMatchMade = 2
    }
    static class DataSender
    {
        public static void SendWelcomeMessage(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackets.SWelcomeMessage);
            buffer.WriteString("Welcome to the server");
            Console.WriteLine("Sending Welcome msg..." + buffer.GetBufferSize() + " bytes");
            ClientManager.sendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendMatchMade(int connectionID, int player2ID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackets.SMatchMade);
            buffer.WriteString(ClientManager.clients[player2ID].name);
            ClientManager.sendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }
        
    }
}
