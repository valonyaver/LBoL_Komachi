using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    /// <summary>
    /// Unused card. Used for early testing of the distance mechanic.
    /// </summary>
    public sealed class KomachiModScytheFinalJudgmentDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            // Other options are 4610, 4051, 6300, 39073, 15111
            // Honorable for other attacks. Putting them here for convenience, 25010
            // Really want something that comes from above.
            config.GunName = GunNameID.GetGunFromId(4660);
            config.IsPooled = true;

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2, Any = 2 };
            config.UpgradedCost = new ManaGroup() { Red = 1, Any = 2 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 30;

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };

            config.Illustrator = "@TheIllustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };

            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModScytheFinalJudgmentDef))]
    public sealed class KomachiModScytheFinalJudgment : KomachiCard
    {
        //By default, if config.Damage / config.Block / config.Shield are set:
        //The card will deal damage or gain Block/Barrier without having to set anything.
        //Here, this is is equivalent to the following code.

        //public int realDamage
        //{
        //    get
        //    {
        //        if (base.GameRun != null && base.Battle != null)
        //        {
        //            KomachiDistanceSe distance;
        //            switch (distance.Level)
        //            {
        //                case 4:
        //                    return (int) (Damage.Damage / distance.DamageMultiplier);
        //                case 5:
        //                    return (int) (Damage.Damage / distance.DamageMultiplier);
        //            }
        //        }
        //    }
        //}

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
                    switch (distance.Level)
                    {
                        case 4:
                        case 5:
                            // 30 * (2-0.7)/0.7 would become 30*1.3/0.7 and since the distance would multiply by 0.7 it becomes 30*1.3 *0.7/0.7
                            args.DamageInfo = args.DamageInfo.MultiplyBy((2 - distance.DamageMultiplier) / distance.DamageMultiplier);
                            args.AddModifier(this);
                            break;
                    }
                }
            }
        }


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector.SelectedEnemy);

            if (selector.SelectedEnemy.IsDead)
            {
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModSpiderLily>(IsUpgraded) });
            }
            yield break;
        }
    }
}


