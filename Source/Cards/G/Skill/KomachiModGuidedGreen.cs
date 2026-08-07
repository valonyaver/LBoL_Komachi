using KomachiMod.Cards.Template;
using KomachiMod.Source.StatusEffects.Spirits;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    /// <summary>
    /// It's actually an ability not a block card
    /// </summary>
    public sealed class KomachiModGuidedGreenDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Green };
            config.Cost = new ManaGroup() { Green = 1, Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            config.Value1 = 12;
            config.UpgradedValue1 = 14;

            config.Value2 = 1;
            config.UpgradedValue2 = 2;

            config.RelativeEffects = new List<string>() { nameof(KomachiModGuidedSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModGuidedSpiritSe) };

            config.Illustrator = "しょーこ";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModGuidedGreenDef))]
    public sealed class KomachiModGuidedGreen : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModGuidedSpiritSe>(base.Value1, 0, 0, 0, 0.2f);
            yield return new DrawManyCardAction(Value2);
            yield break;
        }
    }
}


