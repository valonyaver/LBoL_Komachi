using KomachiMod.Cards.Template;
using KomachiMod.Source.StatusEffects.Spirits;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModSpiritInvitationDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 1, Any = 1};
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Self;

            // Guided Spirit gain
            config.Value1 = 4;
            config.UpgradedValue1 = 6;

            // Divine Spirit Gain
            config.Value2 = 4;
            config.UpgradedValue2 = 6;

            config.Illustrator = "luke (kyeftss)";

            config.RelativeEffects = new string[]
            {
                nameof(KomachiModGuidedSpiritSe),
                nameof(KomachiModDivineSpiritSe),
                nameof(KomachiModReleaseKeyword)
            };

            config.RelativeKeyword = Keyword.Shield;
            config.UpgradedRelativeKeyword = Keyword.Shield;

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSpiritInvitationDef))]
    public sealed class KomachiModSpiritInvitation : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModGuidedSpiritSe>(Value1);
            yield return BuffAction<KomachiModDivineSpiritSe>(Value2);

            yield return BuffAction<KomachiModSpiritInvitation>(1, 0, 0, 0, 0.2f);

        }
    }
}


