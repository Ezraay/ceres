namespace Ceres.Core.BattleSystem
{
	public class CardData : ICardData
	{
		public CardData(string id, string title, string subtitle, int tier, int attack, int defense)
		{
			this.ID = id;
			this.Title = title;
			this.Subtitle = subtitle;
			this.Tier = tier;
			this.Attack = attack;
			this.Defense = defense;
		}

		public string ID { get; }
		public string Title { get; }
		public string Subtitle { get; }
		public int Tier { get; }
		public int Attack { get; }
		public int Defense { get; }

		public override string ToString()
		{
			return this.ID;
		}
	}
}