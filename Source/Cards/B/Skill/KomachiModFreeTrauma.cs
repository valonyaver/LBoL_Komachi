using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModFreeTraumaDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Release cost1
            config.Value1 = 4;

            // Release cost2
            config.Value2 = 8;

            config.Mana = new ManaGroup() { Black = 1 };

            config.RelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };


            config.Illustrator = "Wholesome_illustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModFreeTraumaDef))]
    public sealed class KomachiModFreeTrauma : KomachiCard
    {
        protected override int BaseValue3 { get => 3; }
        protected override int BaseUpgradedValue3 { get => 3; }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DebuffAction<KomachiModVengefulSpiritSe>(selector.SelectedEnemy, count: Value3, duration: 3, startAutoDecreasing: true);
            yield return new GainManaAction(Mana);
            yield break;
        } 
    }
}


