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
        public Dictionary<int, List<Card>> playerMapBench;
        public Dictionary<int, List<Card>> playerMapAttackZone;

        public Dictionary<int, int> playerMapHealth;
        public Dictionary<int, int> playerMapMana;
        public Dictionary<int, int> playerMapSpellMana;
        public Dictionary<int, int> playerToPlayer;

        public Dictionary<int, bool> playerIsDefending;
        public Dictionary<int, bool> playerIsAttacking;


        public string phase;
        public bool isMulliganDone = false;
        public bool endTurn = false;
        public bool canAttackerAttack = true;
        public bool challengerAttack = false;
        public bool targetAlly = false;
        public int attackPlayerId;
        public int turnPlayerId;
        public int turn = -1;

        public GameEngine(int id, int p1, int p2)
        {
            Console.WriteLine("Instanciating GameEngine for {0} and {1}", ClientManager.clients[p1].name, ClientManager.clients[p2].name);
            this.id = id;
            player1ID = p1;
            player2ID = p2;
            phase = "Mulligan";
            InitializeVariables();
            DrawCards(4, player1ID);
            DrawCards(4, player2ID);
            SendUpdatedBoardAndStats();
        }
        private void InitializeVariables()
        {
            turn = 1;
            players = new List<int>();

            players.Add(player1ID);
            players.Add(player2ID);

            playerMapDeck = new Dictionary<int, List<Card>>();
            playerMapHand = new Dictionary<int, List<Card>>();
            playerMapBench = new Dictionary<int, List<Card>>();
            playerMapAttackZone = new Dictionary<int, List<Card>>();

            playerMapHealth = new Dictionary<int, int>();
            playerMapMana = new Dictionary<int, int>();
            playerMapSpellMana = new Dictionary<int, int>();

            playerIsAttacking = new Dictionary<int, bool>();
            playerIsDefending = new Dictionary<int, bool>();


            playerToPlayer = new Dictionary<int, int>();
            playerToPlayer[player1ID] = player2ID;
            playerToPlayer[player2ID] = player1ID;

            foreach (int player in players)
            {
                
                playerMapHand.Add(player, new List<Card>());
                playerMapBench.Add(player, new List<Card>());
                playerMapAttackZone.Add(player, new List<Card>());
                playerMapHealth.Add(player, 20);
                playerMapMana.Add(player, 1);
                playerMapSpellMana.Add(player, 0);

                playerIsAttacking.Add(player, false);
                playerIsDefending.Add(player, false);

                CreateDeck(ClientManager.clients[player].deck, player);
            }
            turnPlayerId = player1ID;
            attackPlayerId = player1ID;
        }

        public void Challenge(int playerID, string challengedCard, int index)
        {
            if (!playerIsAttacking[playerID] || playerID != turnPlayerId || !challengerAttack)
            {
                Console.WriteLine("Player attack is {0} and attacking player is {1} and challenger attack is {2}", playerIsAttacking[playerID], ClientManager.clients[turnPlayerId], challengerAttack);
                return;
            }
            challengerAttack = false;
            if (playerMapBench[playerToPlayer[playerID]][index].id.Equals(challengedCard))
            {
                playerMapAttackZone[playerToPlayer[playerID]][playerMapAttackZone[playerID].Count-1] = playerMapBench[playerToPlayer[playerID]][index];
                playerMapBench[playerToPlayer[playerID]].RemoveAt(index);
            }
            else
            {
                Console.Write("Card at {0} is {1| not {3}", index, playerMapAttackZone[playerToPlayer[playerID]][index].id, challengedCard);
            }
            SendUpdatedBoardAndStats();
        }

        private void CreateDeck(string deck, int id)
        {
            List<Card> tempList = new List<Card>();
            

            for(int i = 2;i < deck.Length;i += 2)
            {
                tempList.Add(CardDictionary.cardMap[deck.Substring(i, 2)].DeepCopy());
            }
            ShuffleDeck(tempList);
            playerMapDeck.Add(id, tempList);

        }
        public void HandToBench(int playerID, string card)
        {
            endTurn = false;
            if(turnPlayerId != playerID || targetAlly)
            {
                Console.WriteLine("Not {0}'s turn", playerID);
                return;
            }
            bool found = false;
            Card temp = null;
            for(int n = 0;n < playerMapHand[playerID].Count;n++)
            {
                if (playerMapHand[playerID][n].id.Equals(card))
                {
                    if(playerMapHand[playerID][n].mana > playerMapMana[playerID])
                    {
                        return;
                    }
                    temp = playerMapHand[playerID][n];
                    playerMapMana[playerID] -= playerMapHand[playerID][n].mana;
                    found = true;
                    playerMapHand[playerID].RemoveAt(n);

                    break;
                }
            }
            if (!found)
            {
                return;
            }

            playerMapBench[playerID].Add(temp);

            if (temp.hasBattlecry & (int)temp.battlecry.type == (int)BattlecryType.targetAlly & playerMapBench[playerID].Count > 1)
            {
                InitBattlecry(temp,playerID);
            }
            else
            {
                turnPlayerId = playerToPlayer[playerID];
                phase = ClientManager.clients[turnPlayerId].name + "'s Turn";
            }

            SendUpdatedBoardAndStats();
        }

        public void InitBattlecry(Card card, int player)
        {
                Console.WriteLine("WAITING FOR TARGET");
                phase = ClientManager.clients[turnPlayerId].name + "'s Choose Friendly Target In Bench";
                targetAlly = true;
        }

        public void BattlecryTargetAlly(string card, int player, int index)
        {
            if(turnPlayerId == player & targetAlly & playerMapBench[player].Count > index)
            {
                Console.WriteLine("Setting target effect");
                targetAlly = false;
                playerMapBench[player][playerMapBench[player].Count - 1].battlecry.TargetAlly(playerMapBench[player][index]);
                turnPlayerId = playerToPlayer[player];
                phase = ClientManager.clients[turnPlayerId].name + "'s Turn";
                SendUpdatedBoardAndStats();
            }
        }
        public void Battle(int player)
        {
            Console.WriteLine("SENT BATTLE {0}", ClientManager.clients[player].name);
            if(!playerIsDefending[player] || turnPlayerId != player)
            {
                Console.WriteLine("Player defend is {0} and turnPlayer is {1} ", playerIsDefending[player], ClientManager.clients[turnPlayerId].name);
                return;
            }
           
                int enemy = playerToPlayer[player];
                for (int x = 0; x < playerMapAttackZone[enemy].Count; x++)
                {
                    if (playerMapAttackZone[player][x] == null)
                    {
                        playerMapHealth[player] -= playerMapAttackZone[enemy][x].attack;
                        playerMapBench[enemy].Add(playerMapAttackZone[enemy][x]);
                    }
                    else
                    {

                        Console.WriteLine("Card: {0} Stats: {1}, {2} vs Card: {3} Stats: {4}, {5}",
                            playerMapAttackZone[player][x].name, playerMapAttackZone[player][x].health, playerMapAttackZone[player][x].attack,
                            playerMapAttackZone[enemy][x].name, playerMapAttackZone[enemy][x].health, playerMapAttackZone[enemy][x].attack
                            );
                    
                    CalculateBattle(playerMapAttackZone[player][x], playerMapAttackZone[enemy][x], x);

                        Console.WriteLine("Card: {0} Stats: {1}, {2} vs Card: {3} Stats: {4}, {5}",
                            playerMapAttackZone[player][x].name, playerMapAttackZone[player][x].health, playerMapAttackZone[player][x].attack,
                            playerMapAttackZone[enemy][x].name, playerMapAttackZone[enemy][x].health, playerMapAttackZone[enemy][x].attack
                            );

                    foreach(int p in players)
                    {
                        if (playerMapAttackZone[p][x].health > 0)
                        {
                            playerMapBench[p].Add(playerMapAttackZone[p][x]);
                        }
                    }

                    }
            }

            if (playerMapHealth[player1ID] <= 0)
            {
                DataSender.SendEndGame(player1ID, "Defeat");
                DataSender.SendEndGame(player2ID, "Victory");

            }
            else if (playerMapHealth[player2ID] <= 0)
            {
                DataSender.SendEndGame(player2ID, "Defeat");
                DataSender.SendEndGame(player1ID, "Victory");
            }
            playerMapAttackZone[player] = new List<Card>();
            playerMapAttackZone[playerToPlayer[player]] = new List<Card>();
            playerIsAttacking[playerToPlayer[player]] = false;
            playerIsDefending[player] = false;
            canAttackerAttack = false;
            turnPlayerId = playerToPlayer[turnPlayerId];
            phase = ClientManager.clients[turnPlayerId].name + "'s Turn";
            SendUpdatedBoardAndStats();

          
        }

        private void CalculateBattle(Card player, Card enemy, int x)
        {

            if(player.isQuickAttack && !enemy.isQuickAttack)
            {
                damageCard(player.attack, enemy);
                if(enemy.health <= 0)
                {
                    return;
                }
            }else if(!player.isQuickAttack && enemy.isQuickAttack)
            {
                damageCard(enemy.attack, player);
                if (player.health <= 0)
                {
                    return;
                }
            }
            else
            {
                damageCard(player.attack, enemy);
                damageCard(enemy.attack, player);
            }

        }

        private void damageCard(int damage, Card card)
        {
            if (card.isBarrier)
            {
                card.isBarrier = false;
                return;
            }
            int damge = damage - card.toughness;
            card.health -= damge;
        }

        public void Defend(string card, int playerID, int index)
        {
            Console.WriteLine("DEFENDING");
            if(!playerIsDefending[playerID] || turnPlayerId != playerID)
            {
                Console.WriteLine("Player defend is {0} and turnPlayer is {1} ", playerIsDefending[playerID], ClientManager.clients[turnPlayerId].name);
                return;
            }
            if(playerMapAttackZone[playerToPlayer[playerID]][index].isElusive && !CardDictionary.cardMap[card].isElusive)
            {
                Console.Write("CANT DEFEND ELUSIVE");
                return;
            }
            bool found = false;
            Card temp = null;
            for (int n = 0; n < playerMapBench[playerID].Count; n++)
            {
                if (playerMapBench[playerID][n].id.Equals(card))
                {
                    temp = playerMapBench[playerID][n];
                    found = true;
                    playerMapBench[playerID].RemoveAt(n);
                    break;
                }
            }
            if (!found)
            {
                return;
            }
            if(playerMapAttackZone[playerID].Count == 0)
            {
                for (int x = 0; x < playerMapAttackZone[playerToPlayer[playerID]].Count; x++)
                {
                    playerMapAttackZone[playerID].Add(null);
                }
            }
            Console.WriteLine(playerMapAttackZone[playerID].Count);
            Console.WriteLine(index);
            playerMapAttackZone[playerID][index] = temp;
            SendUpdatedBoardAndStats();
        }

        public void Attack(int playerID)
        {
            if (!playerIsAttacking[playerID] || playerID != turnPlayerId)
            {
                Console.WriteLine("Player attack is {0} and attacking player is {1} ", playerIsAttacking[playerID], ClientManager.clients[turnPlayerId]);
                return;
            }

            playerIsAttacking[turnPlayerId] = false;
            turnPlayerId = playerToPlayer[playerID];
            playerIsDefending[turnPlayerId] = true;
            phase = ClientManager.clients[turnPlayerId].name + "'s Turn";
            Console.WriteLine("{0} sent attack it is {1} turn to defend", ClientManager.clients[playerID].name, ClientManager.clients[turnPlayerId].name);
            SendUpdatedBoardAndStats();
        }

        public void BenchToAttack(int playerID, string card)
        {
            if (turnPlayerId != playerID || attackPlayerId != playerID)
            {
                Console.WriteLine("Not {0}'s turn", playerID);
                return;
            }
            bool found = false;
            Card temp = null;
            for (int n = 0; n < playerMapBench[playerID].Count; n++)
            {
                if (playerMapBench[playerID][n].id.Equals(card))
                {
                    found = true;
                    temp = playerMapBench[playerID][n];
                    playerMapBench[playerID].RemoveAt(n);

                    break;
                }
            }
            if (!found)
            {
                return;
            }

            challengerAttack = temp.isChallenger;
            Console.WriteLine("Challenger approaches {0}", challengerAttack);
            
            playerIsAttacking[playerID] = true;
            playerIsDefending[playerToPlayer[playerID]] = true;
            playerMapAttackZone[playerID].Add(temp);
            playerMapAttackZone[playerToPlayer[playerID]].Add(null);

            SendUpdatedBoardAndStats();
            
        }
        private void SendUpdatedBoardAndStats()
        {
            foreach (int player in players)
            {
                DataSender.SendUpdatedStats(player, PackStats(player, playerToPlayer[player]));
            }
            foreach (int player in players)
            {
                DataSender.SendUpdatedBoard(player, playerMapHand[player], playerMapBench[player], playerMapAttackZone[player],
                playerMapBench[playerToPlayer[player]], playerMapAttackZone[playerToPlayer[player]]);
            }
            
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
            buffer.WriteInt(playerMapHand[enemy].Count);
            buffer.WriteBool(playerIsAttacking[player]);
            buffer.WriteBool(playerIsDefending[player]);
            buffer.WriteBool(player == attackPlayerId ? true : false);
            buffer.WriteBool(player == attackPlayerId ? canAttackerAttack : false);

            Console.WriteLine("PACKED STATS FOR {0}", player);
            return buffer;
        }
        public void DrawCards(int size, int id)
        {
            for(int x = 0;x < size; x++)
            {
                playerMapHand[id].Add(playerMapDeck[id][playerMapDeck[id].Count-1]);
                playerMapDeck[id].RemoveAt(playerMapDeck[id].Count - 1);
            }
        }

        public void Mulligan(int playerID, List<Card> cards)
        {
            phase = ClientManager.clients[player1ID].name + "'s TURN";
            foreach(Card card in cards)
            {
                int index = -1;
                for(int x =0;x < playerMapHand[playerID].Count; x++)
                {
                    if (card.id.Equals(playerMapHand[playerID][x].id))
                    {
                        index = x;
                        break;
                    }
                }
                if(index != -1)
                {
                    playerMapHand[playerID].RemoveAt(index);
                }
            }

            playerMapDeck[playerID].AddRange(cards);

            ShuffleDeck(playerMapDeck[playerID]);
            DrawCards(cards.Count, playerID);

            DataSender.SendMulligan(playerID, playerMapHand[playerID]);
            DataSender.SendUpdatedBoard(playerID, playerMapHand[playerID], playerMapBench[playerID], playerMapAttackZone[playerID],
                playerMapBench[playerToPlayer[playerID]], playerMapAttackZone[playerToPlayer[playerID]]);
            if (isMulliganDone)
            {
                DrawCards(1, player1ID);
                DrawCards(1, player2ID);
                attackPlayerId = player1ID;
                SendUpdatedBoardAndStats();
            }
            isMulliganDone =  false ? true : true;

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

        public void Pass(int player)
        {
            Console.WriteLine("Player {0} passed", ClientManager.clients[player].name);
            if(turnPlayerId != player)
            {
                return;
            }
            
            if (endTurn)
            {
                NextTurn();
            }
            else
            {
                endTurn = true;
                turnPlayerId = playerToPlayer[turnPlayerId];
                phase = ClientManager.clients[turnPlayerId].name + "'s Turn";
                DataSender.SendUpdatedStats(player1ID, PackStats(player1ID, player2ID));
                DataSender.SendUpdatedStats(player2ID, PackStats(player2ID, player1ID));
            }
        }
        private void NextTurn()
        {
            turn++;

            turnPlayerId = playerToPlayer[attackPlayerId];
            attackPlayerId = turnPlayerId;
            canAttackerAttack = true;
            Console.WriteLine("It is {0} turn to attack", ClientManager.clients[attackPlayerId].name);
            phase = ClientManager.clients[turnPlayerId].name + "'s Turn";
            foreach(int player in players)
            {
                playerMapSpellMana[player] = playerMapSpellMana[player] + playerMapMana[player] > 3 ? 3 : playerMapMana[player];
                playerMapMana[player] = turn > 10 ? 10 : turn;
                DrawCards(1, player);
            }
            SendUpdatedBoardAndStats();

            endTurn = false;
        }


    }
}
