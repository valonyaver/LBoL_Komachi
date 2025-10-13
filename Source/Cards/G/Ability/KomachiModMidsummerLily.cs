using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModMidsummerLilyDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Green };
            config.Cost = new ManaGroup() { Green = 1};
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Midsummers per card
            config.Value1 = 1;
            // Lily amount
            config.Value2 = 1;


            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily), nameof(SummerFlower) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily), nameof(SummerFlower) };

            config.Illustrator = "MARI";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModMidsummerLilyDef))]
    public sealed class KomachiModMidsummerLily : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new AddCardsToDrawZoneAction(Library.CreateCards<KomachiModSpiderLily>(base.Value1, false), DrawZoneTarget.Top, AddCardsType.Normal);
            yield return BuffAction<KomachiModMidsummerLilySe>(base.Value2, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}


