using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {

            Engine.InitializeServer();
            CardDictionary.InitCardMap();
            Console.ReadLine();
        }
    }

    
}
