using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace GameServer
{
    public class Client
    {
        public int connectionID;
        public TcpClient socket;
        public NetworkStream stream;
        private byte[] recBuffer;
        public ByteBuffer buffer;


        public void Start()
        {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            stream = socket.GetStream();
            stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            Console.WriteLine("Incoming connection from '{0}'.", socket.Client.RemoteEndPoint.ToString());
        }

        private void OnReceiveData(IAsyncResult ar)
        {
            try
            {
                int length = stream.EndRead(ar);

                if(length <= 0)
                {
                    CloseConnection();
                    return;
                }

                byte[] newBytes = new byte[length];
                Array.Copy(recBuffer, newBytes, length);
                ServerHandleData.HandleData(connectionID,newBytes);
                stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);

            }catch (Exception)
            {
                CloseConnection();
                return;
            }
        }
        private void CloseConnection()
        {
            Console.WriteLine("Connection from '{0}' has been terminated.", socket.Client.RemoteEndPoint.ToString());
            socket.Close();
        }
    }
}
