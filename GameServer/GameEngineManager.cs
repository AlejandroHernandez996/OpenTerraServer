using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public static class GameEngineManager
    {
        public static int idCounter = 0;
        public static Dictionary<int, GameEngine> gameEngines = new Dictionary<int, GameEngine>();

        public static void CreateEngine(int player1, int player2)
        {
            Console.WriteLine("Player: {0} and Player: {1} Have been matched with match ID: {2}", 
            ClientManager.clients[player1].name, ClientManager.clients[player2].name, idCounter);
            ClientManager.clients[player1].gameEngineID = idCounter;
            ClientManager.clients[player2].gameEngineID = idCounter;
            gameEngines.Add(idCounter,new GameEngine(idCounter, player1, player2));
            idCounter++;
            DataSender.SendMatchMade(player1, player2);
            DataSender.SendMatchMade(player2, player1);
        }

    }
}
