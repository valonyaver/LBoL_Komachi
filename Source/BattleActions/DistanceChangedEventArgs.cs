using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System;

namespace KomachiMod.BattleActions
{
    /// <summary>
    /// Event for changing the distance of a unit. See DistanceChangeAction which changes distance and calls this event.
    /// </summary>
    public class DistanceChangedEventArgs : GameEventArgs
    {
        public Unit Unit;
        public StatusEffect Effect;
        /// <summary>
        /// Distance level before change
        /// </summary>
        public int oldLevel;
        /// <summary>
        /// Amount of attempted level change.
        /// </summary>
        public int levelChange;
        /// <summary>
        /// Distance level after change.
        /// </summary>
        public int newLevel;
        public int distanceChange
        {
            get
            {
                return newLevel - oldLevel;
            }
        }
        public int distanceChangeAbs
        {
            get
            {
                return Math.Abs(newLevel - oldLevel);
            }
        }
        protected override string GetBaseDebugString()
		{
			return $"The {Unit} has had its distance changed. Its distance went from {oldLevel} to {newLevel}";
		}
	}
}