using BepInEx;
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
    public sealed class KomachiModTalkativeFerrymanDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1, Any = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Daily spirit gain
            config.Value1 = 3;
            config.UpgradedValue1 = 5;

            // Release cost
            config.Value2 = 4;

            config.RelativeEffects = new List<string>()
            { 
                nameof(KomachiModGuidedSpiritSe), nameof(KomachiModReleaseKeyword)
            };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiModVengefulSpiritSe), nameof(KomachiModReleaseKeyword)};

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModTalkativeFerrymanDef))]
    public sealed class KomachiModTalkativeFerryman : KomachiCard
    {
        protected override int BaseValue3 { get => 8; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 12; set => base.BaseUpgradedValue3 = value; }

        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModTalkativeFerrymanSe>(base.Value2, 0, 0, 0, 0.2f);

            Card choice = KomachiModUtility.GetPreconditionCard(precondition);
            if (choice != null && choice.GetType() != typeof(KomachiModReleaseNone))
            {
                yield return new KomachiReleaseAction(this, Value2);
                yield return BuffAction<KomachiModGuidedGeneratorNextTurnSe>(Value3);
            }
            yield break;
        }
    }
}


