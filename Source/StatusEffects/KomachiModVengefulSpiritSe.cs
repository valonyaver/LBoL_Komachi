using KomachiMod.BattleActions;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.EventManager;
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
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModVengefulSpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Negative;
            config.HasLevel = false;
            config.HasCount = true;
            config.CountStackType = StackType.Add;
            config.HasDuration = true;
            config.DurationStackType = StackType.Keep;
            config.DurationDecreaseTiming = DurationDecreaseTiming.TurnStart;
            
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModVengefulSpiritSeDef))]
    public sealed class KomachiModVengefulSpiritSe : StatusEffect
    {
        public string gunName
        {
            get => GunNameID.GetGunFromId(4081);
        }
        public int damageDealtMeasure
        {
            get
            {
                DamageInfo damage = new DamageInfo(Count * 2, DamageType.Attack, isAccuracy: true);
                Unit[] array = new Unit[] { Owner };
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
                    Target = Owner,
                    GunName = gunName,
                    DamageInfo = damage2,
                    ActionSource = this
                };
                Owner?.DamageReceiving.Execute(DamageArgs);

                int finalDamage = (int)DamageArgs.DamageInfo.Damage.Round(MidpointRounding.AwayFromZero);
                return finalDamage;
            }
        }
        public string damageDealt
        {
            get
            {
                int finalDamage = damageDealtMeasure;
                string color = KomachiModUtility.GetColorFromDamage(finalDamage, Count*2);
                return $"<color=#{color}>{finalDamage}</color>";
            }
        }

        protected override void OnRemoved(Unit unit)
        {
            if (Battle.BattleShouldEnd) return;
            base.OnRemoved(unit);
            var detonateEvent = new DetonateVengefulSpiritEventArgs()
            {
                Target = Owner,
                noFizzle = true,
                Effect = this,
                amountDetonated = Count,
                durationAtDetonation = Duration,
                damageDealt = damageDealtMeasure
            };
            var action = new DamageAction(Battle.Player, unit, new DamageInfo(Count * 2, DamageType.Attack, isAccuracy: true), gunName);
            React(action);
            if (Duration == 0)
            {
                KomachiEventsManager.DetonatedSpirits.Execute(detonateEvent);
            }
        }
    }
}