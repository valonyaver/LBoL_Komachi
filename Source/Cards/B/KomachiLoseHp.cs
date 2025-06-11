using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Units;
using LBoL.Core.Battle.BattleActions;

namespace KomachiMod.Cards
{
    public sealed class KomachiLoseHpDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            config.Value1 = 2;
            config.UpgradedValue1 = 3;

            config.Value2 = 18;
            config.UpgradedValue2 = 24;

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiLoseHpDef))]
    public sealed class KomachiLoseHp : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            EnemyUnit selectedEnemy = selector.SelectedEnemy;
            //The player loses life:
            yield return LoseLifeAction(base.Value1);
            //This is equvalent to:
            //yield return new DamageAction(Battle.Player, Battle.Player, DamageInfo.HpLose(base.Value2));

            //Target enemy loses hp.
            yield return new DamageAction(Battle.Player, selectedEnemy, DamageInfo.HpLose(base.Value2));
            yield break;
        }
    }
}


