using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace GameServer
{
    static class ServerTCP
    {
        static TcpListener serverSocket = new TcpListener(IPAddress.Any, 6969);

        public static  void InitNetwork()
        {
            Console.WriteLine("INIT Packets...");
            ServerHandleData.InitializePackets();
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
        }
        private static void OnClientConnect(IAsyncResult ar)
        {
            TcpClient client = serverSocket.EndAcceptTcpClient(ar);
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
            ClientManager.CreateNewConnection(client);
        }
    }
}
