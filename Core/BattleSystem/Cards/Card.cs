using System;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
	public class Card
	{
		public ICardData Data { get; }
		public Guid ID { get; }
		public int Attack => this.Data.Attack + this.temporaryAttack;
		private int temporaryAttack;

		public Card(ICardData data) : this(data, Guid.NewGuid()) { }

		[JsonConstructor]
		public Card(ICardData data, Guid id)
		{
			this.Data = data;
			this.ID = id;
		}

		

		static public Card TestCard()
		{
			return new Card(new CardData("archer", "Archer", 1, 5, 5));
		}

		public void Reset()
		{
			this.temporaryAttack = 0;
		}

		public void AddAttack(int boost)
		{
			this.temporaryAttack += boost;
		}
	}
}