using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using KomachiMod.StatusEffects;
using LBoL.Core.Units;
using KomachiMod.BattleActions;
using UnityEngine;

namespace KomachiMod.Cards
{
    /// <summary>
    /// Unused card. Used for early testing of the distance mechanic.
    /// </summary>
    public sealed class KomachiModStygianReelDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            // Junko 3 (4520) for the "Wind" effect of pulling enemies closer. Possible ids for good slashes are 6162, 7311
            config.GunName = GunNameID.GetGunFromId(13130);
            //config.IsPooled = false;
            //config.HideMesuem = true;
           // config.ImageId = nameof(KomachiModAttackR);

            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Blue };
            config.Cost = new ManaGroup() { Red = 1, Blue = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 10;
            config.UpgradedDamage = 12;

            // Value of the Displacement.
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };

            config.Illustrator = "";
            config.Unfinished = true;

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.Unfinished = false;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModStygianReelDef))]
    public sealed class KomachiModStygianReel : KomachiCard
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
            yield return new DistanceChangeAction(selector.SelectedEnemy, -Value1);
            yield return DebuffAction<KomachiModDisplaceNextTurnSe>(selector.SelectedEnemy, Value1);
            yield break;
        }
    }
}


