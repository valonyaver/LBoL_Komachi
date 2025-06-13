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
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModManDistanceDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);
            //If IsPooled is false then the card cannot be discovered or added to the library at the end of combat.
            config.IsPooled = true;

            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            config.Value1 = 1;
            config.Value2 = 2;

            config.Keywords = Keyword.Exile | Keyword.Retain;
            //Setting Upgrading Keyword only provides the keyword when the card is upgraded.    
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;

            config.Illustrator = ""; 

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModManDistanceDef))]
    public sealed class KomachiModManDistance : KomachiCard
    {
        /// <summary>
        /// Had to get a little creative to make this give 4 options that apply up to +/-2 displacement when upgraded.
        /// The functionality works simply but the descriptions don't, because extra descriptions 3 and 4 don't work for card choices for whatever reason.
        /// So what I did was make a second card, and use THAT for its extra descriptions.
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
            if (card != null)
            {
                // value 1 of mandistance2 is 2. value1 of mandistance 1 is 1
                // so whatever card is picked, take its value1.
                if (card.ChoiceCardIndicator == 1)
                {
                    // if it's card choice 1, pull enemy closer for a smooch
                    // yield return KomachiDistanceSe.ChangeDistanceLevel(selector.SelectedEnemy, -card.Value1);
                    yield return new DistanceChangeAction(selector.SelectedEnemy, -card.Value1);
                }
                else
                {
                    // otherwise push them away like an introvert
                    // yield return KomachiDistanceSe.ChangeDistanceLevel(selector.SelectedEnemy, card.Value1);
                    yield return new DistanceChangeAction(selector.SelectedEnemy, card.Value1);
                }
            }
            yield break;
        }
    }
}


