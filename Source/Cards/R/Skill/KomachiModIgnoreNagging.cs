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
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModIgnoreNaggingDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Red = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Amount of displacement
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            // Amount of graze
            config.Value2 = 1;

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModIgnoreNaggingDef))]
    public sealed class KomachiModIgnoreNagging : KomachiCard
    {
        public override Interaction Precondition()
        {
            if (!IsUpgraded) return null;
            // Create list for interaction
            List<Card> list1 = new List<Card>();
            // make the 2 cards
            KomachiModManDistance push1 = Library.CreateCard<KomachiModManDistance>();
            KomachiModManDistance2 push2 = Library.CreateCard<KomachiModManDistance2>();
            // indicate them
            push1.ChoiceCardIndicator = 1; // uses extra description 1
            push2.ChoiceCardIndicator = 1;
            // dk what these do tbh.
            push1.SetBattle(base.Battle);
            push2.SetBattle(base.Battle);
            // add em to the list
            list1.Add(push1);
            list1.Add(push2);
            return new MiniSelectCardInteraction(list1);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!IsUpgraded)
            {
                yield return new DistanceChangeAction(selector.SelectedEnemy, -Value1);
            }
            else
            {// Pick displace action to take.
                MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
                Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
                if (card == null) yield break;
                yield return new DistanceChangeAction(selector.SelectedEnemy, -card.Value1); // Value1 is the amount of distance that card changes
            }
            int grazeAmount = 0;
            foreach(var enemy in Battle.AllAliveEnemies)
            {
                int distance = KomachiDistanceSe.GetDistanceLevel(enemy);
                if (distance < 3)
                {
                    grazeAmount++;
                }
            }
            yield return BuffAction<Graze>(grazeAmount);


            yield break;
        }
    }
}


