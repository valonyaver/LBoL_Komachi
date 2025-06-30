using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    /// <summary>
    /// It's actually an ability not a block card
    /// </summary>
    public sealed class KomachiModEyeOfTheStormDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.ImageId = "KomachiBlockB";

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2, Any =2 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.Nobody;

            config.Block = 14;
            config.UpgradedBlock = 18;

            config.Shield = 6;
            config.UpgradedShield = 8;

            // Vengeful Spirits inflicted
            config.Value1 = 4;
            config.UpgradedValue1 = 6;

            // Guided Spirits gained
            config.Value2 = 6;
            config.UpgradedValue2 = 8;

            config.RelativeEffects = new List<string>() 
            { nameof(KomachiModGuidedSpiritSe), nameof(KomachiModVengefulSpiritSe), nameof(KomachiModReleaseKeyword) };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiModGuidedSpiritSe), nameof(KomachiModVengefulSpiritSe), nameof(KomachiModReleaseKeyword) };

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModEyeOfTheStormDef))]
    public sealed class KomachiModEyeOfTheStorm : KomachiCard
    {
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, Value3);
        // Release cost
        protected override int BaseValue3 { get => 6; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 6; set => base.BaseUpgradedValue3 = value; }

        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value3);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction(Block.Block, 0);
            foreach(var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(enemy, Value1);
            }
            yield return BuffAction<KomachiModGuidedSpiritSe>(base.Value2, 0, 0, 0, 0.2f);

            Card choiceCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(choiceCard))
            {
                yield return new KomachiReleaseAction(this, Value3);
                yield return DefenseAction(0, Shield.Shield);
            }
            yield break;
        }
    }
}


