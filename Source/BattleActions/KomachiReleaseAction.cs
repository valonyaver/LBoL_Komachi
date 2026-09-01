using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.Source.StatusEffects.Spirits;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System;
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
                ReleaseAmount = amount
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
                ReleaseAmount = amount
            };
        }

        protected override void PreEventPhase()
        {
            var player = Args.Unit;
            if (player.TryGetStatusEffect<KomachiModGuidedSpiritSe>(out var guided))
            {
                Args.IntendedGuidedReleaseAmount = Math.Min(guided.Level, Args.ReleaseAmount);
            }
            if (player.TryGetStatusEffect<KomachiModDivineSpiritSe>(out var divine))
            {
                Args.IntendedDivineReleaseAmount = Math.Min(divine.Level, Args.ReleaseAmount);
            }

            Trigger(KomachiEventsManager.SpiritsReleasing);
        }

        protected override void MainPhase()
        {
            // Check if we have any spirits at all
            if (!KomachiModUtility.CanReleaseSpirits(Args.Unit, Args.ReleaseAmount))
            {
                Debug.LogError($"{Args.Unit.SelfName} doesn't have enough total spirits to release {Args.ReleaseAmount}.");
                return;
            }

            Args.Successful = true;
            int remainingRelease = Args.ReleaseAmount;

            // First try to use Guided Spirits
            if (Args.Unit.TryGetStatusEffect<KomachiModGuidedSpiritSe>(out var guidedSpirits) && guidedSpirits.Level > 0)
            {
                int guidedToRelease = Mathf.Min(guidedSpirits.Level, remainingRelease);
                guidedSpirits.Level -= guidedToRelease;
                remainingRelease -= guidedToRelease;
                Args.GuidedSpiritReleaseAmount = guidedToRelease;

                if (guidedSpirits.Level == 0)
                {
                    var removeStatus = new RemoveStatusEffectAction(guidedSpirits, true, 0.1f);
                    React(removeStatus);
                }
            }

            // If still need more, use Divine Spirits
            if (remainingRelease > 0 && Args.Unit.TryGetStatusEffect<KomachiModDivineSpiritSe>(out var divineSpirits))
            {
                int divineToRelease = Mathf.Min(divineSpirits.Level, remainingRelease);
                divineSpirits.Level -= divineToRelease;
                Args.DivineSpiritReleaseAmount = divineToRelease;

                if (divineSpirits.Level == 0)
                {
                    var removeStatus = new RemoveStatusEffectAction(divineSpirits, true, 0.1f);
                    React(removeStatus);
                }
            }
            // If need be, split removed completely into two separate components later.
            Args.RemovedCompletely = (Args.GuidedSpiritReleaseAmount == Args.ReleaseAmount) ||
                                   (Args.DivineSpiritReleaseAmount == Args.ReleaseAmount);
        }

        protected override void PostEventPhase()
        {
            Trigger(KomachiEventsManager.SpiritsReleased);
        }
    }
}