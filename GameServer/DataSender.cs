using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{   
    public enum ServerPackets
    {
        SWelcomeMessage = 1,
    }
    static class DataSender
    {
        public static void SendWelcomeMessage(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackets.SWelcomeMessage);
            buffer.WriteString("Welcome to the server");
            ClientManager.sendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }
    }
}
