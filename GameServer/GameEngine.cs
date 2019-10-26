using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class GameEngine
    {
        public int id;
        public int player1ID;
        public int player2ID;
        public GameEngine(int id, int p1, int p2)
        {
            this.id = id;
            player1ID = p1;
            player2ID = p2;
        }

    }
}
