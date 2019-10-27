using System;
using System.Collections.Generic;

namespace GameServer
{
    public class GameEngine
    {
        public int id;
        public int player1ID;
        public int player2ID;

        public List<int> players;
        public Dictionary<int, List<Card>> playerMapDeck;
        public Dictionary<int, List<Card>> playerMapHand;

        public Dictionary<int, int> playerMapHealth;
        public Dictionary<int, int> playerMapMana;
        public Dictionary<int, int> playerMapSpellMana;

        public string phase;

        public int turn = -1;
        public GameEngine(int id, int p1, int p2)
        {
            Console.WriteLine("Instanciating GameEngine for {0} and {1}", ClientManager.clients[p1].name, ClientManager.clients[p2].name);

            this.id = id;
            player1ID = p1;
            player2ID = p2;
            phase = "Mulligan";
            InitializeVariables();

            foreach(int player in players)
            {
                CreateDeck(ClientManager.clients[player].deck, player);
                Console.WriteLine("Deck created for {0}", player);

            }

            StartGame();
        }
        private void InitializeVariables()
        {
            turn = 1;
            players = new List<int>();

            players.Add(player1ID);
            players.Add(player2ID);

            playerMapDeck = new Dictionary<int, List<Card>>();
            playerMapHand = new Dictionary<int, List<Card>>();

            playerMapHand.Add(player1ID, new List<Card>());
            playerMapHand.Add(player2ID, new List<Card>());

            playerMapHealth = new Dictionary<int, int>();
            playerMapMana = new Dictionary<int, int>();
            playerMapSpellMana = new Dictionary<int, int>();

            Console.WriteLine("Maps init");
            foreach(int player in players)
            {
                playerMapHealth.Add(player, 20);
                playerMapMana.Add(player, 1);
                playerMapSpellMana.Add(player, 0);
            }
            Console.WriteLine("Stats set");
        }
        private void CreateDeck(string deck, int id)
        {
            List<Card> tempList = new List<Card>();
            

            for(int i = 2;i < deck.Length;i += 2)
            {
                tempList.Add(CardDictionary.cardMap[deck.Substring(i, 2)]);
            }
            ShuffleDeck(tempList);
            playerMapDeck.Add(id, tempList);

        }

        private void StartGame()
        {
            foreach (int player in players)
            {
                DrawCards(4, player);
                DataSender.SendStartGame(player, playerMapHand[player]);
            }
            DataSender.SendUpdatedStats(player1ID, PackStats(player1ID, player2ID));
            DataSender.SendUpdatedStats(player2ID, PackStats(player2ID, player1ID));


        }
        private ByteBuffer PackStats(int player, int enemy)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInt((int)ServerPackets.SUpdatedStats);
            buffer.WriteInt(turn);
            buffer.WriteString(phase);
            buffer.WriteInt(playerMapHealth[player]);
            buffer.WriteInt(playerMapMana[player]);
            buffer.WriteInt(playerMapSpellMana[player]);
            buffer.WriteInt(playerMapDeck[player].Count);
            buffer.WriteInt(playerMapHealth[enemy]);
            buffer.WriteInt(playerMapMana[enemy]);
            buffer.WriteInt(playerMapSpellMana[enemy]);
            buffer.WriteInt(playerMapDeck[enemy].Count);
            Console.WriteLine("PACKED STATS FOR {0}", player);
            return buffer;
        }
        public void DrawCards(int size, int id)
        {
            for(int x = 0;x < size; x++)
            {
                playerMapHand[id].Add(playerMapDeck[id][playerMapDeck[id].Count-1]);
                playerMapDeck[id].RemoveAt(playerMapDeck[id].Count - 1);
                Console.WriteLine("Drew starting hand for {0}", id);
            }
        }

        public void Mulligan(int playerID, List<Card> cards)
        {
            phase = ClientManager.clients[player1ID].name + "'s TURN";
            foreach(Card card in cards)
            {
                playerMapHand[playerID].Remove(card);
            }

            playerMapDeck[playerID].AddRange(cards);

            ShuffleDeck(playerMapDeck[playerID]);
            DrawCards(cards.Count, playerID);

            DataSender.SendMulligan(playerID, playerMapHand[playerID]);
            DataSender.SendUpdatedStats(playerID, PackStats(playerID, playerID == player1ID ? player2ID : player1ID));
        }

        public static void ShuffleDeck(List<Card> deck)
        {
            int n = deck.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }


    }
}
