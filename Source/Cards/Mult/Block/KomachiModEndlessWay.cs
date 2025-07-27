using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.UI.CanvasScaler;

namespace KomachiMod.Cards
{
    public sealed class KomachiModEndlessWayDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();

            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            //Hybrid color 7 = B/R
            config.Cost = new ManaGroup() { Red = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 2, HybridColor = 7};
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.SingleEnemy;

            config.Block = 13;
            config.UpgradedBlock = 17;

            // Push Amount
            config.Value1 = 2;

            // Release cost 1
            config.Value2 = 3;
            config.UpgradedValue2 = 2;

            config.Illustrator = "樫の木";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.Unfinished = false;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModEndlessWayDef))]
    public sealed class KomachiModEndlessWay : KomachiCard
    {
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(this, Value2);
        // Release cost 2
        protected override int BaseValue3 { get => 6; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 4; set => base.BaseUpgradedValue3 = value; }

        int lockOnAmount = 3;
        // For description
        int lockOnAmount2 => lockOnAmount * 2;
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2, Value3);
        }
         
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new DistanceChangeAction(selector.SelectedEnemy, Value1);
            if (selector.SelectedEnemy.HasStatusEffect<Graze>())
            {
                yield return new RemoveStatusEffectAction(selector.SelectedEnemy.GetStatusEffect<Graze>(), true, 0.1f);
            }
            yield return DefenseAction(true);
            Card releaseChoice = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(releaseChoice))
            {
                int releaseAmount = Value2;
                if (releaseChoice.ChoiceCardIndicator == 2) releaseAmount = Value3;
                yield return new KomachiReleaseAction(this, releaseAmount);
                int lockonLevel = releaseChoice.ChoiceCardIndicator * lockOnAmount;
                yield return DebuffAction<LockedOn>(selector.SelectedEnemy, lockonLevel);
            }
        }
    }
}


