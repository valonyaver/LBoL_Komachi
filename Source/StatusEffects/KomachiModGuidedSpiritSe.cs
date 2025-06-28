using KomachiMod.BattleActions;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModGuidedSpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = true;
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModGuidedSpiritSeDef))]
    public sealed class KomachiModGuidedSpiritSe : StatusEffect
    {
        public string gunName
        {
            get => GunNameID.GetGunFromId(6061);
        }
        /// <summary>
        /// Calls both damage dealing of player and damage receiving of target to get the correct damage on the UI.
        /// </summary>
        public string damageDealt
        {
            get
            {
                DamageInfo damage = new DamageInfo(Level, DamageType.Attack, isAccuracy: true);
                EnemyUnit enemyUnit = target;
                Unit[] array = new Unit[] { enemyUnit };
                DamageDealingEventArgs DealingArgs = new DamageDealingEventArgs
                {
                    Source = Battle.Player,
                    Targets = array,
                    GunName = gunName,
                    DamageInfo = damage,
                    ActionSource = this
                };
                Battle.Player?.DamageDealing.Execute(DealingArgs);
                DamageInfo damage2 = DealingArgs.DamageInfo;
                DamageEventArgs DamageArgs = new DamageEventArgs
                {
                    Source = Battle.Player,
                    Target = enemyUnit,
                    GunName = gunName,
                    DamageInfo = damage2,
                    ActionSource = this
                };
                enemyUnit?.DamageReceiving.Execute(DamageArgs);

                int finalDamage = (int)DamageArgs.DamageInfo.Damage.Round(MidpointRounding.AwayFromZero);

                string color = KomachiModUtility.GetColorFromDamage(finalDamage, Level);

                return $"<color=#{color}>{finalDamage}</color>"; 
            }
        }

        public EnemyUnit target
        {
            get => base.Battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp);
        }
         
        public string targetName
        {
            get => target.SelfName;
        }
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>
                (base.Battle.Player.TurnEnding, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnEnding));
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnding(GameEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd && base.Battle.EnemyGroup.Alives != null)
            {
                base.NotifyActivating();
                EnemyUnit enemyUnit = target;
                yield return new 
                    DamageAction(base.Owner, enemyUnit, DamageInfo.Attack(base.Level, isAccuracy:true), gunName);
                int num = base.Level - 1;
                base.Level = num;
                if (base.Level == 0)
                {
                    yield return new RemoveStatusEffectAction(this, true, 0.1f);
                }
            }
            yield break;
        }
    }
}