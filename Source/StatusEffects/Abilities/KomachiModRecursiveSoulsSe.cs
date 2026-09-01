using KomachiMod.Source.StatusEffects.Spirits;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.StatusEffects.Reimu;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core.Tokens;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModRecursiveSoulsSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            config.RelativeEffects = new string[]
            {
                nameof(KomachiModGuidedSpiritSe)
            };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModRecursiveSoulsSeDef))]
    public sealed class KomachiModRecursiveSoulsSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent(Battle.CardExiled, OnCardExiled);
        }

        private IEnumerable<BattleAction> OnCardExiled(CardEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();
                yield return new ApplyStatusEffectAction<KomachiModGuidedSpiritSe>(Owner, Level);
            }
        }
    }
}