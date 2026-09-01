using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.Source.StatusEffects.Spirits;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Source.Cards
{
    public sealed class KomachiModBoundSpiritsOfEarthDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Black = 1, Any = 2 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Immediate spirits inflicted
            config.Value1 = 10;
            config.UpgradedValue1 = 13;

            // Level of buff gained
            config.Value2 = 4;
            config.UpgradedValue2 = 6;

            config.RelativeEffects = new List<string>()
            { 
                nameof(KomachiModVengefulSpiritSe)
            };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiModVengefulSpiritSe)};

            config.Illustrator = "市葉葉市";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModBoundSpiritsOfEarthDef))]
    public sealed class KomachiModBoundSpiritsOfEarth : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach(var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(this, enemy, Value1);
                if (enemy.TryGetStatusEffect<KomachiModVengefulSpiritSe>(out var spirits))
                {
                    spirits.Duration = 1;
                }
            }


            yield break;
        }
    }
}


