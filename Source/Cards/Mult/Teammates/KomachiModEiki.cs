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
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards.B
{
    public sealed class KomachiModEikiDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();  

            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1, White = 1, Hybrid = 1, HybridColor = 1, Any = 1 };

            config.Rarity = Rarity.Rare;

            config.Type = CardType.Friend;
            config.TargetType = TargetType.Nobody;

            //Loyalty is called "Unity" ingame.
            config.Loyalty = 4;
            config.UpgradedLoyalty = 4;
            //Passive cost is the passive amount of Unity gained/consumed at the strt of each turn.  
            config.PassiveCost = 1;
            config.UpgradedPassiveCost = 2;
            //Cost of the Active ability. 
            config.ActiveCost = -4;
            //Cost of the Ultimate ability.
            config.UltimateCost = -8;

            // Divine Spirits gain 
            config.Shield = 10;
            config.UpgradedShield = 14;

            // Amount of cards chosen
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            // Guided Spirits if junk card
            config.Value2 = 4;

            config.Mana = new ManaGroup() { Any = 0 };

            config.RelativeKeyword = Keyword.TempMorph;
            config.UpgradedRelativeKeyword = Keyword.TempMorph;

            config.RelativeEffects = new List<string>()
            {
                nameof(KomachiModDivineSpiritSe),
                nameof(KomachiModGuidedSpiritSe),
                nameof(KomachiModReleaseKeyword)
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(KomachiModDivineSpiritSe),
                nameof(KomachiModGuidedSpiritSe),
                nameof(KomachiModReleaseKeyword)
            };


            config.Illustrator = "いびつ";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModEikiDef))]
    public sealed class KomachiModEiki : KomachiCard
    {
        public string Indent {get;} = "<indent=80>";
        public string PassiveCostIcon
        {
            get
            {
                return string.Format("<indent=0><sprite=\"Passive\" name=\"{0}\">{1}", base.PassiveCost, Indent);
            }
        }
        public string ActiveCostIcon
        {
            get
            {
                return string.Format("<indent=0><sprite=\"Active\" name=\"{0}\">{1}", base.ActiveCost, Indent);
            }
        }
        public string UltimateCostIcon
        {
            get
            {
                return string.Format("<indent=0><sprite=\"Ultimate\" name=\"{0}\">{1}", base.UltimateCost, Indent);
            }
        }
        // Recurred exile cards.
        protected override int BaseValue3 { get => 2; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 3; set => base.BaseUpgradedValue3 = value; }

        //Effect to trigger at the start of the end.
        public override IEnumerable<BattleAction> OnTurnStartedInHand()
		{
			return this.GetPassiveActions();
		}

        public override IEnumerable<BattleAction> GetPassiveActions()
		{
            //Triigger the effect only if the card has been summoned. 
			if (!base.Summoned || base.Battle.BattleShouldEnd)
			{
				yield break;
			}
			base.NotifyActivating();
            //Increase base loyalty.
			base.Loyalty += base.PassiveCost;

            List<Card> list = base.Battle.HandZone.Where
                ((Card card) => card.Cost.Amount > 0 || card.CardType == CardType.Misfortune || card.CardType == CardType.Status).ToList<Card>();
            if (list.Count < 1) yield break;
            //Trigger the action multiple times if "Mental Energy Injection" is active.
            for (int i = 0; i < base.Battle.FriendPassiveTimes + Value1 - 1; i++) // pffset value 1
			{
				if (base.Battle.BattleShouldEnd)
				{
					yield break;
				}
                // Pick a random card from the hand
                Card card = list.Sample(GameRun.BattleRng);
                if (card.CardType == CardType.Misfortune || card.CardType == CardType.Status)
                {
                    yield return new ExileCardAction(card);
                    yield return BuffAction<KomachiModGuidedSpiritSe>(Value2);
                    list.Remove(card);
                }
                else
                {
                    yield return BuffAction<KomachiModGuidedSpiritSe>(card.Cost.Amount);
                    ManaColor[] array = card.Cost.EnumerateComponents().SampleManyOrAll(1, base.GameRun.BattleRng);
                    card.DecreaseTurnCost(ManaGroup.FromComponents(array));
                }
            }
			yield break;
		}

        //Action to perform when the teammate card is summoned.
        protected override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
            yield return BuffAction<KomachiModDivineSpiritSe>(Shield.Shield);
            foreach (BattleAction battleAction in base.SummonActions(selector, consumingMana, precondition))
			{
				yield return battleAction;
			}

            yield break;
		}

        //When the summoned card is played, choose and resolve either the active or ultimate effect.
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
            //
			if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
			{
                //Adjust the card's loyalty. 
                //Because the ActiveCost is negative, the Cost has to be added instead of substracted.
				base.Loyalty += base.ActiveCost;
                List<Card> list = base.Battle.HandZone.Where((Card hand) => hand != this).ToList<Card>();
                if (list.Count <= 0)
                {
                    yield break;
                }
                var interaction = new MiniSelectCardInteraction(list);
                yield return new InteractionAction(interaction);
                Card card = interaction.SelectedCard;
                ManaGroup newCost;
                if (IsUpgraded) newCost = Mana;
                else newCost = new ManaGroup() { Any = card.Cost.Any };
                card.SetTurnCost(newCost);
                if (card.CanUpgradeAndPositive)
                {
                    yield return new UpgradeCardAction(card);
                }
                yield return base.SkillAnime;
			}
            // Ultimate ability.
			else
			{
				base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                List<Card> exileZone = Battle.ExileZone.ToList();
                var interaction = new SelectCardInteraction(0, Value3, exileZone);
                yield return new InteractionAction(interaction);
                foreach(var card in interaction.SelectedCards)
                {
                    yield return new MoveCardAction(card, CardZone.Hand);
                    card.SetBaseCost(Mana);
                }
                yield return BuffAction<KomachiModEikiSe>(1);
                yield return base.SkillAnime;
			}
			yield break;
		}
    }
}


