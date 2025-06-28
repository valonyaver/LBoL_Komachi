using Cysharp.Threading.Tasks;
using KomachiMod.StatusEffects;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

namespace KomachiMod.BattleActions
{
    public class DetonateVengefulSpiritEventArgs : GameEventArgs
	{
        public Unit Target;
        public StatusEffect Effect;
		// Source card
        public Card Card { get; internal set; }
		public bool detonatedByEffect;
		/// <summary>
		/// This is false if the target has no vengeful spirits to speak of, so the action just fizzles and does nothing.
		/// </summary>
		public bool noFizzle;
		/// <summary>
		/// Total count of the vengeful spirits
		/// </summary>
		public int amountDetonated;
		public int durationAtDetonation;
		// Damage that would be dealt. Technically doesn't take into account flawless but whatev
		public int damageDealt;

		protected override string GetBaseDebugString()
		{
			return $"The target {Target.SelfName} is being detonated with spirits.";
		}
	}
}