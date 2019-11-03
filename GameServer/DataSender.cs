using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{   
    public enum ServerPackets
    {
        SConnect = 1, SMatchMade = 2, SMulligan = 4, SUpdatedStats = 5, SUpdatedBoard = 7, SEndGame = 8
    }
    static class DataSender
    {

        public static void SendMatchMade(int connectionID, int player2ID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackets.SMatchMade);
            buffer.WriteString(ClientManager.clients[player2ID].name);
            ClientManager.sendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendConnect(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackets.SConnect);
            ClientManager.sendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendMulligan(int connectionID, List<Card> hand)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackets.SMulligan);
            buffer.WriteInt(hand.Count);
            foreach (Card card in hand)
            {
                buffer.WriteString(card.id);
            }

            ClientManager.sendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();

        }
        /*
         * PacketID, Turn, Phase, PlayerHealth, PlayerMana, PlayerSpellMana, PlayerDeckSize, EnemyHelath, EnemyMana, EnemySpellMana, EnemyDeckSize, EnemyHandSize
         */
        public static void SendUpdatedStats(int player, ByteBuffer byteBuffer)
        {
            ClientManager.sendDataTo(player, byteBuffer.ToArray());

        }
        public static void SendUpdatedBoard(int playerID, List<Card> playerHand, List<Card> playerBench, List<Card> playerAttackZone, List<Card> enemyBench, List<Card> enemyAttackZone)
        {
            Console.WriteLine("Sending Updated Board");
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackets.SUpdatedBoard);

            PackCards(playerHand, buffer);
            PackCards(playerBench, buffer);
            PackCards(playerAttackZone, buffer);
            PackCards(enemyBench, buffer);
            PackCards(enemyAttackZone, buffer);

            ClientManager.sendDataTo(playerID, buffer.ToArray());
            buffer.Dispose();

        }
        private static void PackCards(List<Card> cards, ByteBuffer buffer)
        {
            buffer.WriteInt(cards.Count);
            foreach (Card card in cards)
            {
                if (card == null)
                {
                    buffer.WriteString("BLOCKER");
                    buffer.WriteInt(-1);
                    buffer.WriteInt(-1);
                    buffer.WriteBool(false);
                }
                else
                {
                    buffer.WriteString(card.id);
                    buffer.WriteInt(card.health);
                    buffer.WriteInt(card.attack);
                    buffer.WriteBool(card.isBarrier);
                }
            }
        }

        public static void SendEndGame(int player, string msg)
        {
            Console.WriteLine("Sending End Game");
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackets.SEndGame);
            buffer.WriteString(msg);
            ClientManager.sendDataTo(player, buffer.ToArray());
            buffer.Dispose();
        }
    }
}
