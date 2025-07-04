using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    /// <summary>
    /// It's actually an ability not a block card
    /// </summary>
    public sealed class KomachiModSpiritDefenceDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();

            // config.ImageId = nameof(KomachiModBlockB);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2 };
            config.UpgradedCost = new ManaGroup() { Black = 1, Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.Nobody;

            config.Block = 12;
            config.UpgradedBlock = 15;

            config.Value1 = 4;
            config.UpgradedValue1 = 6;

            config.RelativeEffects = new List<string>() { nameof(KomachiModGuidedSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModGuidedSpiritSe) };

            config.Illustrator = "delant";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSpiritDefenceDef))]
    public sealed class KomachiModSpiritDefence : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return BuffAction<KomachiModGuidedSpiritSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}


