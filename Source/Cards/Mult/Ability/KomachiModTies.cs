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

namespace KomachiMod.Source.Cards.Mult.Ability
{
    public sealed class KomachiModTiesDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Blue, ManaColor.Red };
            config.Cost = new ManaGroup() { Blue = 1, Black = 1, Red = 1, Any = 1};
            config.UpgradedCost = new ManaGroup() { Hybrid = 2, HybridColor = 4, Red = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Level of buff
            config.Value1 = 1;

            config.Mana = new ManaGroup() { Any = 1 };

            config.Illustrator = "wjstpwls4";

            config.RelativeKeyword = Keyword.Copy;
            config.UpgradedRelativeKeyword = Keyword.Copy;

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModTiesDef))]
    public sealed class KomachiModTies : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModTiesSe>(Value1, 0, 0, 0, 0.2f);
        }
    }
}


