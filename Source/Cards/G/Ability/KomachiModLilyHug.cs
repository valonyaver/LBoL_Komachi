using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModLilyHugDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            config.Cost = new ManaGroup() { Red = 1, Green =1, Any = 1 };
            config.UpgradedCost = new ManaGroup() { HybridColor = 9, Hybrid = 1, Any = 1 };
            config.Rarity = Rarity.Uncommon; 
            config.IsPooled = false;
            config.HideMesuem = true;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Spider lilies added the hand
            config.Value1 = 1;

            // Buff amount.
            config.Value2 = 1;

            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };

            config.RelativeEffects = new List<string>() { nameof(Amulet) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(Amulet) };

            config.Illustrator = "";
            config.Unfinished = true;

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModLilyHugDef))]
    public sealed class KomachiModLilyHug : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new AddCardsToHandAction(Library.CreateCards<KomachiModSpiderLily>(Value1, false));
            yield return BuffAction <KomachiModLilyHugSe>(base.Value2, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}


