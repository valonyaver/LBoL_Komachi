using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

namespace KomachiMod.BattleActions
{
    public class KomachiReleaseEventArgs : GameEventArgs
	{
		public Unit Unit;
		public int releaseAmount;
		public int guidedSpiritReleaseAmount;
		// Implement later.
		public int divineSpiritReleaseAmount;
		public bool successful;
		public bool removedCompletely;
		protected override string GetBaseDebugString()
		{
			return $"Released {releaseAmount} guided spirits.";
		}
	}
}