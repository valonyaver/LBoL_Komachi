using KomachiMod.BattleActions;
using KomachiMod.Source.BattleActions.EventManager;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModEikiSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            config.LevelStackType = StackType.Keep;
            config.RelativeEffects = new List<string>()
            {
                nameof(KomachiModReleaseKeyword)
            };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModEikiSeDef))]
    public sealed class KomachiModEikiSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<KomachiReleaseEventArgs>
                (KomachiEventsManager.SpiritsReleasing, new EventSequencedReactor<KomachiReleaseEventArgs>(this.OnSpiritsReleasing));
        }

        private IEnumerable<BattleAction> OnSpiritsReleasing(KomachiReleaseEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();
                args.ReleaseAmount = 0;
                args.AddModifier(this);
            }
            yield break;
        }
    }
}