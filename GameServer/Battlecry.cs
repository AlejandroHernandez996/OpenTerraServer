namespace GameServer
{
    public enum BattlecryType { targetAlly = 1}
    public abstract class Battlecry
    {
        public virtual BattlecryType type { get; protected set; }
        public abstract void TargetAlly(Card card);

    }
}