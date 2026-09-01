using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

namespace KomachiMod.BattleActions
{
    public class KomachiReleaseEventArgs : GameEventArgs
	{
        /// <summary>
        /// Player releasing the spirits
        /// </summary>
		public Unit Unit;
		public int OriginalReleaseAmount;
		public int ReleaseAmount;

        /// <summary>
        /// The virtual amount of Guided Spirits that would be released (ignores Eiki's free release)
        /// </summary>
        public int IntendedGuidedReleaseAmount;
        /// <summary>
        /// The virtual amount of Divine Spirits that would be released (ignores Eiki's free release)
        /// </summary>
        public int IntendedDivineReleaseAmount;
        /// <summary>
        /// How many Guided spirits were released by this command
        /// </summary>
        public int GuidedSpiritReleaseAmount;
        /// <summary>
        /// How many Guided spirits were released by this command
        /// </summary>
        public int DivineSpiritReleaseAmount;
		public bool Successful;
		public bool RemovedCompletely;
		protected override string GetBaseDebugString()
		{
			return $"Released {ReleaseAmount} guided spirits.";
		}
	}
}