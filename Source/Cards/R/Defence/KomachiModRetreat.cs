using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
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
            config.ImageId = "KomachiBlockR";

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

            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModManDistance) };

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModRetreatDef))]
    public sealed class KomachiModRetreat : KomachiCard
    {
        protected override int BaseValue3 { get => 5; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 5; set => base.BaseUpgradedValue3 = value; }
         
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
            if (KomachiDistanceSe.GetDistanceLevel(selector.SelectedEnemy) == Value3)
            {
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModManDistance>() });
            }
        }
    }
}


