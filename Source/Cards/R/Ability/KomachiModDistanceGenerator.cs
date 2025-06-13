using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModDistanceGeneratorDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2 };
            config.UpgradedCost = new ManaGroup() { Red = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            config.Value1 = 1;

            config.Value2 = 0;
            config.UpgradedValue2 = 1;

            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModManDistance) };

            config.RelativeEffects = new List<string>() { nameof(KomachiModDistanceGeneratorSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModDistanceGeneratorSe) };
            

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModDistanceGeneratorDef))]
    public sealed class KomachiModDistanceGenerator : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModDistanceGeneratorSe>(base.Value1, 0, 0, 0, 0.2f);
            if (base.Value2 > 0)
            {
                yield return new AddCardsToHandAction(Library.CreateCards<KomachiModManDistance>(base.Value2, false), AddCardsType.Normal);
            }
            yield break;
        }
    }
}


