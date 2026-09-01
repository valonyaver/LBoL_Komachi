using KomachiMod.BattleActions;
using KomachiMod.Source.BattleActions.EventManager;
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
using LBoL.EntityLib.Cards.Neutral.Red;
using LBoL.EntityLib.StatusEffects.Neutral.TwoColor;
using LBoL.EntityLib.StatusEffects.Reimu;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core.Tokens;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModSpiritInvitationSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            config.RelativeEffects = new string[]
            {
                nameof(KomachiModReleaseKeyword)
            };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModSpiritInvitationSeDef))]
    public sealed class KomachiModSpiritInvitationSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent(KomachiEventsManager.SpiritsReleased, OnReleased);
        }

        private IEnumerable<BattleAction> OnReleased(KomachiReleaseEventArgs args)
        {
            if (args.Unit == Owner && args.Successful)
            {
                NotifyActivating();
                yield return new CastBlockShieldAction(Owner, 0, Level * args.OriginalReleaseAmount, BlockShieldType.Direct);

                int num = Owner.Shield;
                string gunName = "秽火";
                if (num > 10)
                {
                    gunName = "秽火B";
                }

                if (num > 20)
                {
                    gunName = "秽火C";
                }

                yield return new DamageAction(base.Battle.Player, base.Battle.EnemyGroup.Alives, DamageInfo.Reaction(num), gunName);
            }
        }
    }
}