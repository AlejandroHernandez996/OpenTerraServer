using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class Battlecry_03 : Battlecry
    {
        public override BattlecryType type
        {
            get { return BattlecryType.targetAlly; }
        }
        public override void TargetAlly(Card card)
        {
            card.isBarrier = true;
        }
    }
}
