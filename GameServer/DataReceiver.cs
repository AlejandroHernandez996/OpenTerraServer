using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public enum ClientPackets
    {
        CHelloServer = 1, CNameDeckServer = 2, CMulligan = 3
    }
    class DataReceiver
    {
        public static void HandleHelloServer(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInt();
            string msg = buffer.ReadString();

            Console.WriteLine(msg);

            buffer.Dispose();
        }

        public static void HandleNameAndDeck(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInt();
            string playerName = buffer.ReadString();
            string playerDeck = buffer.ReadString();

            Console.WriteLine("Player: {0} and Deck Code: {1}\nValidating deck...", playerName, playerDeck);

            if (DeckValidator.Validate(playerDeck))
            {
                Console.WriteLine("Deck is valid");
                ClientManager.clients[connectionID].name = playerName;
                ClientManager.clients[connectionID].deck = playerDeck;

                MatchingEngine.MatchPlayer(connectionID);

            }
            else
            {
                Console.WriteLine("Deck is invalid");
                ClientManager.clients[connectionID].socket.Close();

            }

            buffer.Dispose();
        }

        public static void HandleMulligan(int connectionID, byte[] data)
        {
            Console.WriteLine("Player mulligan received");
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInt();

            int mulliganSize = buffer.ReadInt();
            List<Card> cardsMulliganed = new List<Card>();

            for(int x = 0; x < mulliganSize; x++)
            {
                cardsMulliganed.Add(CardDictionary.cardMap[buffer.ReadString()]);
            }

            GameEngineManager.gameEngines[ClientManager.clients[connectionID].gameEngineID].Mulligan(connectionID, cardsMulliganed);
        }

    }
}
