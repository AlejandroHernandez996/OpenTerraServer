using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public enum ClientPackets
    {
        CHelloServer = 1, CNameDeckServer = 2
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

    }
}
