using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class Card
    {
        public int health;
        public int attack;
        public string name;
        public string id;
        public Card(string name, int health, int attack, string id)
        {
            this.name = name;
            this.health = health;
            this.attack = attack;
            this.id = id;
        }

    }
}
