using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using UnityEngine;

namespace KomachiMod.BattleActions
{
    public sealed class KomachiReleaseAction : SimpleEventBattleAction<KomachiReleaseEventArgs>
    {             
        internal KomachiReleaseAction(Unit unit, int amount)
		{
			Args = new KomachiReleaseEventArgs
            { 
                Unit = unit,
                releaseAmount = amount
			};
		}

        /// <summary>
        /// Automatically assigns the event args unit to the player.
        /// </summary>
        /// <param name="amount"></param>
        internal KomachiReleaseAction(Card card, int amount)
        {
            Args = new KomachiReleaseEventArgs
            {
                Unit = card.Battle.Player,
                releaseAmount = amount
            };
        }

        // In the future update it to be able to include divine spirits as well.
        protected override void MainPhase()
        {
            if (!Args.Unit.HasStatusEffect<KomachiModGuidedSpiritSe>())
            {
                Debug.LogError($"{Args.Unit.SelfName} has no guided spirits to release.");
                return;
            }
            KomachiModGuidedSpiritSe spirits = Args.Unit.GetStatusEffect<KomachiModGuidedSpiritSe>();
            if (spirits.Level < Args.releaseAmount)
            {
                Debug.LogError($"{Args.Unit.SelfName} does not have enough guided spirits for this release.");
                return;
            }
            Args.successful = true;
            int num = spirits.Level - Args.releaseAmount;
            Args.guidedSpiritReleaseAmount = Args.releaseAmount;
            spirits.Level = num;
            if (spirits.Level == 0)
            {
                var removeStatus = new RemoveStatusEffectAction(spirits, true, 0.1f);
                React(removeStatus);
                Args.removedCompletely = true;
            }

        }

        protected override void PostEventPhase()
        {
            Trigger(KomachiEventsManager.spiritsReleased);
        }
    }
}