using Cysharp.Threading.Tasks;
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
        public int Amount { get; internal set; }
		/// <summary>
		/// Increases duration of vengeful spirits by its amount.
		/// </summary>
		public int Duration;
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

		public bool successful
		{
			get => stacked || applied;
		}
		protected override string GetBaseDebugString()
		{
			return $"The target {Target.SelfName} is being applied with {Amount} vengeful spirits.";
		}
	}
}