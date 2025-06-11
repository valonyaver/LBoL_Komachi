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
using static UnityEngine.UI.CanvasScaler;

namespace KomachiMod.Cards
{
    public sealed class KomachiModEndlessWayDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();

            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            //Hybrid color 7 = B/R
            config.Cost = new ManaGroup() { Red = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 7, Any = 1};
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.SingleEnemy;

            config.Block = 12;
            config.UpgradedBlock = 16;

            config.Value1 = 2;

            config.Illustrator = "Credit_the_artist";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModEndlessWayDef))]
    public sealed class KomachiModEndlessWay : KomachiCard
    {
        //By default, if config.Damage / config.Block / config.Shield are set:
        //The card will deal damage or gain Block/Barrier without having to set anything.
        //Here, this is is equivalent to the following code.
         
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction(true); 
            yield return KomachiDistanceSe.ChangeDistanceLevel(selector.SelectedEnemy, Value1);
            if (selector.SelectedEnemy.HasStatusEffect<Graze>())
            {
                yield return new RemoveStatusEffectAction(selector.SelectedEnemy.GetStatusEffect<Graze>(), true, 0.1f);
            }
        }
    }
}


