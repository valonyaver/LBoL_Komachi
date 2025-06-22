using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.StatusEffects;
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
    public sealed class ApplyVengefulSpiritAction : SimpleEventBattleAction<ApplyVengefulSpiritEventArgs>
    {             
        internal ApplyVengefulSpiritAction(Card source, Unit target, int amount)
		{
			Args = new ApplyVengefulSpiritEventArgs
            { 
                Card = source,
                Target = target,
                Amount = amount
			};
		}
        internal ApplyVengefulSpiritAction(StatusEffect source, Unit target, int amount)
        {
            Args = new ApplyVengefulSpiritEventArgs
            {
                statusEffect = source,
                Target = target,
                Amount = amount
            };
        }
        internal ApplyVengefulSpiritAction(Unit target, int amount)
        {
            Args = new ApplyVengefulSpiritEventArgs
            {
                Target = target,
                Amount = amount
            };
        }

        protected override void MainPhase()
        {
            KomachiModVengefulSpiritSe spirits;
            Args.Target.TryGetStatusEffect(out spirits);
            if (spirits == null)
            {
                Args.oldAmount = 0;
                Args.applying = true;
            }
            else Args.Amount = spirits.Count;
            var applyStatus = new ApplyStatusEffectAction<KomachiModVengefulSpiritSe>(Args.Target, count: Args.Amount, duration: 3, startAutoDecreasing: true);
            React(applyStatus);
            Args.Effect = applyStatus.Args.Effect;
            
        }

        protected override void PostEventPhase()
        {
            if (Args.Target.HasStatusEffect<KomachiModVengefulSpiritSe>())
            {
                if (Args.applying) Args.applied = true;
                else Args.stacked = true;
            }
            Trigger(KomachiEventsManager.AppliedVengefulSpirit);
        }
    }
}