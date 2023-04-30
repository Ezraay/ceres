#region

using System;

#endregion

namespace Ceres.Core.BattleSystem
{
	public abstract class ServerAction
	{
		public Guid AuthorId;

		public void SetAuthor(Guid authorId)
		{
			this.AuthorId = authorId;
		}

		public abstract void Apply(ClientBattle battle, IPlayer author);
	}
}