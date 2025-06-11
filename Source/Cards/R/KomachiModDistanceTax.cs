using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using KomachiMod.StatusEffects;

namespace KomachiMod.Cards
{
    public sealed class KomachiModDistanceTaxDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.AllEnemies;

            // Apply weak
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            // Apply lock on and temp firepower down
            config.Value2 = 2;
            config.UpgradedValue2 = 4;

            //Add Weak, Vulnerable, Firepower Down descrption when hovering over the card.
            config.RelativeEffects = new List<string>() { nameof(KomachiDistanceKeyword), nameof(Weak), nameof(FirepowerNegative), nameof(LockedOn), nameof(Vulnerable)  };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDistanceKeyword), nameof(Weak), nameof(FirepowerNegative), nameof(LockedOn), nameof(Vulnerable),  };

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModDistanceTaxDef))]
    public sealed class KomachiModDistanceTax : KomachiCard
    {
        // Apply vulnerable
        protected override int BaseValue3 { get => 1; }
        protected override int BaseUpgradedValue3 { get => 2; }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            //Buff and debuff have either a level or a duration.
            //Duration: Effects that last a certain amount of turns then disappear.
            //Level: Effects that have a fixed duration but that can vary in intensity. 
            //Weak and Vulnerable have a duration, FirepowerNegative has a level.  
            //DebuffAction's 2nd field is the level, the 3rd one is the duration.
            KomachiDistanceSe enemyDistance;

            foreach (var selectedEnemy in base.Battle.AllAliveEnemies)
            {
                selectedEnemy.TryGetStatusEffect(out enemyDistance);
                if (enemyDistance == null)
                {
                    yield return base.DebuffAction<TempFirepowerNegative>(selectedEnemy, base.Value2, 0, 0, 0, true, 0.2f);
                    yield return base.DebuffAction<LockedOn>(selectedEnemy, base.Value2, 0, 0, 0, true, 0.2f);
                }
                switch(enemyDistance.Level)
                {
                    case 1:
                    case 2:
                        yield return base.DebuffAction<Weak>(selectedEnemy, 0, base.Value1, 0, 0, true, 0.2f);
                        break;
                    case 4:
                    case 5:
                        yield return base.DebuffAction<Vulnerable>(selectedEnemy, 0, base.Value3, 0, 0, true, 0.2f);
                        break;
                    default:
                        yield return base.DebuffAction<TempFirepowerNegative>(selectedEnemy, base.Value2, 0, 0, 0, true, 0.2f);
                        yield return base.DebuffAction<LockedOn>(selectedEnemy, base.Value2, 0, 0, 0, true, 0.2f);
                        break;
                }
            }
            yield break;
        }
    }
}


