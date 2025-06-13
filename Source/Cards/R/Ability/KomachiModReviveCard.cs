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
    public sealed class KomachiModReviveCardDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            config.Cost = new ManaGroup() { Red = 2, Black = 2, Any = 1};
            config.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 3, HybridColor = 7 };
            config.Rarity = Rarity.Rare;
            config.FindInBattle = false;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            config.Value1 = 6;
            config.UpgradedValue1 = 10;

            config.Illustrator = "";

            config.RelativeEffects = new List<string>() { nameof(Invincible) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(Invincible) };


            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModReviveCardDef))]
    public sealed class KomachiModReviveCard : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModReviveSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}


