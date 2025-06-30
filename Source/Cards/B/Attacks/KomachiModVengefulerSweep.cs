using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
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

            config.Damage = 14;
            config.UpgradedDamage = 18;

            // Spirits inflicted
            config.Value1 = 5;
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
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, Value2);
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2);
        }
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card releaseChoice = KomachiModUtility.GetPreconditionCard(precondition);
            if (releaseChoice == null || releaseChoice.GetType() == typeof(KomachiModReleaseNone))
            {
                yield return AttackAction(selector);
            }
            else
            {
                yield return new KomachiReleaseAction(Battle.Player, Value2);
                yield return AttackAction(selector, new DamageInfo(Damage.Damage, DamageType.Attack, isAccuracy: true));
            }
            foreach (var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(enemy, Value1);
            }
            yield break;
        } 
    }
}


