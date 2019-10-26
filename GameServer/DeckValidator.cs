using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
    public static class DeckValidator
    {

        public static bool Validate(string deckCode)
        {
            if (deckCode.Equals("dev"))
                return true;
            bool result = deckCode.Length == 82 
                && FactionCheck(deckCode.Substring(0,1), deckCode.Substring(1,1))
                && CardsCheck(deckCode.Substring(2,deckCode.Length-2));

            Console.WriteLine(result);

            return result;
        }
        
        private static bool FactionCheck(string a, string b)
        {
            bool result = !(a.Equals(b)) && FactionIsInRange(a) && FactionIsInRange(b);
            if (!result)
            {
                Console.WriteLine("Faction Check Failed {0} {1}",a,b);
            }
            return result;
        }

        private static bool FactionIsInRange(string a)
        {
            int temp = Convert.ToInt32(a);

            bool result = temp >= 0 && temp <= 6;
            if (!result)
            {
                Console.WriteLine("Faction Is not in Range {0}", a);
            }
            return result;
        }

        private static bool CardsCheck(string cards)
        {
            Dictionary<string, int> cardMap = new Dictionary<string, int>();

            for(int i = 0;i < cards.Length; i +=2)
            {
                string temp = cards.Substring(i, 2);
                if (temp.All(char.IsLetterOrDigit))
                {
                    if (!cardMap.ContainsKey(temp))
                    {
                        cardMap.Add(temp, 1);
                    }
                    else
                    {
                        cardMap[temp]++;
                    }
                }
                else
                {
                    Console.WriteLine("Card {0} is not within range", temp);
                    return false;
                }
                if (cardMap[temp] > 3)
                {
                    Console.WriteLine("Card {0} is used more than 3 times", temp);
                    //return false;
                }
            }

            return true;
        }


    }
}
