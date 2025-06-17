using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModVengefulerSweepDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.ImageId = "KomachiAttackB";

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1, Any = 2 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.AllEnemies;

            config.Damage = 15;
            config.UpgradedDamage = 18;

            // Spirits inflicted
            config.Value1 = 4;
            config.UpgradedValue1 = 8;

            // Release cost
            config.Value2 = 5;

            config.RelativeKeyword = Keyword.Accuracy;
            config.UpgradedRelativeKeyword = Keyword.Accuracy;

            config.RelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };


            config.Illustrator = "Wholesome_illustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModVengefulerSweepDef))]
    public sealed class KomachiModVengefulerSweep : KomachiCard
    {
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            foreach (var enemy in Battle.AllAliveEnemies)
            {
                yield return DebuffAction<KomachiModVengefulSpiritSe>(enemy, count: Value1, duration: 3, startAutoDecreasing: true);
            }
            yield break;
        } 
    }
}


