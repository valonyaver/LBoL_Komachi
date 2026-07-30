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
    public sealed class KomachiModTasteOfDeathDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            // Junko 3 (4520) for the "Wind" effect of pulling enemies closer. Possible ids for good slashes are 6162, 7311
            config.GunName = GunNameID.GetGunFromId(6162);
            config.IsPooled = true;
           // config.ImageId = nameof(KomachiModAttackR);

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2, Any = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 15;
            config.UpgradedDamage = 20;

            // Value of the Displacement. Can displace up to +/- value1
            config.Value1 = 5;

            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };

            config.Illustrator = "mixarumixa";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.Unfinished = false;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModTasteOfDeathDef))]
    public sealed class KomachiModTasteOfDeath : KomachiCard
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
                if (target.HasStatusEffect<KomachiModDistanceSe>())
                {
                    KomachiModDistanceSe distance = target.GetStatusEffect<KomachiModDistanceSe>();
                    args.DamageInfo = args.DamageInfo.MultiplyBy(2 / distance.DamageMultiplier);
                    args.AddModifier(this);
                }
                else // Enemy has no status.
                {
                    args.DamageInfo = args.DamageInfo.MultiplyBy(2);
                    args.AddModifier(this);
                }
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // GET OVER HERE
            yield return PerformAction.Gun(Battle.Player, selector.SelectedEnemy, GunNameID.GetGunFromId(4520), 2.2f);
            yield return new DistanceChangeAction(selector.SelectedEnemy, -Value1);
            yield return PerformAction.Animation(Battle.Player, "", shakeLevel: 5);
            yield return base.AttackAction(selector, base.GunName);
            yield break;
        }
    }
}


