namespace Ceres.Core.BattleSystem
{
    public struct CardPosition
    {
        public int X;
        public int Y;

        public CardPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
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