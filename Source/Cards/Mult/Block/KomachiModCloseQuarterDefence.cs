using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Koishi;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
using static UnityEngine.UI.GridLayoutGroup;

namespace KomachiMod.Cards
{
    public sealed class KomachiModCloseQuarterDefenceDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();

            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            //Hybrid color 7 = B/R
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 7 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.SingleEnemy;

            config.Block = 5;
            config.UpgradedBlock = 7;
            
            // Vengeful Spirits inflicted
            config.Value1 = 4;
            config.UpgradedValue1 = 6;

            // Guided Spirits gained
            config.Value2 = 4;
            config.UpgradedValue2 = 8;

            config.Illustrator = "ぺろぽねそす";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe), nameof(KomachiDistanceKeyword), nameof(KomachiModGuidedSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe), nameof(KomachiDistanceKeyword), nameof(KomachiModGuidedSpiritSe) };
            config.Unfinished = false;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModCloseQuarterDefenceDef))]
    public sealed class KomachiModCloseQuarterDefence : KomachiCard 
    {
        
        // Additional block amount
        protected override int BaseValue3 { get => 8; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 11; set => base.BaseUpgradedValue3 = value; }
        public int additionalBlockMeasure
        {
            get
            {
                if (Battle == null) return Value3;
                BlockShieldEventArgs rawBlockingArgs = new BlockShieldEventArgs
                {
                    Source = Battle.Player,
                    Target = Battle.Player,
                    Block = Block.Block,
                    HasBlock = true,
                    HasShield = false,
                    Type = BlockShieldType.Normal,
                    ActionSource = this,
                    Cause = ActionCause.Card
                };
                Battle.Player?.BlockShieldGaining.Execute(rawBlockingArgs);
                BlockShieldEventArgs blockingArgs = new BlockShieldEventArgs
                {
                    Source = Battle.Player,
                    Target = Battle.Player,
                    Block = Value3 + RawBlock,
                    HasBlock = true,
                    HasShield = false,
                    Type = BlockShieldType.Normal,
                    ActionSource = this,
                    Cause = ActionCause.Card
                };
                Battle.Player?.BlockShieldGaining.Execute(blockingArgs);
                blockingArgs.Block -= rawBlockingArgs.Block;
                return (int)blockingArgs.Block.Round(MidpointRounding.AwayFromZero);
            }
        }
        public string additionalBlockString
        {
            get
            {
                int block = additionalBlockMeasure;
                string color = KomachiModUtility.GetColorFromDamage(block, Value3);
                return $"<color=#{color}>{block}</color>";
            }
        }
        // Additional block from close smooching
        bool getAdditionalBlock;
        protected override int AdditionalBlock
        {
            get
            {
                if (base.Battle == null || !getAdditionalBlock)
                {
                    return 0;
                }
                return Value3;
            }
        }

        public int distanceLimit = 3;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (KomachiModDistanceSe.GetDistanceLevel(selector.SelectedEnemy) < 3)
            {
                getAdditionalBlock = true;
            }
            else getAdditionalBlock = false;
            yield return DefenseAction(true);
            yield return new ApplyVengefulSpiritAction(selector.SelectedEnemy, Value1);

            if (KomachiModDistanceSe.GetDistanceLevel(selector.SelectedEnemy) < 3)
            {
                yield return BuffAction<KomachiModGuidedSpiritSe>(Value2);
            }
            getAdditionalBlock = false;
        }
    }
}


