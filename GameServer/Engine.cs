using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    static class Engine
    {
        public static void InitializeServer()
        {
            Console.WriteLine("Initializing Server...");
            ServerTCP.InitNetwork();
            Console.WriteLine("Server has started!");
        }
    }
}
