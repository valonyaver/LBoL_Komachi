using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModQuickNapDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 7 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            config.Value1 = 5;
            config.UpgradedValue1 = 8;

            config.Mana = new ManaGroup() { Red = 1, Black = 1 };
            config.UpgradedMana = new ManaGroup() { Philosophy = 2, Colorless = 1 };

            config.Illustrator = "Valonadthe";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModQuickNapDef))]
    public sealed class KomachiModQuickNap : KomachiCard
    {
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.HealAction(base.Value1);
            yield return new GainTurnManaAction(base.Mana);
            yield return new RequestEndPlayerTurnAction();
            yield break;
        }
    }
}


