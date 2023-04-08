using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
	public class SupportUnitInput : IInputCommand
	{
		public bool CanExecute(InputCommandData data)
		{
			return data.StartSlot == data.EndSlot &&
			       data.StartSlot is UnitSlotDisplay && 
			       data.StartSlot.Owner == data.MyPlayerDisplay &&
			       data.Card != null && 
			       (data.StartSlot as UnitSlotDisplay).Position.Y != 0;
		}

		public IClientCommand GetCommand(InputCommandData data)
		{
			UnitSlotDisplay slot = data.StartSlot as UnitSlotDisplay;
			return new SupportCommand(slot.Position);
		}
	}
}