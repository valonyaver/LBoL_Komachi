using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModPushDrawDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            config.Value1 = 1;
            config.UpgradedValue1 = 3;

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModPushDrawDef))]
    public sealed class KomachiModPushDraw : KomachiCard
    {
        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list1 = new List<Card>();
            // make the 2 cards
            KomachiModPushDraw noPush = Library.CreateCard<KomachiModPushDraw>();
            KomachiModManDistance push1 = Library.CreateCard<KomachiModManDistance>();
            // indicate them
            noPush.ChoiceCardIndicator = 1; // uses extra description 1
            push1.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            noPush.SetBattle(base.Battle);
            push1.SetBattle(base.Battle);
            // add em to the list
            list1.Add(noPush);
            list1.Add(push1);
            if (this.IsUpgraded)
            {
                KomachiModManDistance2 push2 = Library.CreateCard<KomachiModManDistance2>();
                KomachiModManDistance3 push3 = Library.CreateCard<KomachiModManDistance3>();
                push2.ChoiceCardIndicator = 2; // uses extra description 2 of mandistance2
                push3.ChoiceCardIndicator = 2; // uses extra description 2
                push2.SetBattle(base.Battle);
                push3.SetBattle(base.Battle);
                list1.Add(push2);
                list1.Add(push3);
            }
            return new MiniSelectCardInteraction(list1);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // Pick displace action to take.
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
            if (card == null) yield break;
            // apply the card choice
            if (card.GetType() != typeof(KomachiModPushDraw))
            {
                yield return KomachiDistanceSe.ChangeDistanceLevel(selector.SelectedEnemy, card.Value1);
            }
            //int drawAmount = 3;
            //KomachiDistanceSe distance;
            //selector.SelectedEnemy.TryGetStatusEffect(out distance);
            //if (distance != null)
            //{
            //    drawAmount = distance.Level;
            //}

            yield return new DrawManyCardAction(KomachiDistanceSe.GetDistanceLevel(selector.SelectedEnemy));
            yield break;
        }
    }
}


