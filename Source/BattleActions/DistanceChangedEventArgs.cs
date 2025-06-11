using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System;

namespace KomachiMod.BattleActions
{
    public class DistanceChangedEventArgs : GameEventArgs
    {
        public Unit Unit;
        public StatusEffect Effect;
        public int oldLevel;
        public int levelChange;
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