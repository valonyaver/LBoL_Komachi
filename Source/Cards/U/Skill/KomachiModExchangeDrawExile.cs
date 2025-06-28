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
            config.Value2 = 0;
            config.UpgradedValue2 = 2;

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModExchangeDrawExileDef))]
    public sealed class KomachiModExchangeDrawExile : KomachiCard
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Battle.DrawZone.Count == 0) yield break;
            Card topCard = Battle.DrawZone.FirstOrDefault();
            MiniSelectCardInteraction topcardInteraction = new MiniSelectCardInteraction(new List<Card>() { topCard }) { Source = this };
            yield return new InteractionAction(topcardInteraction);
            yield return new MoveCardAction(topCard, CardZone.Exile);
            List<Card> list = Battle.ExileZone.Where(card => card.Cost.Amount <= topCard.Cost.Amount && card.CardType == topCard.CardType && card != topCard).ToList();
            if (list.Count > 0)
            {
                MiniSelectCardInteraction selectBanishInteraction = new MiniSelectCardInteraction(list, canSkip: true) { Source = this };
                yield return new InteractionAction(selectBanishInteraction);
                if (selectBanishInteraction.SelectedCard != null)
                {
                    yield return new MoveCardAction(selectBanishInteraction.SelectedCard, CardZone.Hand);
                }
            }
            else if (IsUpgraded)
            {
                yield return new DrawManyCardAction(Value2);
            }
            yield break;
        }
    }
}


