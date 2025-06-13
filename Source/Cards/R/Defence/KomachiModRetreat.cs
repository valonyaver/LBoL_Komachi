using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModRetreatDef : KomachiCardTemplate
    {


        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.IsPooled = true;

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2 };
            config.UpgradedCost = new ManaGroup() { Red = 1, Any = 1};
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.SingleEnemy;

            config.Block = 12;
            config.UpgradedBlock = 15;

            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Value2 = 1;

            config.Illustrator = "Credit_the_artist";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModRetreatDef))]
    public sealed class KomachiModRetreat : KomachiCard
    {
        //By default, if config.Damage / config.Block / config.Shield are set:
        //The card will deal damage or gain Block/Barrier without having to set anything.
        //Here, this is is equivalent to the following code.
         
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (this.IsUpgraded)
            {
                foreach (EnemyUnit enemyUnit in base.Battle.AllAliveEnemies)
                {
                    if (selector.SelectedEnemy == enemyUnit)
                    {
                        yield return new DistanceChangeAction(enemyUnit, Value1);
                    }
                    else
                    {
                        yield return new DistanceChangeAction(enemyUnit, Value2);
                    }
                }
            }
            else
            {
                yield return new DistanceChangeAction(selector.SelectedEnemy, Value1);
            }
            yield return DefenseAction(true);
        }
    }
}


