using KomachiMod.BattleActions;
using KomachiMod.Patches;
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
        int revived = 1;
        float reviveHeal = 0.5f;
        int flawlessTurns = 2;
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DieEventArgs>(base.Owner.Dying, new GameEventHandler<DieEventArgs>(this.OnDying));
            base.ReactOwnerEvent<DieEventArgs>(base.Battle.EnemyDied, new EventSequencedReactor<DieEventArgs>(this.OnEnemyDied));
            Count = 1;
        }

        private void OnDying(DieEventArgs args)
        {
            if (revived == 1)
            {
                base.NotifyActivating();
                int num = (args.Unit.MaxHp * reviveHeal).RoundToInt();
                base.GameRun.Player.Heal(num);
                args.CancelBy(this);
                revived = 2;
                Count = 0;
                if (base.GameRun.Battle != null)
                {
                    this.React(new ApplyStatusEffectAction<Invincible>(base.Owner, 0, new int?(flawlessTurns), null, null, 0f, true));
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
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            base.NotifyActivating();
            bool isSummon = arg.Unit.HasStatusEffect<Servant>();
            if (isSummon)
            {
                yield return new CastBlockShieldAction(base.Battle.Player, new ShieldInfo(base.Level, BlockShieldType.Direct), false);
            }
            else
            {
                base.Battle.Heal(Owner, base.Level);
            }
            yield break;
        }
    }
}