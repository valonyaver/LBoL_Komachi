using KomachiMod.BattleActions;
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
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    /// <summary>
    /// It's actually an ability not a block card
    /// </summary>
    public sealed class KomachiModBoomBenefitsDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Guided spirits
            config.Value1 = 2;
            config.UpgradedValue1 = 1;

            // Draw
            config.Value2 = 3;
            config.UpgradedValue2 = 2;

            config.RelativeEffects = new List<string>() 
            { nameof(KomachiDetonationKeyword), nameof(KomachiModGuidingSpiritSe), nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiDetonationKeyword), nameof(KomachiModGuidingSpiritSe), nameof(KomachiModVengefulSpiritSe) };

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModBoomBenefitsDef))]
    public sealed class KomachiModBoomBenefits : KomachiCard
    {
        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModBoomBenefits guide = Library.CreateCard<KomachiModBoomBenefits>();
            KomachiModBoomBenefits draw = Library.CreateCard<KomachiModBoomBenefits>();
            // indicate them
            guide.ChoiceCardIndicator = 1; // uses extra description 1
            draw.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            guide.SetBattle(base.Battle);
            draw.SetBattle(base.Battle);
            // add em to the list
            list.Add(guide);
            list.Add(draw);
            return new MiniSelectCardInteraction(list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);

            // if enemy has no spirits, why are you even using this...?
            if (!selector.SelectedEnemy.HasStatusEffect<KomachiModVengefulSpiritSe>()) yield break;

            int vengefulSpiritsAmount = selector.SelectedEnemy.GetStatusEffect<KomachiModVengefulSpiritSe>().Count;
            yield return new RemoveStatusEffectAction(selector.SelectedEnemy.GetStatusEffect<KomachiModVengefulSpiritSe>(), true, 0.5f);

            if (card != null)
            {
                // -Gain 1 Guided Spirit for every {value1} detonated Vengeful Spirit.
                if (card.ChoiceCardIndicator == 1)
                {
                    int guidedSpiritsNum = vengefulSpiritsAmount / Value1;
                    yield return BuffAction<KomachiModGuidingSpiritSe>(guidedSpiritsNum);
                }
                // Draws 1 for every {value2} detonated Vengeful Spirit
                else
                {
                    int drawNum = vengefulSpiritsAmount / Value2;
                    yield return new DrawManyCardAction(drawNum);
                }
            }

            yield break;
        }
    }
}


