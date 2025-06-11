using LBoL.Base;
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
    public sealed class KomachiDistanceBlockSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword) };
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiDistanceBlockSeDef))]
    public sealed class KomachiDistanceBlockSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            foreach (EnemyUnit enemyUnit in base.Battle.AllAliveEnemies)
            {
                base.ReactOwnerEvent<StatusEffectApplyEventArgs>
                    (enemyUnit.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnEnemyDistanceAdded));
                base.HandleOwnerEvent<StatusEffectEventArgs>
                    (enemyUnit.StatusEffectChanged, new GameEventHandler<StatusEffectEventArgs>(this.OnEnemyDistanceChange));
            }
            base.HandleOwnerEvent<UnitEventArgs>(base.Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(this.OnEnemySpawned));
        }
        private void OnEnemySpawned(UnitEventArgs args)
        {
            base.ReactOwnerEvent<StatusEffectApplyEventArgs>
                (args.Unit.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnEnemyDistanceAdded));
            base.HandleOwnerEvent<StatusEffectEventArgs>
                    (args.Unit.StatusEffectChanged, new GameEventHandler<StatusEffectEventArgs>(this.OnEnemyDistanceChange));
        }

        private IEnumerable<BattleAction> OnEnemyDistanceAdded(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (args.Effect.GetType() == typeof(KomachiDistanceSe))
            {
                yield return new CastBlockShieldAction(base.Battle.Player, base.Level, 0, BlockShieldType.Direct, false);
            }
            yield break;
        }

        private void OnEnemyDistanceChange(StatusEffectEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                return;
            }
            if (args.Effect.GetType() == typeof(KomachiDistanceSe))
            {
                base.NotifyActivating();
                React(new CastBlockShieldAction(base.Battle.Player, base.Level, 0, BlockShieldType.Direct, false));
            }
        }
    }
}