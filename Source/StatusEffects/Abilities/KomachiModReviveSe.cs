using KomachiMod.BattleActions;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModReviveSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            config.LevelStackType = StackType.Add;
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModReviveSeDef))]
    public sealed class KomachiModReviveSe : StatusEffect
    {
        bool revived = false;
        float reviveHeal = 0.5f;
        int flawlessTurns = 1;

        protected override string GetBaseDescription()
        {
            if (revived) return ExtraDescription;
            return base.GetBaseDescription();
        }
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DieEventArgs>(base.Owner.Dying, new GameEventHandler<DieEventArgs>(this.OnDying));
            base.ReactOwnerEvent<DieEventArgs>(base.Battle.EnemyDied, new EventSequencedReactor<DieEventArgs>(this.OnEnemyDied), GameEventPriority.Lowest);
            Count = 1;
        }

        private void OnDying(DieEventArgs args)
        {
            if (!revived)
            {
                base.NotifyActivating();
                int num = (args.Unit.MaxHp * reviveHeal).RoundToInt();
                base.GameRun.Player.Hp = num;
                args.CancelBy(this);
                revived = true;
                Count = 0;
                if (base.GameRun.Battle != null)
                {
                    this.React(new ApplyStatusEffectAction<Invincible>(base.Owner, 0, new int?(flawlessTurns), null, null, 0f, false));
                }
                Card deckCardByInstanceId = base.GameRun.GetDeckCardByInstanceId(SourceCard.InstanceId);
                if (deckCardByInstanceId != null)
                {
                    GameRun.RemoveDeckCardByInstanceId(deckCardByInstanceId.InstanceId);
                }
            }
        }
        private IEnumerable<BattleAction> OnEnemyDied(DieEventArgs arg)
        {
            Debug.Log("Activating God of Death to heal");
            base.NotifyActivating();
            bool isSummon = arg.Unit.HasStatusEffect<Servant>();
            if (isSummon)
            {
                Debug.Log($"Getting {Level} shield.");
                yield return new CastBlockShieldAction(base.Battle.Player, new ShieldInfo(base.Level, BlockShieldType.Direct), false);
            }
            else
            {
                Debug.Log($"Getting {Level} health.");
                Owner.Heal(base.Level);
            }
            yield break;
        }
    }
}