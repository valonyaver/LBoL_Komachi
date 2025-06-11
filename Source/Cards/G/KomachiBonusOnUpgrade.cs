using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;

namespace KomachiMod.Cards
{
    public sealed class KomachiBonusOnUpgradeDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Green };
            config.Cost = new ManaGroup() { Any = 2, Green = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 20;
            config.UpgradedDamage = 25;

            config.Value1 = 0;
            config.UpgradedValue1 = 2;

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiBonusOnUpgradeDef))]
    public sealed class KomachiBonusOnUpgrade : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector, base.GunName);
            //Only gain the additional effect if battle wouldn't end and the card is upgraded.
            if (!base.Battle.BattleShouldEnd && base.IsUpgraded)
            {
                yield return new DrawManyCardAction(base.Value1);
            }
            yield break;
        }
    }
}


