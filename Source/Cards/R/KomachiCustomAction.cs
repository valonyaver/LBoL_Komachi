using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using System.Linq;
using LBoL.Core.Battle.Interactions;
using KomachiMod.BattleActions;
using KomachiMod.StatusEffects;

namespace KomachiMod.Cards
{
    public sealed class KomachiCustomActionDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            config.Value1 = 8;

            config.Value2 = 1;
            config.UpgradedValue2 = 2;

            config.Illustrator = "";

            config.RelativeEffects = new List<string>() { nameof(KomachiEnhanceSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiEnhanceSe) };

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiCustomActionDef))]
    public sealed class KomachiCustomAction : KomachiCard
    {
        //Choose an Attack card in the hand.
        public override Interaction Precondition()
		{
			List<Card> list = base.Battle.HandZone.Where((Card card) => card != this && card.Config.Type == CardType.Attack).ToList<Card>();
			if (list.Count <= 0)
			{
				return null;
			}
			return new SelectHandInteraction(0, base.Value2, list);
		}
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            //If a card was selected. 
            if (precondition != null)
			{
				IReadOnlyList<Card> cards = ((SelectHandInteraction)precondition).SelectedCards;
				if (cards.Count > 0)
				{
                    foreach(Card card in cards)
                    {
                        yield return new BuffAttackCardAction(card, base.Value1);
                    }
                }
            }
            yield break;
        }
    }
}


