using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModShootAndMoveDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);
            config.IsPooled = true;

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2 };
            config.UpgradedCost = new ManaGroup() { Red = 2 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 14;
            config.UpgradedDamage = 20;

            // Value of the Displacement. Can displace up to +/- value1
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            // config.Keywords = Keyword.Displace;
            // config.UpgradedKeywords = Keyword.Displace;

            config.Illustrator = "@TheIllustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.Unfinished = true;
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModShootAndMoveDef))]
    public sealed class KomachiModShootAndMove : KomachiCard
    {
        //By default, if config.Damage / config.Block / config.Shield are set:
        //The card will deal damage or gain Block/Barrier without having to set anything.

        /// <summary>
        /// Copied straight from KomachiManDistance.cs
        /// </summary>
        /// <returns></returns>
        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list1 = new List<Card>();
            // make the 2 cards
            KomachiModManDistance manipulateDistancePull = Library.CreateCard<KomachiModManDistance>();
            KomachiModManDistance manipulateDistancePush = Library.CreateCard<KomachiModManDistance>();
            // indicate them
            manipulateDistancePull.ChoiceCardIndicator = 1; // uses extra description 1
            manipulateDistancePush.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            manipulateDistancePull.SetBattle(base.Battle);
            manipulateDistancePush.SetBattle(base.Battle);
            // add em to the list
            list1.Add(manipulateDistancePull);
            list1.Add(manipulateDistancePush);
            if (this.IsUpgraded)
            {
                // Batch create cards. Could write them same as above if i want to.
                List<KomachiModManDistance2> list2 = Library.CreateCards<KomachiModManDistance2>(2, upgraded: true).ToList();
                // notice how they are MAN DISTANCE 2?
                KomachiModManDistance2 manipulateDistancePullUp = list2[0];
                KomachiModManDistance2 manipulateDistancePushUp = list2[1];
                manipulateDistancePullUp.ChoiceCardIndicator = 1; // uses extra description 1 of mandistance2
                manipulateDistancePushUp.ChoiceCardIndicator = 2; // uses extra description 2
                manipulateDistancePushUp.SetBattle(base.Battle);
                manipulateDistancePullUp.SetBattle(base.Battle);
                list1.AddRange(list2);
            }
            return new MiniSelectCardInteraction(list1);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
            yield return base.AttackAction(selector, base.GunName);
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            // apply the card choice
            if (card != null)
            {
                // value 1 of mandistance2 is 2. value1 of mandistance 1 is 1
                // so whatever card is picked, take its value1.
                if (card.ChoiceCardIndicator == 1)
                {
                    // if it's card choice 1, pull enemy closer for a smooch
                    yield return new DistanceChangeAction(selector.SelectedEnemy, -card.Value1);
                }
                else
                {
                    // otherwise push them away like an introvert
                    yield return new DistanceChangeAction(selector.SelectedEnemy, card.Value1);
                }
            }
            yield break;
        }
    }
}


