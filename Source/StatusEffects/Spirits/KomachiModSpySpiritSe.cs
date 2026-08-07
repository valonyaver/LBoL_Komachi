using KomachiMod.BattleActions;
using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.StatusEffects;
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

namespace KomachiMod.Source.StatusEffects.Spirits
{
    public sealed class KomachiModSpySpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword) };
            config.Type = StatusEffectType.Negative;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModSpySpiritSeDef))]
    public sealed class KomachiModSpySpiritSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            HandleOwnerEvent
                (KomachiEventsManager.DistanceChanged, new GameEventHandler<DistanceChangedEventArgs>(OnEnemyDistanceChange));
        }

        private void OnEnemyDistanceChange(DistanceChangedEventArgs args)
        {
            if (Battle.BattleShouldEnd)
            {
                return;
            }
            if (args.Effect.GetType() == typeof(KomachiModDistanceSe) && args.Unit == Owner)
            {
                NotifyActivating();
                React(new CastBlockShieldAction(Battle.Player, 0, Level * args.distanceChangeAbs, BlockShieldType.Direct, false));
            }
        }
    }
}