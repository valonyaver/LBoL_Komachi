using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

namespace KomachiMod.BattleActions
{
    public class ApplyVengefulSpiritEventArgs : GameEventArgs
	{
        public Unit Target;
        public StatusEffect Effect;
        public Card Card { get; internal set; }
		public StatusEffect statusEffect;
        public int Amount { get; internal set; }
		public int oldAmount;
		/// <summary>
		/// First time applying effect. As target doesn't have it.
		/// </summary>
		public bool applying;
		/// <summary>
		/// Applying the effect was successful.
		/// </summary>
		public bool applied;
		/// <summary>
		/// The target already had spirits. So its count was increased.
		/// </summary>
		public bool stacked;
		protected override string GetBaseDebugString()
		{
			return $"The target {Target.SelfName} is being applied with {Amount} vengeful spirits.";
		}
	}
}