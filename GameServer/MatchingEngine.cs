using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public static class MatchingEngine
    {
        public static Queue<int> matchingQueue = new Queue<int>();

        public static void MatchPlayer(int connectionID)
        {
            if(matchingQueue.Count == 0)
            {
                matchingQueue.Enqueue(connectionID);
                Console.WriteLine("Queuing player: {0}", ClientManager.clients[connectionID].name);
            }
            else
            {
                GameEngineManager.CreateEngine(connectionID, matchingQueue.Dequeue());
            }
        }
    }
}
