using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.Source.StatusEffects.Spirits;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Core.Tokens;
using static UnityEngine.GraphicsBuffer;

namespace KomachiMod.BattleActions
{
    public sealed class DetonateVengefulSpiritAction : SimpleEventBattleAction<DetonateVengefulSpiritEventArgs>
    {             
        internal DetonateVengefulSpiritAction(Card source, Unit target)
		{
			Args = new DetonateVengefulSpiritEventArgs
            { 
                Card = source,
                Target = target
			};
		}
        /// <summary>
        /// MAKE SURE THAT IF YOU ARE CALLING THIS FROM A CARD, YOU USE THE OTHER OVERRIDE VERSION.
        /// </summary>
        /// <param name="target"></param>
        internal DetonateVengefulSpiritAction(Unit target, bool detonateByEffect = false)
        {
            Args = new DetonateVengefulSpiritEventArgs
            {
                Target = target,
                detonatedByEffect = detonateByEffect
            };
        }

        protected override void MainPhase()
        {
            KomachiModVengefulSpiritSe spirits;
            Args.Target.TryGetStatusEffect(out spirits);
            if (spirits == null)
            {
                Args.noFizzle = false;
                return;
            }
            Args.noFizzle = true;
            Args.damageDealt = spirits.damageDealtMeasure;
            Args.amountDetonated = spirits.Count;
            Args.durationAtDetonation = spirits.Duration;
            if (Args.Card != null) Args.detonatedByEffect = true;
            var removeStatus = new RemoveStatusEffectAction(spirits, true, 0.5f);
            React(removeStatus);
            Args.Effect = removeStatus.Args.Effect;
            Debug.Log($"Detonating {Args.Effect.Count} spirits to deal {Args.damageDealt} damage");
        }

        protected override void PostEventPhase()
        {
            Trigger(KomachiEventsManager.DetonatedSpirits);
        }
    }
}