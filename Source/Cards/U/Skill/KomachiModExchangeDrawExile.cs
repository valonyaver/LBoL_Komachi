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
    public sealed class KomachiModExchangeDrawExileDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Blue};
            config.Cost = new ManaGroup() { Blue = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;


            // Exiled cards added to hand
            config.Value1 = 1;

            // draw if fail
            config.Value2 = 2;
            config.UpgradedValue2 = 3;

            config.Illustrator = "Akyuun";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            // config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModExchangeDrawExileDef))]
    public sealed class KomachiModExchangeDrawExile : KomachiCard
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // If the draw zone is empty get wreckt lmao
            if (Battle.DrawZone.Count == 0) yield break;
            // Check the top card
            Card topCard = Battle.DrawZone.FirstOrDefault();
            // Interaction that does nothing but show you what the top card is.
            MiniSelectCardInteraction topcardInteraction = new MiniSelectCardInteraction(new List<Card>() { topCard }) { Source = this };
            yield return new InteractionAction(topcardInteraction);
            yield return new MoveCardAction(topCard, CardZone.Exile);
            // Get the list of exiled cards that are of the same card type
            List<Card> list = Battle.ExileZone.Where(card => card.CardType == topCard.CardType && card != topCard).ToList();
            if (list.Count > 0)
            {
                // Select them.
                SelectCardInteraction selectBanishInteraction = new SelectCardInteraction(0, 1, list) { Source = this };
                yield return new InteractionAction(selectBanishInteraction);
                // Get the card if picked
                if (selectBanishInteraction.SelectedCards.Count > 0)
                {
                    yield return new MoveCardAction(selectBanishInteraction.SelectedCards[0], CardZone.Hand);
                }
            }
            else // If no card in banish. Vanish.
            {
                yield return new DrawManyCardAction(Value2);
            }
            yield break;
        }
    }
}


