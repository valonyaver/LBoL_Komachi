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
    public class KomachiModManDistanceTemplate : KomachiCard 
    {
        public string extraDescriptionAddition; 
        /// <summary>
        /// This is literally only for shinigami tactics.
        /// </summary>
        public int lastDistanceChange = 0;
        /// <summary>
        /// Had to get a little creative to make this give 4 options that apply up to +/-2 displacement when upgraded.
        /// The functionality works simply but the descriptions don't, because extra descriptions 3 and 4 don't work for card choices for whatever reason.
        /// So what I did was make a second card, and use THAT for its extra descriptions.
        /// </summary>
        /// <returns></returns>
        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list = new List<Card>();
            var unupgradedManDist = manDistanceType(Value1);
            var upgradedManDist = manDistanceType(Value2);
            // Generates distance -2
            if (this.IsUpgraded)
            {
                var manipulateDistancePull2 = Library.CreateCard(upgradedManDist.GetType());
                manipulateDistancePull2.ChoiceCardIndicator = 1; // uses extra description 1 of mandistance2
                manipulateDistancePull2.SetBattle(base.Battle);
                list.Add(manipulateDistancePull2);
            }
            // Generate distance -1
            var manipulateDistancePull = Library.CreateCard(unupgradedManDist.GetType());
            manipulateDistancePull.ChoiceCardIndicator = 1; // uses extra description 1
            manipulateDistancePull.SetBattle(base.Battle);
            list.Add(manipulateDistancePull);
            // Generate distance +1
            var manipulateDistancePush = Library.CreateCard(unupgradedManDist.GetType()); 
            manipulateDistancePush.ChoiceCardIndicator = 2; // uses extra description 2
            manipulateDistancePush.SetBattle(base.Battle);
            list.Add(manipulateDistancePush);
            // Generates distance -2
            if (this.IsUpgraded)
            {
                var manipulateDistancePush2 = Library.CreateCard(upgradedManDist.GetType());
                manipulateDistancePush2.ChoiceCardIndicator = 2; // uses extra description 2 of mandistance2
                manipulateDistancePush2.SetBattle(base.Battle);
                list.Add(manipulateDistancePush2);
            }
            return new MiniSelectCardInteraction(list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
            int originalDistance = KomachiDistanceSe.GetDistanceLevel(selector.SelectedEnemy);
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

            lastDistanceChange = KomachiDistanceSe.GetDistanceLevel(selector.SelectedEnemy) - originalDistance;
            yield break;
        }

        public static Card manDistanceType(int value)
        {
            switch (value)
            {
                case 0:
                    return Library.CreateCard<KomachiModManDistance0>();
                case 1:
                default:
                    return Library.CreateCard<KomachiModManDistance>();
                case 2:
                    return Library.CreateCard<KomachiModManDistance2>();
                case 3:
                    return Library.CreateCard<KomachiModManDistance3>();
            }
        }
    }
}


