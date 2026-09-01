using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.Source.StatusEffects.Spirits;
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
    public sealed class KomachiModRecursiveSoulsDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Black = 1, Any = 2};
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Spirits per exile
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            config.Illustrator = "non (nobu)";

            config.RelativeEffects = new string[]
            {
                nameof(KomachiModGuidedSpiritSe)
            };
            config.UpgradedRelativeEffects = new string[]
            {
                nameof(KomachiModGuidedSpiritSe)
            };


            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModRecursiveSoulsDef))]
    public sealed class KomachiModRecursiveSouls : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModRecursiveSoulsSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}


