using System;
using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
	public class TakeDamageCommand : ClientCommand
	{
		private Card card;
		
		public override void Apply(ServerBattle battle, IPlayer author)
		{
			MultiCardSlot pile = author.GetMultiCardSlot(MultiCardSlotType.Pile) as MultiCardSlot;
			card = pile.PopCard();
			author.GetMultiCardSlot(MultiCardSlotType.Damage).AddCard(this.card);
		}

		public override ServerAction[] GetActionsForAlly(IPlayer author)
		{
			if (this.card == null)
				throw new Exception("Damage is null");
			return new ServerAction[] { new TakeDamageAction(author.Id, this.card) };
		}
	}
}