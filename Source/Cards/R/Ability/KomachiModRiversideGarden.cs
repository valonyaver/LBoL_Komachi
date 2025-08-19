using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModRiversideGardenDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 3, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Red = 2, Any = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Spider lilies added to each zone
            config.Value1 = 1;

            // Heal from lilies
            config.Value2 = 2;
            config.UpgradedValue2 = 3;

            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };

            config.Keywords = Keyword.Battlefield;
            config.UpgradedKeywords = Keyword.Initial | Keyword.Battlefield;

            config.Illustrator = "BigRed";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModRiversideGardenDef))]
    public sealed class KomachiModRiversideGarden : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModRiversideGardenSe>(base.Value2, 0, 0, 0, 0.2f);
            yield return new AddCardsToHandAction(Library.CreateCards<KomachiModSpiderLily>(Value1, false));
            //Add a token card to the draw pile in a random position.
            yield return new AddCardsToDrawZoneAction(Library.CreateCards<KomachiModSpiderLily>(base.Value1, false), DrawZoneTarget.Random, AddCardsType.Normal);
            //Add a token card to the discard pile.
            yield return new AddCardsToDiscardAction(Library.CreateCards<KomachiModSpiderLily>(base.Value1, false), AddCardsType.Normal);
            yield break; 
        }
    }
}


