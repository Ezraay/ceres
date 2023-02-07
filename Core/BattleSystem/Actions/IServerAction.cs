using System.Runtime.Serialization;

namespace Ceres.Core.BattleSystem
{
    public interface IServerAction
    {
        void Apply(ClientBattle battle);
    }
}