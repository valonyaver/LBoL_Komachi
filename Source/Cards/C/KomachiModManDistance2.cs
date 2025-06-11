using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    // Duplicate of ManDistance because extra description 1 and 2 don't work for some reason
    public sealed class KomachiModManDistance2Def : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);
            //If IsPooled is false then the card cannot be discovered or added to the library at the end of combat.
            config.IsPooled = true;
            config.HideMesuem = true;

            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            config.Value1 = 2;
            config.Value2 = 3;

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
    
    [EntityLogic(typeof(KomachiModManDistance2Def))]
    public sealed class KomachiModManDistance2 : KomachiCard
    {
        /// <summary>
        /// Taken from CirnoConsider
        /// Copy this code for cards that give an option to push or pull cards.
        /// Make sure to change the type of cards in the list though.
        /// Also, should probably make a SUPER manipulate distance for cards that +/- by 3 or more distance.
        /// Just look at your code okay?
        /// </summary>
        /// <returns></returns>
        public override Interaction Precondition()
        {
            int size = 2;
            List<KomachiModManDistance> list1 = Library.CreateCards<KomachiModManDistance>(2, false).ToList();
            KomachiModManDistance manipulateDistancePull = list1[0]; // uses extra description 1
            KomachiModManDistance manipulateDistancePush = list1[1]; // uses extra description 2
            manipulateDistancePull.ChoiceCardIndicator = 1;
            manipulateDistancePush.ChoiceCardIndicator = 2;
            manipulateDistancePull.SetBattle(base.Battle);
            manipulateDistancePush.SetBattle(base.Battle);
            if (this.IsUpgraded)
            {
                List<KomachiModManDistance> list2 = Library.CreateCards<KomachiModManDistance>(2, true).ToList();
                KomachiModManDistance manipulateDistancePullUp = list2[0]; // uses extra description 3
                KomachiModManDistance manipulateDistancePushUp = list2[1]; // uses extra description 4
                manipulateDistancePullUp.ChoiceCardIndicator = 3;
                manipulateDistancePushUp.ChoiceCardIndicator = 4;
                manipulateDistancePushUp.SetBattle(base.Battle);
                manipulateDistancePullUp.SetBattle(base.Battle);
                list1.AddRange(list2);
            }
            return new MiniSelectCardInteraction(list1, false, false, false);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
            if (card != null)
            {
                if (card.ChoiceCardIndicator == 1)
                {
                    yield return base.DebuffAction<KomachiDistanceSe>(selector.SelectedEnemy, -Value1);
                }
                else if (card.ChoiceCardIndicator == 2)
                {
                    yield return base.DebuffAction<KomachiDistanceSe>(selector.SelectedEnemy, Value1);
                }
                else if (card.ChoiceCardIndicator == 3)
                {
                    yield return base.DebuffAction<KomachiDistanceSe>(selector.SelectedEnemy, -Value2);
                }
                else
                {
                    yield return base.DebuffAction<KomachiDistanceSe>(selector.SelectedEnemy, -Value2);
                }
            }
            yield break;
        }
    }
}


