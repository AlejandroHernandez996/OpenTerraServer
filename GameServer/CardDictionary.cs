using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    static class CardDictionary
    {

        public static Dictionary<string, Card> cardMap = new Dictionary<string, Card>();

        public static void InitCardMap()
        {
            cardMap.Add("00", new Card("Placeholder", 2, 2, "00"));
        }

    }
}
