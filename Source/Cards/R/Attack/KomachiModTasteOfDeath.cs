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
            config.GunName = GunNameID.GetGunFromId(400);
            config.IsPooled = true;

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Red = 1, Any = 2 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 16;
            config.UpgradedDamage = 20;

            // Value of the Displacement. Can displace up to +/- value1
            config.Value1 = 5;

            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };

            config.Illustrator = "@TheIllustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModTasteOfDeathDef))]
    public sealed class KomachiModTasteOfDeath : KomachiCard
    {
        //By default, if config.Damage / config.Block / config.Shield are set:
        //The card will deal damage or gain Block/Barrier without having to set anything.
        //Here, this is is equivalent to the following code.

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
                if (target.HasStatusEffect<KomachiDistanceSe>())
                {
                    KomachiDistanceSe distance = target.GetStatusEffect<KomachiDistanceSe>();
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
            yield return new DistanceChangeAction(selector.SelectedEnemy, -Value1);
            yield return base.AttackAction(selector, base.GunName);
            yield break;
        }
    }
}


