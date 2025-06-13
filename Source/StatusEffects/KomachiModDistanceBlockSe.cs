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
    public sealed class KomachiModDistanceBlockSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(KomachiDistanceSe) };
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModDistanceBlockSeDef))]
    public sealed class KomachiModDistanceBlockSe : StatusEffect
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
                React(new CastBlockShieldAction(base.Battle.Player, base.Level * args.distanceChangeAbs, 0, BlockShieldType.Direct, false));
            }
        }
    }
}