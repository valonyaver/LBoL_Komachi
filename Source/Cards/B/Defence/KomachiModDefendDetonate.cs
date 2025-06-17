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
using System.Linq;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModDefendDetonateDef : KomachiCardTemplate
    {


        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.ImageId = "KomachiBlockB";

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.SingleEnemy;

            config.Block = 14;
            config.UpgradedBlock = 17;

            // Amount of vengeful spirits inflicted
            config.Value1 = 0;
            config.UpgradedValue1 = 2;

            // Temp attack down inflicted
            config.Value2 = 1;

            config.Illustrator = "Credit_the_artist";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() 
            { nameof(KomachiDetonationKeyword), nameof(KomachiModVengefulSpiritSe), nameof(TempFirepowerNegative) };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiDetonationKeyword), nameof(KomachiModVengefulSpiritSe), nameof(TempFirepowerNegative) };
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModDefendDetonateDef))]
    public sealed class KomachiModDefendDetonate : KomachiCard
    {
        // how many vengeful spirits are needed for each debuff
        protected override int BaseValue3
        {
            get { return 2; }
        }

        protected override int BaseUpgradedValue3
        {
            get { return 2; }
        }
        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list1 = new List<Card>();
            // make the 2 cards
            KomachiModDefendDetonate dontexplode = Library.CreateCard<KomachiModDefendDetonate>();
            KomachiModDefendDetonate explode = Library.CreateCard<KomachiModDefendDetonate>();
            // indicate them
            dontexplode.ChoiceCardIndicator = 1; // uses extra description 1
            explode.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            dontexplode.SetBattle(base.Battle);
            explode.SetBattle(base.Battle);
            // add em to the list
            list1.Add(dontexplode);
            list1.Add(explode);
            return new MiniSelectCardInteraction(list1);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
            yield return DefenseAction(true);
            if (Value1 > 0)
            {
                yield return DebuffAction<KomachiModVengefulSpiritSe>(selector.SelectedEnemy, count: Value1, duration:3, occupationTime: 1f);
            }
            if (selector.SelectedEnemy.HasStatusEffect<KomachiModVengefulSpiritSe>() && card != null && card.ChoiceCardIndicator == 2)
            {
                int vengefulSpiritsAmount = selector.SelectedEnemy.GetStatusEffect<KomachiModVengefulSpiritSe>().Count;
                yield return new RemoveStatusEffectAction(selector.SelectedEnemy.GetStatusEffect<KomachiModVengefulSpiritSe>(), true, 0.5f);

                int firepowerdownAmount = vengefulSpiritsAmount / 2;
                yield return DebuffAction<TempFirepowerNegative>(selector.SelectedEnemy, firepowerdownAmount);
            }
        }
    }
}


