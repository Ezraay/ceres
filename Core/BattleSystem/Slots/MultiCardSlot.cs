#region

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace Ceres.Core.BattleSystem
{
	public class MultiCardSlot : Slot, IMultiCardSlot
	{
		public List<Card> Cards;

		public MultiCardSlot()
		{
			this.Cards = new List<Card>();
		}

		[JsonIgnore] public int Count => this.Cards.Count;

		public void AddCard(Card card)
		{
			this.Cards.Add(card);
			OnAdd?.Invoke(card);
		}

		public Card? GetCard(Guid id)
		{
			foreach (Card card in this.Cards)
				if (card.ID == id)
					return card;

			return null;
		}

		public void RemoveCard(Card card)
		{
			this.Cards.Remove(card);
			OnRemove?.Invoke(card);
		}

		public event Action<Card>? OnAdd;
		public event Action<Card>? OnRemove;

		public void Clear()
		{
			for (int i = this.Cards.Count - 1; i >= 0; i--) RemoveCard(this.Cards[i]);
		}

		public bool Contains(Card card)
		{
			return this.Cards.Contains(card);
		}

		public Card PopCard()
		{
			if (this.Cards.Count == 0)
				throw new Exception("No cards left to pop");

			Card card = this.Cards[0];
			RemoveCard(card);
			return card;
		}

		public Card PopRandomCard()
		{
			if (this.Cards.Count == 0)
				throw new Exception("No cards left to random pop");

			Random random = new Random();
			int index = random.Next(0, this.Cards.Count);
			Card card = this.Cards[index];
			RemoveCard(card);
			return card;
		}

		public void Shuffle()
		{
			List<Card> newCards = new List<Card>();
			Random random = new Random();
			for (int i = this.Cards.Count - 1; i >= 0; i--)
			{
				int index = random.Next(0, newCards.Count);
				newCards.Insert(index, this.Cards[i]);
				this.Cards.RemoveAt(i);
			}

			this.Cards = newCards;
		}
	}
}