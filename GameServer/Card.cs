using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class Card
    {
        public int health;
        public int attack;
        public int mana;

        public string name;
        public string id;

        public bool isBarrier = false;
        public bool isChallenger = false;
        public bool isQuickAttack = false;
        public bool isElusive = false;
        public bool hasBattlecry = false;
        public Battlecry battlecry;
        public int toughness = 0;
        public Card(string name, int attack, int health, string id, int mana)
        {
            this.name = name;
            this.health = health;
            this.attack = attack;
            this.id = id;
            this.mana = mana;
        }

        public Card DeepCopy()
        {
            Card clone = (Card)this.MemberwiseClone();

            return clone;
        }

    }
}
