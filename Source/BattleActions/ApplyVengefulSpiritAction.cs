using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Core;
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
        internal ApplyVengefulSpiritAction(GameEntity source, Unit target, int amount, int duration = 0)
        {
            Args = new ApplyVengefulSpiritEventArgs
            {
                Target = target,
                Amount = amount,
                ActionSource = source,
                Duration = duration
            };
        }

        internal ApplyVengefulSpiritAction(Unit target, int amount, int duration = 0)
        {
            Args = new ApplyVengefulSpiritEventArgs
            {
                Target = target,
                Amount = amount,
                Duration = duration
            };
        }

        protected override void MainPhase()
        {
            if (Args.Target.TryGetStatusEffect<KomachiModVengefulSpiritSe>(out var spirits))
            {
                Args.oldAmount = spirits.Count;
            }
            else
            {
                Args.oldAmount = 0;
                Args.applying = true;
            }
            Debug.Log($"Applying {Args.Amount} spirits");
            var applyStatus = new ApplyStatusEffectAction<KomachiModVengefulSpiritSe>
                (Args.Target, count: Args.Amount, duration: 3, startAutoDecreasing: true, occupationTime: 0.5f);
            React(applyStatus);
        }

        protected override void PostEventPhase()
        {
            if (Args.Target.TryGetStatusEffect<KomachiModVengefulSpiritSe>(out var spirits))
            {
                Args.Effect = spirits;
                if (Args.applying) Args.applied = true;
                else Args.stacked = true;
                if (Args.Duration > 0)
                {
                    Debug.Log($"Increase duration of spirits by {Args.Duration}");
                    spirits.Duration += Args.Duration;
                    if (spirits.Duration > 1)
                    {
                        Args.Effect.Highlight = false;
                    }
                }
            }
            Trigger(KomachiEventsManager.AppliedVengefulSpirit);
        }
    }
}