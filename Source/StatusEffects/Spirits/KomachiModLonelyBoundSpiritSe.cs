using KomachiMod.BattleActions;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.Source.Patching;
using KomachiMod.StatusEffects;
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

namespace KomachiMod.Source.StatusEffects.Spirits
{
    public sealed class KomachiModLonelyBoundSpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Negative;
            config.HasLevel = true;
            config.CountStackType = StackType.Add;
            config.HasCount = true;
            config.HasDuration = false;
            
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModLonelyBoundSpiritSeDef))]
    public sealed class KomachiModLonelyBoundSpiritSe : StatusEffect, IHasTopLeftText
    {
        // Other possible candidate is yuyuko shoot 2 (id 4003)
        public string gunName
        {
            get => GunNameID.GetGunFromId(960);
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
                string color = KomachiModUtility.GetColorFromDamage(finalDamage, Count * 2);
                return $"<color=#{color}>{finalDamage}</color>";
            }
        }
        public string TopLeftText => damageDealt;
        public string OwnerName
        {
            get => Owner.SelfName;
        }

        protected override void OnAdded(Unit unit)
        {
            HandleOwnerEvent
                (KomachiEventsManager.AppliedVengefulSpirit, new GameEventHandler<ApplyVengefulSpiritEventArgs>(OnApplyVengefulSpirit));
            HandleOwnerEvent
                (KomachiEventsManager.DetonatedSpirits, new GameEventHandler<DetonateVengefulSpiritEventArgs>(OnDetonatingVengefulSpirits), GameEventPriority.Highest);
        }


        public void OnApplyVengefulSpirit(ApplyVengefulSpiritEventArgs args)
        {
            if (args.Target != Owner) return;
            NotifyChanged();
            if (args.successful)
            {
                Count += args.Amount * Level;
            }
        }

        public void OnDetonatingVengefulSpirits(DetonateVengefulSpiritEventArgs args)
        {
            if (args.Target == Owner && args.detonatedByEffect && args.Effect.GetType() == typeof(KomachiModVengefulSpiritSe))
            {
                args.amountDetonated += Count;
                var action = new RemoveStatusEffectAction(this);
                React(action); 
            }
        }


        protected override void OnRemoved(Unit unit)
        {
            if (Battle.BattleShouldEnd) return;
            var detonateEvent = new DetonateVengefulSpiritEventArgs()
            {
                Target = Owner,
                noFizzle = true,
                Effect = this,
                amountDetonated = Count,
                durationAtDetonation = 0,
                damageDealt = damageDealtMeasure
            };
            React(new DamageAction(Battle.Player, unit, new DamageInfo(Count * 2, DamageType.Attack, isAccuracy:true), gunName));
            KomachiEventsManager.DetonatedSpirits.Execute(detonateEvent);
        }
    }
}