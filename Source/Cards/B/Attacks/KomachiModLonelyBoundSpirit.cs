using KomachiMod.BattleActions;
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
    public sealed class KomachiModLonelyBoundSpiritDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(444003);

            // config.ImageId = nameof(KomachiModAttackB);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Black = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 14;
            config.UpgradedDamage = 18;

            config.Value1 = 1;

            config.RelativeEffects = new List<string>() { nameof(KomachiModLonelyBoundSpiritSe), nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModLonelyBoundSpiritSe), nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword) };


            config.Illustrator = "みえはる";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModLonelyBoundSpiritDef))]
    public sealed class KomachiModLonelyBoundSpirit : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector, base.GunName);
            yield return new ApplyStatusEffectAction<KomachiModLonelyBoundSpiritSe>(selector.SelectedEnemy, Value1);
            yield break;
        }
    }
}


