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
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModRitualOfEcstasyDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            config.Block = 5;
            // Draw
            config.Value1 = 3;

            // Graze
            config.Value2 = 1;

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };


            config.Illustrator = "Zuo大鴿";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModRitualOfEcstasyDef))]
    public sealed class KomachiModRitualOfEcstasy : KomachiCard
    {
        public int three = 3;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int originalDistance = KomachiModDistanceSe.GetDistanceLevel(selector.SelectedEnemy);
            if (originalDistance != 3)
            {
                // Inverts distance
                int newDistance = -(originalDistance - 3) + 3;
                yield return new DistanceChangeAction(selector.SelectedEnemy, newDistance - originalDistance);
                if (newDistance < originalDistance)
                {
                    yield return new DrawManyCardAction(Value1);
                }
                else
                {
                    yield return BuffAction<Graze>(Value2);
                    yield return DefenseAction();
                }
            }
            else
            {
                Card drawChoice = Library.CreateCard<KomachiModRitualOfEcstasy>(IsUpgraded);
                drawChoice.ChoiceCardIndicator = 1;
                drawChoice.SetBattle(Battle);
                Card blockChoice = Library.CreateCard<KomachiModRitualOfEcstasy>(IsUpgraded);
                blockChoice.ChoiceCardIndicator = 2;
                blockChoice.SetBattle(Battle);
                List<Card> choices = new List<Card>() { drawChoice, blockChoice };
                MiniSelectCardInteraction chooseEffect = new MiniSelectCardInteraction(choices);
                yield return new InteractionAction(chooseEffect);
                var choiceCard = chooseEffect.SelectedCard;
                if (choiceCard.ChoiceCardIndicator == 1)
                {
                    yield return new DrawManyCardAction(Value1);
                }
                else
                {
                    yield return BuffAction<Graze>(Value2);
                    yield return DefenseAction();
                }
            }
            yield break;
        }
    }
}


