using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

namespace KomachiMod.Cards
{
    public sealed class KomachiMultipleKeywordsDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            config.Value1 = 5;
            config.UpgradedValue1 = 8;

            config.RelativeEffects = new List<string>() { nameof(TempFirepower), nameof(TempFirepowerNegative)};
            config.UpgradedRelativeEffects = new List<string>() { nameof(TempFirepower), nameof(TempFirepowerNegative) };

            //Multiple Keywords are separated by a |
            config.Keywords = Keyword.Exile | Keyword.Ethereal | Keyword.Initial;
            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiMultipleKeywordsDef))]
    public sealed class KomachiMultipleKeywords : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            EnemyUnit selectedEnemy = selector.SelectedEnemy;
            yield return base.BuffAction<TempFirepower>(base.Value1, 0, 0, 0, 0.2f);
            yield return base.DebuffAction<TempFirepowerNegative>(selectedEnemy, base.Value1, 0, 0, 0, true, 0.2f);
            yield break;
        }
    }
}


