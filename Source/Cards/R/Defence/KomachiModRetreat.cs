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

namespace KomachiMod.Cards
{
    public sealed class KomachiModRetreatDef : KomachiCardTemplate
    {


        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.ImageId = "KomachiBlockR";

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1, Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.SingleEnemy;

            config.Block = 12;
            config.UpgradedBlock = 15;

            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Value2 = 1;

            config.Illustrator = "Credit_the_artist";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModManDistance) };

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModRetreatDef))]
    public sealed class KomachiModRetreat : KomachiCard
    {
        protected override int BaseValue3 { get => 5; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 5; set => base.BaseUpgradedValue3 = value; }

        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModRetreat nopush = Library.CreateCard<KomachiModRetreat>();
            KomachiModManDistance push1 = Library.CreateCard<KomachiModManDistance>();
            // indicate them
            nopush.ChoiceCardIndicator = 1; // tells you dont push this guy
            push1.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            nopush.SetBattle(base.Battle);
            push1.SetBattle(base.Battle);
            // add em to the list
            list.Add(nopush);
            list.Add(push1);
            if (IsUpgraded)
            {
                KomachiModManDistance2 push2 = Library.CreateCard<KomachiModManDistance2>();
                push2.ChoiceCardIndicator = 2; // uses extra description 2
                push2.SetBattle(base.Battle);
                list.Add(push2);
            }
            return new MiniSelectCardInteraction(list);
        }

        public Interaction Precondition2()
        {
            // Create list for interaction
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModRetreat nopush = Library.CreateCard<KomachiModRetreat>();
            KomachiModManDistance1AllExcept push1 = Library.CreateCard<KomachiModManDistance1AllExcept>();
            // indicate them
            nopush.ChoiceCardIndicator = 2; // Tells you dont push others
            push1.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            nopush.SetBattle(base.Battle);
            push1.SetBattle(base.Battle);
            // add em to the list
            list.Add(nopush);
            list.Add(push1);
            return new MiniSelectCardInteraction(list);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card singlepushcard = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
            if (singlepushcard != null && singlepushcard.GetType() != typeof(KomachiModRetreat))
            {
                yield return new DistanceChangeAction(selector.SelectedEnemy, singlepushcard.Value1);
            }

            if (IsUpgraded)
            {
                MiniSelectCardInteraction miniSelectCardInteraction2 = (MiniSelectCardInteraction)Precondition2();
                yield return new InteractionAction(miniSelectCardInteraction2);
                Card aoepushcard = ((miniSelectCardInteraction2 != null) ? miniSelectCardInteraction2.SelectedCard : null);
                if (aoepushcard != null && singlepushcard.GetType() != typeof(KomachiModRetreat))
                {
                    foreach (var enemy in Battle.AllAliveEnemies)
                    {
                        if (enemy == selector.SelectedEnemy) continue;
                        yield return new DistanceChangeAction(enemy, aoepushcard.Value1);
                    }
                }
            }

            yield return DefenseAction(true);
            if (KomachiDistanceSe.GetDistanceLevel(selector.SelectedEnemy) == Value3)
            {
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModManDistance>() });
            }
        }
    }
}


