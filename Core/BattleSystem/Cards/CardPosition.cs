#region

using System;

#endregion

namespace Ceres.Core.BattleSystem
{
	[Serializable]
	public struct CardPosition
	{
		public int X;
		public int Y;

		public CardPosition(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public bool Equals(CardPosition other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		public override bool Equals(object? obj)
		{
			return obj is CardPosition other && this.Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (this.X * 397) ^ this.Y;
			}
		}


		public static bool operator ==(CardPosition c1, CardPosition c2)
		{
			return c1.Equals(c2);
		}

		public static bool operator !=(CardPosition c1, CardPosition c2)
		{
			return !c1.Equals(c2);
		}
	}
}