using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModMoveTokenDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            // config.ImageId = "";
            //If IsPooled is false then the card cannot be discovered or added to the library at the end of combat.
            config.IsPooled = false;

            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            config.Value1 = 1;
            config.Value2 = 2;

            config.Keywords = Keyword.Exile | Keyword.Ethereal | Keyword.Echo;
            //Setting Upgrading Keyword only provides the keyword when the card is upgraded.    
            config.UpgradedKeywords = Keyword.Exile | Keyword.Ethereal | Keyword.Echo;

            config.Mana = ManaGroup.Anys(1);
            config.UpgradedMana = ManaGroup.Anys(1);


            config.Illustrator = "60mai"; 

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModMoveTokenDef))]
    public sealed class KomachiModMoveToken : KomachiCard 
    {
        protected override ManaGroup AdditionalCost
        {
            get
            {
                if (Battle == null) return base.AdditionalCost;
                int cardsUsed = Battle.TurnCardUsageHistory.Count;
                return ManaGroup.Anys(cardsUsed);
            }
        }
        /// <summary>
        /// This is just a dummy card for rowingRetreat
        /// </summary>
        /// <returns></returns>
        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list1 = new List<Card>();
            // Make displace -2
            if (IsUpgraded)
            {
                KomachiModMoveToken manipulateDistancePullUp = Library.CreateCard<KomachiModMoveToken>(IsUpgraded);
                manipulateDistancePullUp.ChoiceCardIndicator = 1; // uses extra description 1 of mandistance2
                manipulateDistancePullUp.SetBattle(base.Battle);
                list1.Add(manipulateDistancePullUp);
            }
            // make displace -1
            KomachiModMoveToken manipulateDistancePull = Library.CreateCard<KomachiModMoveToken>();
            manipulateDistancePull.ChoiceCardIndicator = 1; // uses extra description 1
            manipulateDistancePull.SetBattle(base.Battle);
            list1.Add(manipulateDistancePull);

            // Make displace +1
            KomachiModMoveToken manipulateDistancePush = Library.CreateCard<KomachiModMoveToken>(); 
            manipulateDistancePush.ChoiceCardIndicator = 2; // uses extra description 2
            manipulateDistancePush.SetBattle(base.Battle);
            list1.Add(manipulateDistancePush);

            // Make displace +2
            if (this.IsUpgraded)
            {
                KomachiModMoveToken manipulateDistancePushUp = Library.CreateCard<KomachiModMoveToken>(IsUpgraded);
                manipulateDistancePushUp.ChoiceCardIndicator = 2; // uses extra description 2
                manipulateDistancePushUp.SetBattle(base.Battle);
                list1.Add(manipulateDistancePushUp);
            }
            return new MiniSelectCardInteraction(list1);
        }

        public int displaceAmount
        {
            get
            {
                if (IsUpgraded) return Value2;
                else return Value1;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            KomachiModMoveToken card = (KomachiModMoveToken) KomachiModUtility.GetPreconditionCard(precondition);
            if (card != null)
            {
                // value 1 of mandistance2 is 2. value1 of mandistance 1 is 1
                // so whatever card is picked, take its value1.
                if (card.ChoiceCardIndicator == 1)
                {
                    // if it's card choice 1, pull enemy closer for a smooch
                    // yield return KomachiDistanceSe.ChangeDistanceLevel(selector.SelectedEnemy, -card.Value1);
                    foreach(var enemy in Battle.AllAliveEnemies)
                    {
                        yield return new DistanceChangeAction(enemy, -card.displaceAmount);
                    }
                }
                else
                {
                    // otherwise push them away like an introvert
                    // yield return KomachiDistanceSe.ChangeDistanceLevel(selector.SelectedEnemy, card.Value1);
                    foreach (var enemy in Battle.AllAliveEnemies)
                    {
                        yield return new DistanceChangeAction(enemy, card.displaceAmount);
                    }
                }
            }

            yield break;
        }
    }
}


