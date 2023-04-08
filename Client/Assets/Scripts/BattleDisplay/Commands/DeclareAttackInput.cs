using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
	public class DeclareAttackInput : IInputCommand
	{
		public bool CanExecute(InputCommandData data)
		{
			return data.StartSlot is UnitSlotDisplay && 
			       data.EndSlot is UnitSlotDisplay &&
			       data.EndSlot.Owner != data.StartSlot.Owner && 
			       data.StartSlot.Owner == data.MyPlayerDisplay;
		}

		public IClientCommand GetCommand(InputCommandData data)
		{
			UnitSlotDisplay attacker = data.StartSlot as UnitSlotDisplay;
			UnitSlotDisplay target = data.EndSlot as UnitSlotDisplay;
			return new DeclareAttackCommand(
				attacker.Position, target.Owner.PlayerId, target.Position);
		}
	}
}