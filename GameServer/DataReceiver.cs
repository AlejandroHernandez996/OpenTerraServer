using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public enum ClientPackets
    {
        CConnect = 1, CNameDeckServer = 2, CMulligan = 3, CHandToBench = 4, CPass = 5, CBenchToAttack = 6, CAttack = 7, CDefend = 8,
        CBattle = 9,
    }
    class DataReceiver
    {
        public static void HandleConnect(int connectionID, byte[] data)
        {
            DataSender.SendConnect(connectionID);
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
            buffer.Dispose();

        }

        public static void HandleHandToBench(int connectionID, byte[] data)
        {
            Console.WriteLine("Verifying hand to bench from {0}", connectionID);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInt();
            GameEngineManager.gameEngines[ClientManager.clients[connectionID].gameEngineID].HandToBench(connectionID, buffer.ReadString());
            buffer.Dispose();
        }
        public static void HandleBenchToAttack(int connectionID, byte[] data)
        {
            Console.WriteLine("Verifying bench to attack from {0}", connectionID);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInt();
            GameEngineManager.gameEngines[ClientManager.clients[connectionID].gameEngineID].BenchToAttack(connectionID, buffer.ReadString());
            buffer.Dispose();
        }

        public static void HandlePass(int connectionID, byte[] data)
        {
            GameEngineManager.gameEngines[ClientManager.clients[connectionID].gameEngineID].Pass(connectionID);
        }

        public static void HandleAttack(int connectionId, byte[] data)
        {
            Console.WriteLine("Handling Attack");
            GameEngineManager.gameEngines[ClientManager.clients[connectionId].gameEngineID].Attack(connectionId);
        }
        public static void HandleDefend(int connectionID, byte[] data)
        {

            Console.WriteLine("Handling Defend");

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInt();

            GameEngineManager.gameEngines[ClientManager.clients[connectionID].gameEngineID].Defend(buffer.ReadString(),connectionID,buffer.ReadInt());

        }

        public static void HandleBattle(int connectionId, byte[] data)
        {
            GameEngineManager.gameEngines[ClientManager.clients[connectionId].gameEngineID].Battle(connectionId);
        }
    }
}
