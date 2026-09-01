using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    /// <summary>
    /// It's actually an ability not a block card
    /// </summary>
    public sealed class KomachiModBWBridgeDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            config.Cost = new ManaGroup() { Red = 1, Black = 1, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 2, HybridColor = 7, Any = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Amount of cards to select
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            // Size of array
            config.Value2 = 3;
            config.UpgradedValue2 = 5;

            config.Mana = new ManaGroup() { White = 1 };

            config.UpgradedRelativeCards = new List<string>() { nameof(WManaCard) };

            config.Illustrator = "9時";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModBWBridgeDef))]
    public sealed class KomachiModBWBridge : KomachiCard
    {
        protected override int BaseValue3 { get => 1; }
        protected override int BaseUpgradedValue3 { get => 1; }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card[] array = base.Battle.RollCardsWithoutManaLimit(
                new CardWeightTable
                (
                    RarityWeightTable.BattleCard, //Rarity distribution of the cards. (BattleCard: 40% Common, 40% Uncommon, 20% Rare)
                    OwnerWeightTable.Valid, //Player and Neutral card pool. (Valid: Includes the Character, Act 1 Boss Exhibit and Neutral cards.) 
                    CardTypeWeightTable.CanBeLoot, //Card types. (Can Be Loot: Can be Attack, Defense, Skill, ability; Cannot be Tools).
                    false
                ),
                base.Value2, //Total amount of card to choose from.
                (CardConfig config) => config.Id != base.Id && config.Colors.Contains(ManaColor.Black) && config.Colors.Contains(ManaColor.White)
                && config.Colors.Count == 2 && config.Id != nameof(QingeUpgrade)
            );
            if (array.Length != 0)
            {
                SelectCardInteraction interactionMultiple = new SelectCardInteraction(0, base.Value1, array);
                yield return new InteractionAction(interactionMultiple);
                IReadOnlyList<Card> cards = interactionMultiple.SelectedCards;
                yield return new AddCardsToHandAction(cards);
                var exileCards = array.Where(c => !cards.Contains(c));
                yield return new AddCardsToExileAction(exileCards);
            }

            if (IsUpgraded)
            {
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<WManaCard>() });
            }
            yield return new GainManaAction(Mana);
            yield return BuffAction<KomachiModBWBridgeSe>(1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}


