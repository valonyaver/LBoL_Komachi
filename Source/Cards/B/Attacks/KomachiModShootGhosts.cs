using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModShootGhostsDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(7081);

            // config.ImageId = nameof(KomachiModAttackB);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2, Any = 1};
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 20;
            config.UpgradedDamage = 25;

            config.Value1 = 3;
            config.UpgradedValue1 = 4;

            config.Keywords = Keyword.Accuracy | Keyword.Retain | Keyword.Exile;
            config.UpgradedKeywords = Keyword.Accuracy | Keyword.Retain | Keyword.Exile;

            config.RelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };


            config.Illustrator = "yudaoshan";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModShootGhostsDef))]
    public sealed class KomachiModShootGhosts : KomachiCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<DamageDealingEventArgs>
                (base.Battle.Player.DamageDealing,
                new GameEventHandler<DamageDealingEventArgs>(this.OnPlayerDamageDealing), GameEventPriority.ConfigDefault);
        }

        /// <summary>
        /// This will fix the attack damage on the card when targeting a far enemy.
        /// </summary>
        /// <param name="args"></param>
        private void OnPlayerDamageDealing(DamageDealingEventArgs args)
        {
            if (args.ActionSource == this && args.Targets != null)
            {
                Unit target = args.Targets[0];
                int damageIncrease = KomachiModUtility.GetVengefulCount(target);
                if (damageIncrease > 0)
                {
                    args.DamageInfo = args.DamageInfo.IncreaseBy(Value1 * damageIncrease);
                    args.AddModifier(this);
                }
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            selector.SelectedEnemy.TryGetStatusEffect<KomachiModVengefulSpiritSe>(out var spirits);
            int spiritCount = KomachiModUtility.GetVengefulCount(selector.SelectedEnemy);
            yield return base.AttackAction(selector, base.GunName);
            if (spirits != null && selector.SelectedEnemy.IsDead && !Battle.BattleShouldEnd)
            {
                foreach(var enemy in Battle.AllAliveEnemies)
                {
                    if (enemy != selector.SelectedEnemy)
                    {
                        yield return new ApplyVengefulSpiritAction(enemy, spiritCount);
                    }
                }
            }
            yield break;
        } 
    }
}


