using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModOasisDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };

            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Cost limit
            config.Value1 = 2;
            config.UpgradedValue1 = 3;

            config.Mana = new ManaGroup() { Any = 0 };

            config.Illustrator = "kotobuki ryou";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.RelativeKeyword = Keyword.TempRetain | Keyword.Copy;
            config.UpgradedRelativeKeyword = Keyword.TempRetain | Keyword.Copy;


            config.Unfinished = false;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModOasisDef))]
    public sealed class KomachiModOasis : KomachiCard
    {

        public override Interaction Precondition()
        {
            if (base.Battle.ExileZone.Count <= 0)
            {
                return null;
            }
            List<Card> list = Battle.ExileZone.Concat(Battle.DiscardZone).Where(card => card.Cost.Amount <= Value1 && !card.HasKeyword(Keyword.Copy)).ToList();

            return new SelectCardInteraction(1, 1, list, SelectedCardHandling.DoNothing);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // Get the cards in hand.
            SelectCardInteraction selectBanishInteraction = (SelectCardInteraction)precondition;
            IReadOnlyList<Card> cards = (selectBanishInteraction).SelectedCards;

            // Validate card count
            if (cards.Count > 0)
            {
                var card = cards[0];
                var copy = card.CloneBattleCard();
                yield return new AddCardsToHandAction(copy);
                copy.IsTempRetain = true;
                if (card.CardType == CardType.Ability || card.HasKeyword(Keyword.Exile))
                {
                    card.SetKeyword(Keyword.Copy, true);
                }
            }

            yield break;
        }
    }
}


