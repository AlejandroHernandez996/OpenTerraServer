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
            cardMap.Add("00", new Card("Placeholder_00", 2, 2, "00",1));
            cardMap.Add("01", new Card("Placeholder_01", 2, 1, "01",1));
            cardMap.Add("02", new Card("Placeholder_02", 1, 1, "02",1));
            cardMap.Add("03", new Card("Placeholder_03", 2, 2, "03",2));
            cardMap.Add("04", new Card("Placeholder_04", 2, 2, "04",2));
            cardMap.Add("05", new Card("Placeholder_05", 1, 1, "05", 1));
            cardMap.Add("06", new Card("Placeholder_06", 3, 3, "06", 4));
            cardMap.Add("07", new Card("Placeholder_07", 6, 6, "07", 6));
            cardMap.Add("08", new Card("Placeholder_08", 2, 2, "08", 4));
            cardMap.Add("09", new Card("Placeholder_09", 2, 3, "09", 4));
            cardMap.Add("0a", new Card("Placeholder_0a", 7, 7, "0a", 7));
            cardMap.Add("0b", new Card("Placeholder_0b", 2, 4, "0b", 3));
            cardMap.Add("0c", new Card("Placeholder_0c", 1, 4, "0c", 2));
            cardMap.Add("0d", new Card("Placeholder_0d", 5, 3, "0d", 5));
            cardMap.Add("0e", new Card("Placeholder_0e", 2, 2, "0e", 2));
            cardMap.Add("0f", new Card("Placeholder_0f", 2, 3, "0f", 4));
            cardMap.Add("0g", new Card("Placeholder_0g", 4, 1, "0g", 6));
            cardMap.Add("0h", new Card("Placeholder_0h", 1, 4, "0h", 3));
            cardMap.Add("0i", new Card("Placeholder_0i", 1, 4, "0i", 4));
            cardMap.Add("0j", new Card("Placeholder_0j", 3, 3, "0j", 4));
            cardMap.Add("0k", new Card("Placeholder_0k", 3, 2, "0k", 3));
            cardMap.Add("0l", new Card("Placeholder_0l", 2, 1, "0l", 4));
            cardMap.Add("0m", new Card("Placeholder_0m", 9, 9, "0m", 9));
            cardMap.Add("0n", new Card("Placeholder_0n", 2, 3, "0n", 2));
            cardMap.Add("0o", new Card("Placeholder_0o", 3, 3, "0o", 3));
            cardMap.Add("0p", new Card("Placeholder_0p", 3, 2, "0p", 2));
            cardMap.Add("0q", new Card("Placeholder_0q", 5, 5, "0q", 5));
            cardMap.Add("0r", new Card("Placeholder_0r", 2, 2, "0r", 2));
            cardMap.Add("0s", new Card("Placeholder_0s", 3, 3, "0s", 3));
            cardMap.Add("0t", new Card("Placeholder_0t", 3, 3, "0t", 3));
            cardMap.Add("0u", new Card("Placeholder_0u", 4, 2, "0u", 3));
            cardMap.Add("0v", new Card("Placeholder_0v", 5, 4, "0v", 5));
            cardMap.Add("0w", new Card("Placeholder_0w", 5, 5, "0w", 5));
            cardMap.Add("0x", new Card("Placeholder_0x", 6, 6, "0x", 6));
            cardMap.Add("0y", new Card("Placeholder_0y", 3, 1, "0y", 4));

            cardMap["02"].toughness = 1;
            cardMap["04"].toughness = 1;
            cardMap["0a"].toughness = 1;
            cardMap["0q"].toughness = 1;

            cardMap["0d"].isBarrier = true;
            cardMap["0m"].isBarrier = true;

        }

    }
}
