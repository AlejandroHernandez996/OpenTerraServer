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
        public Dictionary<int, Stack<Card>> playerMapDeck;
        public Dictionary<int, List<Card>> playerMapHand;

        public GameEngine(int id, int p1, int p2)
        {
            Console.WriteLine("Instanciating GameEngine for {0} and {1}", ClientManager.clients[p1].name, ClientManager.clients[p2].name);

            this.id = id;
            player1ID = p1;
            player2ID = p2;

            players = new List<int>();
            players.Add(p1);
            players.Add(p2);

            playerMapDeck = new Dictionary<int, Stack<Card>>();
            playerMapHand = new Dictionary<int, List<Card>>();

            playerMapHand.Add(p1, new List<Card>());
            playerMapHand.Add(p2, new List<Card>());

            foreach(int player in players)
            {
                CreateDeck(ClientManager.clients[player].deck, player);

            }

            StartGame();
        }

        private void CreateDeck(string deck, int id)
        {
            Stack<Card> tempStack = new Stack<Card>();

            for(int i = 2;i < deck.Length;i += 2)
            {
                tempStack.Push(CardDictionary.cardMap[deck.Substring(i, 2)]);
            }

            playerMapDeck.Add(id, tempStack);

        }

        private void StartGame()
        {
            foreach (int player in players)
            {
                DrawCards(5, player);
                DataSender.SendStartGame(player, playerMapHand[player]);
            }

        }

        public void DrawCards(int count, int id)
        {
            for(int x = 0;x < count; x++)
            {
                playerMapHand[id].Add(playerMapDeck[id].Pop());
            }
        }



    }
}
