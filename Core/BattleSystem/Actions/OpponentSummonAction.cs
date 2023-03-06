namespace Ceres.Core.BattleSystem
{
    public class OpponentSummonAction : IServerAction
    {
        public Card Card;
        public MultiCardSlotType SlotType;
        public int X;
        public int Y;

        public OpponentSummonAction(MultiCardSlotType slotType, int x, int y, Card card)
        {
            SlotType = slotType;
            Y = y;
            X = x;
            Card = card;
        }

        public void Apply(ClientBattle battle)
        {
            UnitSlot unitSlot = battle.AllyPlayer.GetSlotByPosition(X, Y);
            IMultiCardSlot multiSlot = battle.AllyPlayer.GetMultiCardSlot(SlotType);

            unitSlot.SetCard(Card);
            multiSlot.RemoveCard(Card);
        }
    }
}