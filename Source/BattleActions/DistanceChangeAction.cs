using KomachiMod.Patches;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace KomachiMod.BattleActions
{
    public sealed class DistanceChangeAction : SimpleEventBattleAction<DistanceChangedEventArgs>
    {             
        internal DistanceChangeAction(Unit target, int distanceChange)
		{
			Args = new DistanceChangedEventArgs
            { 
                Unit = target,
                levelChange = distanceChange
			};
		}

        protected override void MainPhase()
        {
            if (Args.levelChange == 0) return;
            KomachiDistanceSe distance;
            Args.Unit.TryGetStatusEffect(out distance);
            if (distance == null)
            {
                // If no distance, assume it's three so that level is set to 3 + levelChange.
                SetDistanceLevel(Args.Unit, 3 + Args.levelChange);
            }
            else
            {
                Debug.Log($"Distance level before checking whether to change level or not is {distance.Level}. Gon be changing it by {Args.levelChange}");
                // Set distance level clamps the target level and checks before notification.
                // If for example the current level is 1 and level change is -1,
                // Then the target level will be clamped to 1. It will equal the current level and won't notify.
                SetDistanceLevel(Args.Unit, distance.Level + Args.levelChange);
            }
        }

        public void SetDistanceLevel(Unit target, int targetLevel)
        {
            KomachiDistanceSe distance;
            target.TryGetStatusEffect(out distance);
            Args.Effect = distance;
            // If first time applying status.
            if (distance == null)
            {
                // Clamps target level.
                if (targetLevel < 1) targetLevel = 1;
                if (targetLevel > 5) targetLevel = 5;
                Args.oldLevel = 3;
                Args.newLevel = targetLevel;
                var ApplyDistanceAction = new ApplyStatusEffectAction<KomachiDistanceSe>(target, targetLevel, startAutoDecreasing: false);
                Args.Effect = ApplyDistanceAction.Args.Effect;
                React(ApplyDistanceAction);
                return;
            }
            // Otherwise, clamp the target level and set status level.
            if (targetLevel < distance.minDistance)
            {
                targetLevel = distance.minDistance;
                Debug.LogWarning($"Trying to set distance on the target. " +
                    $"But the target level {targetLevel} is lower than the minimum allowed distance of {distance.minDistance}.");
            }
            else if (targetLevel > distance.maxDistance)
            {
                targetLevel = distance.minDistance;
                Debug.LogWarning($"Trying to set distance on the target. " +
                                    $"But the target level {targetLevel} is higher than the maximum allowed distance of {distance.maxDistance}.");
            }
            Args.oldLevel = distance._level;
            Args.newLevel = targetLevel;
            // Only change and notify if it actually changed status.
            if (Args.oldLevel != Args.newLevel)
            {
                distance._level = targetLevel;
                distance.NotifyChanged();
            }
            return;
        }

        protected override void PostEventPhase()
        {
            if (Args.distanceChangeAbs > 0)
            {
                Trigger(CustomGameEventManager.DistanceChanged);
            }
        }
    }
}