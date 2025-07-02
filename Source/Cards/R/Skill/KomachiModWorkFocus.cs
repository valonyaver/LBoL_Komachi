using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoL.Presentation.UI.Panels;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModWorkFocusDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1};
            config.UpgradedCost = new ManaGroup() { Red = 1};
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Amount of displacement
            config.Value1 = 2;

            config.Illustrator = "白露/浅葱";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);


            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance), nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModManDistance), nameof(KomachiModSpiderLily) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModWorkFocusDef))]
    public sealed class KomachiModWorkFocus : KomachiCard
    {
        // Code stolen from purify the land lol
        public override Interaction Precondition()
        {
            if (this.IsUpgraded)
            {
                List<Card> list = (from card in base.Battle.HandZone.Concat(base.Battle.DrawZoneToShow).Concat(base.Battle.DiscardZone)
                                   where card != this
                                   select card).ToList<Card>();
                if (!list.Empty<Card>())
                {
                    return new SelectCardInteraction(0, base.Value1, list, SelectedCardHandling.DoNothing);
                }
                return null;
            }
            else
            {
                List<Card> list2 = base.Battle.HandZone.Where((Card card) => card != this).ToList<Card>();
                if (!list2.Empty<Card>())
                {
                    return new SelectHandInteraction(0, base.Value1, list2);
                }
                return null;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                IReadOnlyList<Card> cards = (this.IsUpgraded ? ((SelectCardInteraction)precondition).SelectedCards : ((SelectHandInteraction)precondition).SelectedCards);
                if (cards.Count > 0)
                {
                    yield return new ExileManyCardAction(cards);
                    yield return new AddCardsToHandAction(Library.CreateCards<KomachiModManDistance>(cards.Count, false), AddCardsType.Normal);
                }
                foreach (var card in cards)
                {
                    if (card.IsUpgraded)
                    {
                        yield return new AddCardsToHandAction(Library.CreateCards<KomachiModSpiderLily>(1, false), AddCardsType.Normal);
                        break;
                    }
                }
            }
            yield break;
        }
    }
}


