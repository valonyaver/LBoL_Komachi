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
            config.IsPooled = false;

            config.ImageId = nameof(KomachiModMoveAndShoot);

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1, Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 12;
            config.UpgradedDamage = 16;

            // Value of the Displacement. Can displace up to +/- value1
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            // config.Keywords = Keyword.Displace;
            // config.UpgradedKeywords = Keyword.Displace;

            config.Illustrator = "sakimiya@土曜つ28a";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            // config.Unfinished = true;
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
            if (this.IsUpgraded)
            {
                // notice how they are MAN DISTANCE 2?
                KomachiModManDistance2 manipulateDistancePull2 = Library.CreateCard<KomachiModManDistance2>(upgraded: true);
                manipulateDistancePull2.ChoiceCardIndicator = 1; // uses extra description 1 of mandistance2
                manipulateDistancePull2.SetBattle(base.Battle);
                list1.Add(manipulateDistancePull2);
            }
            // make the 2 cards
            KomachiModManDistance manipulateDistancePull1 = Library.CreateCard<KomachiModManDistance>();
            KomachiModManDistance0 manipulateDistance0 = Library.CreateCard<KomachiModManDistance0>();
            KomachiModManDistance manipulateDistancePush1 = Library.CreateCard<KomachiModManDistance>();
            // indicate them
            manipulateDistancePull1.ChoiceCardIndicator = 1; // uses extra description 1
            manipulateDistance0.ChoiceCardIndicator = 1; // uses extra description 1
            manipulateDistancePush1.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            manipulateDistancePull1.SetBattle(base.Battle);
            manipulateDistance0.SetBattle(base.Battle);
            manipulateDistancePush1.SetBattle(base.Battle);
            // add em to the list
            list1.Add(manipulateDistancePull1);
            list1.Add(manipulateDistance0);
            list1.Add(manipulateDistancePush1);
            if (this.IsUpgraded)
            {
                KomachiModManDistance2 manipulateDistancePush2 = Library.CreateCard<KomachiModManDistance2>(upgraded: true);
                manipulateDistancePush2.ChoiceCardIndicator = 2; // uses extra description 2
                manipulateDistancePush2.SetBattle(base.Battle);
                list1.Add(manipulateDistancePush2);
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
            if (card != null && card.GetType() != typeof(KomachiModManDistance0))
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


