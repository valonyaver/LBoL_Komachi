using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModCoinThrowDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(444001);

            // config.ImageId = nameof(KomachiModAttackR);

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.AllEnemies;

            config.Damage = 10;
            config.UpgradedDamage = 13;

            // Money Spent
            config.Value1 = 5;

            // config.Keywords = Keyword.Displace;
            // config.UpgradedKeywords = Keyword.Displace;

            config.Illustrator = "Fujy";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.UpgradedKeywords = Keyword.Echo;

            // config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModCoinThrowDef))]
    public sealed class KomachiModCoinThrow : KomachiCard
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            //Attack all enemies, selector is set to Battle.AllAliveEnemies.
            yield return base.AttackAction(selector, base.GunName);
            yield return new LoseMoneyAction(Value1);
            yield break;
        }
    }
}


