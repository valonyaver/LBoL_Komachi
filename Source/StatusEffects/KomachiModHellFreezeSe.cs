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
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModHellFreezeSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(KomachiDetonationKeyword), nameof(Cold) };
            config.Type = LBoL.Base.StatusEffectType.Negative;
            config.LevelStackType = StackType.Keep;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModHellFreezeSeDef))]
    public sealed class KomachiModHellFreezeSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DetonateVengefulSpiritEventArgs>
                (KomachiEventsManager.DetonatedSpirits, new GameEventHandler<DetonateVengefulSpiritEventArgs>(this.OnEnemyDetonated));
        }

        private void OnEnemyDetonated(DetonateVengefulSpiritEventArgs args)
        {
            if (base.Battle.BattleShouldEnd || !args.noFizzle)
            {
                return;
            }
            if (args.Target == Owner)
            {
                base.NotifyActivating();
                React(new ApplyStatusEffectAction<Cold>(Owner, Level));
            }
        }
    }
}