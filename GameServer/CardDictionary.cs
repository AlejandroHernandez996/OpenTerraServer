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
            cardMap.Add("00", new Card("Placeholder_00", 2, 2, "00"));
            cardMap.Add("01", new Card("Placeholder_01", 2, 2, "01"));
            cardMap.Add("02", new Card("Placeholder_02", 2, 2, "02"));
            cardMap.Add("03", new Card("Placeholder_03", 2, 2, "03"));
            cardMap.Add("04", new Card("Placeholder_04", 2, 2, "04"));
            cardMap.Add("05", new Card("Placeholder_05", 2, 2, "05"));
            cardMap.Add("06", new Card("Placeholder_06", 2, 2, "06"));
            cardMap.Add("07", new Card("Placeholder_07", 2, 2, "07"));
            cardMap.Add("08", new Card("Placeholder_08", 2, 2, "08"));
            cardMap.Add("09", new Card("Placeholder_09", 2, 2, "09"));
            cardMap.Add("0a", new Card("Placeholder_0a", 2, 2, "0a"));
            cardMap.Add("0b", new Card("Placeholder_0b", 2, 2, "0b"));
            cardMap.Add("0c", new Card("Placeholder_0c", 2, 2, "0c"));
            cardMap.Add("0d", new Card("Placeholder_0d", 2, 2, "0d"));
            cardMap.Add("0e", new Card("Placeholder_0e", 2, 2, "0e"));
            cardMap.Add("0f", new Card("Placeholder_0f", 2, 2, "0f"));
            cardMap.Add("0g", new Card("Placeholder_0g", 2, 2, "0g"));
            cardMap.Add("0h", new Card("Placeholder_0h", 2, 2, "0h"));

        }

    }
}
