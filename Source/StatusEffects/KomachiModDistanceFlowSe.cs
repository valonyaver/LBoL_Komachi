using KomachiMod.BattleActions;
using KomachiMod.Patches;
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
    public sealed class KomachiModDistanceFlowSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(KomachiDistanceSe) };
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModDistanceFlowSeDef))]
    public sealed class KomachiModDistanceFlowSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DistanceChangedEventArgs>
                (CustomGameEventManager.DistanceChanged, new GameEventHandler<DistanceChangedEventArgs>(this.OnEnemyDistanceChange));
        }

        private void OnEnemyDistanceChange(DistanceChangedEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                return;
            }
            if (args.Effect.GetType() == typeof(KomachiDistanceSe))
            {
                base.NotifyActivating();
                if (args.distanceChange > 0)
                {
                    React(new ApplyStatusEffectAction<TempSpirit>(base.Owner, new int?(base.Level), null, null, null, 0f, true));
                }
                else if (args.distanceChange < 0)
                {
                    React(new ApplyStatusEffectAction<TempFirepower>(base.Owner, new int?(base.Level), null, null, null, 0f, true));
                }
            }
        }
    }
}