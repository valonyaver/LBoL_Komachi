using KomachiMod.BattleActions;
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
    public sealed class KomachiModSpiritReturnDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2, Any = 2 };
            config.UpgradedCost = new ManaGroup() { Black = 2, Any = 1};
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Immediate spirits inflicted
            config.Value1 = 1;

            config.Mana = new ManaGroup() { Any = 1 };

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSpiritReturnDef))]
    public sealed class KomachiModSpiritReturn : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModSpiritReturnSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}


