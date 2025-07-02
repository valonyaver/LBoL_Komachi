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
    public sealed class KomachiModAfterlifePassageDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1};
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Exile amount
            config.Value1 = 2;
            config.UpgradedValue1 = 3;

            // Spirits gain per exile
            config.Value2 = 4;
            config.UpgradedValue2 = 6;

            config.Illustrator = "きつね丸";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);


            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.RelativeKeyword = Keyword.Basic;
            config.UpgradedRelativeKeyword = Keyword.Basic;
            config.RelativeEffects = new List<string>() { nameof(KomachiModGuidedSpiritSe), nameof(KomachiModDivineSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModGuidedSpiritSe), nameof(KomachiModDivineSpiritSe) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModAfterlifePassageDef))]
    public sealed class KomachiModAfterlifePassage : KomachiCard
    {
        // Code stolen from purify the land lol
        public override Interaction Precondition()
        {
            List<Card> hand = base.Battle.HandZone.Where((Card card) => card != this).ToList<Card>();
            if (!hand.Empty<Card>())
            {
                int minAmount = Value1;
                if (IsUpgraded)
                {
                    minAmount = 0;
                }
                return new SelectHandInteraction(minAmount, base.Value1, hand);
            }
            return null;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                IReadOnlyList<Card> cards = ((SelectHandInteraction)precondition).SelectedCards;
                if (cards.Count > 0)
                {
                    yield return new ExileManyCardAction(cards);
                }
                foreach (var card in cards)
                {
                    if (card.CardType == CardType.Status || card.CardType == CardType.Misfortune || card.HasKeyword(Keyword.Basic))
                    {
                        yield return BuffAction<KomachiModGuidedSpiritSe>(Value2);
                    }
                    else
                    {
                        yield return BuffAction<KomachiModDivineSpiritSe>(Value2);
                    }
                }
            }
            yield break;
        }
    }
}


