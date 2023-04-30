#region

using System;
using Newtonsoft.Json;

#endregion

namespace Ceres.Core.BattleSystem
{
	public class ClientBattle : Battle
	{
		[JsonConstructor]
		public ClientBattle(PhaseManager phaseManager, IPlayer player1, IPlayer player2) : base(phaseManager, player1,
			player2)
		{
		}

		public void Execute(ServerAction action)
		{
			IPlayer? author = GetPlayerById(action.AuthorId);
			if (author == null)
				throw new Exception("Author is null");
			action.Apply(this, author);
		}

		public IPlayer? GetPlayerById(Guid id)
		{
			if (id == this.Player1.Id) return this.Player1;
			if (id == this.Player2.Id) return this.Player2;
			return null;
		}
	}
}