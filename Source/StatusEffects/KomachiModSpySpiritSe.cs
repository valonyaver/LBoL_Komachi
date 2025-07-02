using KomachiMod.BattleActions;
using KomachiMod.Source.BattleActions.EventManager;
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
    public sealed class KomachiModSpySpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword) };
            config.Type = LBoL.Base.StatusEffectType.Negative;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModSpySpiritSeDef))]
    public sealed class KomachiModSpySpiritSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DistanceChangedEventArgs>
                (KomachiEventsManager.DistanceChanged, new GameEventHandler<DistanceChangedEventArgs>(this.OnEnemyDistanceChange));
        }

        private void OnEnemyDistanceChange(DistanceChangedEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                return;
            }
            if (args.Effect.GetType() == typeof(KomachiModDistanceSe) && args.Unit == Owner)
            {
                base.NotifyActivating();
                React(new CastBlockShieldAction(base.Battle.Player, 0, base.Level * args.distanceChangeAbs, BlockShieldType.Direct, false));
            }
        }
    }
}