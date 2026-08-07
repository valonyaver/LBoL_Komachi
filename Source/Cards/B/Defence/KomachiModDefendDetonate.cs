using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.Source.StatusEffects.Spirits;
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

            // config.ImageId = nameof(KomachiModBlockB);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1, Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.SingleEnemy;

            config.Block = 12;
            config.UpgradedBlock = 16;

            // Amount of vengeful spirits inflicted
            config.Value1 = 2;
            config.UpgradedValue1 = 4;

            // Temp attack down inflicted
            config.Value2 = 1;

            config.Illustrator = "ひでふキタヤン(hidehu kitayan)";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() 
            { nameof(KomachiDetonationKeyword), nameof(KomachiModVengefulSpiritSe), nameof(TempFirepowerNegative) };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiDetonationKeyword), nameof(KomachiModVengefulSpiritSe), nameof(TempFirepowerNegative) };
            // config.Unfinished = true;
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
            // apply spirits if it has it
            if (Value1 > 0)
            {
                yield return DebuffAction<KomachiModVengefulSpiritSe>(selector.SelectedEnemy, count: Value1, duration:3, occupationTime: 1f);
            }
            // remove dat
            if (card != null && card.ChoiceCardIndicator == 2)
            {
                var detonation = new DetonateVengefulSpiritAction(this, selector.SelectedEnemy);
                yield return detonation;

                int vengefulSpiritsAmount = detonation.Args.amountDetonated;
                if (vengefulSpiritsAmount > 0)
                {
                    int firepowerdownAmount = vengefulSpiritsAmount / 2;
                    yield return DebuffAction<TempFirepowerNegative>(selector.SelectedEnemy, firepowerdownAmount);
                }
            }
        }
    }
}


