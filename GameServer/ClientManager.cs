using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GameServer
{
    public static class ClientManager
    {
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();


        public static void CreateNewConnection(TcpClient tempClient)
        {
            Client client = new Client();

            client.socket = tempClient;
            client.connectionID = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
            client.Start();
            clients.Add(client.connectionID, client);

        }

        public static void sendDataTo(int connectionId, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((data.GetUpperBound(0) - data.GetLowerBound(0) + 1));
            buffer.WriteBytes(data);
            clients[connectionId].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer.Dispose();
        }
    }
}
