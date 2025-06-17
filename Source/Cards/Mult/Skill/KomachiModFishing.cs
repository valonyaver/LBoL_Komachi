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
    public sealed class KomachiModFishingDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1, Red = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Discard
            config.Value1 = 1;
            // Cards targeted
            config.Value2 = 2;

            config.Mana = new ManaGroup() { Any = 0 };

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModFishingDef))]
    public sealed class KomachiModFishing : KomachiCard
    {
        public override bool CanUse
        {
            get
            {
                return Battle.HandZone.Count > Value1;
            }
        }
        // cost restriction
        protected override int BaseValue3 { get => 3; }
        protected override int BaseUpgradedValue3 { get => 4; }

        public override Interaction Precondition()
        {
            if (base.Battle.ExileZone.Count <= 0)
            {
                return null;
            }
            List<Card> list = Battle.ExileZone.Where(card => card.Cost.Amount < Value3).ToList();

            return new SelectCardInteraction(0, Value2, list, SelectedCardHandling.DoNothing);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // Discard your cards for cost BUDDY
            if (base.Battle.HandZone.Count > Value1)
            {
                SelectHandInteraction discardInteraction = new SelectHandInteraction(base.Value1, base.Value1, base.Battle.HandZone)
                {
                    Source = this
                };
                yield return new InteractionAction(discardInteraction, false);
                yield return new DiscardManyAction(discardInteraction.SelectedCards);
                discardInteraction = null;
            }
            else
            {
                yield return new DiscardManyAction(base.Battle.HandZone);
            }
            SelectCardInteraction selectBanishInteraction = (SelectCardInteraction)precondition;
            IReadOnlyList<Card> cards = (selectBanishInteraction).SelectedCards;

            foreach (Card card in cards)
            {
                yield return new MoveCardAction(card, CardZone.Hand);
                card.SetTurnCost(Mana);
            }

            yield break;
        }
    }
}


