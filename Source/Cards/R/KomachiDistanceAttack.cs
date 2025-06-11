using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Patches;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModDistanceAttackDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 6;
            config.UpgradedDamage = 9;

            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;

            config.Illustrator = "@TheIllustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.RelativeEffects = new List<string>() { nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDistanceKeyword), nameof(Graze) };
            config.Unfinished = true;
            return config;
        }
    }
    
    // THIS is broken for some reason
    [EntityLogic(typeof(KomachiModDistanceAttackDef))]
    public sealed class KomachiModDistanceAttack : KomachiCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<DistanceChangedEventArgs>
                (CustomGameEventManager.DistanceChanged, new GameEventHandler<DistanceChangedEventArgs>(this.OnEnemyDistanceChange));
        }

        private void OnEnemyDistanceChange(DistanceChangedEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                return;
            }
            if (args.Effect.GetType() == typeof(KomachiDistanceSe) && base.Zone == CardZone.Discard && base.Battle.HandIsNotFull)
            {
                React(new MoveCardAction(this, CardZone.Hand));
            }
        }

        //protected override void OnEnterBattle(BattleController battle)
        //{
        //    foreach (var enemy in battle.EnemyGroup)
        //    {
        //        // For when the distance is first applied.
        //        base.ReactBattleEvent<StatusEffectApplyEventArgs>
        //            (enemy.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnEnemyDistanceAdd));
        //        // For when the distance is changed.
        //        base.HandleBattleEvent<StatusEffectEventArgs>
        //            (enemy.StatusEffectChanged, new GameEventHandler<StatusEffectEventArgs>(this.OnEnemyDistanceChange));
        //    }
        //    base.HandleBattleEvent<UnitEventArgs>
        //            (base.Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(this.OnEnemySpawn));
        //}
        //private void OnEnemySpawn(UnitEventArgs args)
        //{
        //    // For when the distance is first applied.
        //    base.ReactBattleEvent<StatusEffectApplyEventArgs>
        //        (args.Unit.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnEnemyDistanceAdd));
        //    // For when the distance is changed.
        //    base.HandleBattleEvent<StatusEffectEventArgs>
        //            (args.Unit.StatusEffectChanged, new GameEventHandler<StatusEffectEventArgs>(this.OnEnemyDistanceChange));
        //}

        //// Same function
        private IEnumerable<BattleAction> OnEnemyDistanceAdd(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (args.Effect.GetType() == typeof(KomachiDistanceSe) && base.Zone == CardZone.Discard && base.Battle.HandIsNotFull)
            {
                yield return new MoveCardAction(this, CardZone.Hand);
            }
            yield break;
        }

        private void OnEnemyDistanceChange(StatusEffectEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                return;
            }
            if (args.Effect.GetType() == typeof(KomachiDistanceSe) && base.Zone == CardZone.Discard && base.Battle.HandIsNotFull)
            {
                React(new MoveCardAction(this, CardZone.Hand));
            }
        }
    }
}


