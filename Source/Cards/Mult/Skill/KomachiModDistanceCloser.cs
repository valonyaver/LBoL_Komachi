using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.Source.BattleActions.Helpers;
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
    public sealed class KomachiModDistanceCloserDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 7 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Distance manip
            config.Value1 = 2;
            config.UpgradedValue1 = 4;

            // Spirit infliction
            config.Value2 = 3;
            config.UpgradedValue2 = 4;

            config.Illustrator = "@evermythic";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() 
            { 
                nameof(KomachiDisplacementKeyword), 
                nameof(KomachiDistanceKeyword), 
                nameof(KomachiModVengefulSpiritSe), 
                nameof(KomachiModReleaseKeyword), 
                nameof(TempFirepower) 
            };
            config.UpgradedRelativeEffects = new List<string>() 
            {
                nameof(KomachiDisplacementKeyword),
                nameof(KomachiDistanceKeyword),
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiModReleaseKeyword),
                nameof(TempFirepower)
            };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModDistanceCloserDef))]
    public sealed class KomachiModDistanceCloser : KomachiCard
    {
        public int firepowerValue = 1;
        public int firepowerCond = 6;

        public int releaseCost = 3;

        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, releaseCost);
        }
        public Interaction Precondition2()
        {
            // Create list for interaction
            List<Card> list1 = new List<Card>();
            // make the 2 cards
            KomachiModManDistance pull1 = Library.CreateCard<KomachiModManDistance>();
            KomachiModManDistance2 pull2 = Library.CreateCard<KomachiModManDistance2>();
            // indicate them
            pull1.ChoiceCardIndicator = 1; // uses extra description 1
            pull2.ChoiceCardIndicator = 1; // uses extra description 1
            // dk what these do tbh.
            pull1.SetBattle(base.Battle);
            pull2.SetBattle(base.Battle);
            // add em to the list
            list1.Add(pull1);
            list1.Add(pull2);
            if (this.IsUpgraded)
            {
                KomachiModManDistance3 pull3 = Library.CreateCard<KomachiModManDistance3>();
                KomachiModManDistance4 pull4 = Library.CreateCard<KomachiModManDistance4>();
                pull3.ChoiceCardIndicator = 1; // uses extra description 1 of mandistance2
                pull4.ChoiceCardIndicator = 1; // uses extra description 1
                pull3.SetBattle(base.Battle);
                pull4.SetBattle(base.Battle);
                list1.Add(pull3);
                list1.Add(pull4);
            }
            return new MiniSelectCardInteraction(list1);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int displaceAmount = 0;
            // Pick displace action to take.
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)Precondition2();
            yield return new InteractionAction(miniSelectCardInteraction);
            Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
            if (card != null)
            {
                // apply the card choice
                displaceAmount = card.Value1;
                yield return new DistanceChangeAction(selector.SelectedEnemy, -card.Value1);
            }

            yield return new ApplyVengefulSpiritAction(this, selector.SelectedEnemy, displaceAmount * Value2);

            Card releaseCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(releaseCard))
            {
                yield return new KomachiReleaseAction(this, releaseCost);
                KomachiModVengefulSpiritSe spirits;
                selector.SelectedEnemy.TryGetStatusEffect(out spirits);
                if (spirits != null)
                {
                    yield return BuffAction<TempFirepower>(spirits.Count / firepowerCond);
                }
            }
            yield break;
        }
    }
}


