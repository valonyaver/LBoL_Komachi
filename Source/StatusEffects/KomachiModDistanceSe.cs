using Cysharp.Threading.Tasks.Triggers;
using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using LBoL.Base;
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
using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModDistanceSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = new StatusEffectConfig(
                Id: "",
                ImageId: null,
                Index: 0,
                Order: 10,
                Type: StatusEffectType.Special,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Overwrite,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModDistanceSeDef))]
    public sealed class KomachiModDistanceSe : StatusEffect
    {
        public int maxDistance = 5;
        public int minDistance = 1;
        public string DistanceString
        {
            get
            {
                switch (Level)
                {
                    case 1: // Very close distance
                        {
                            return VeryCloseDistance;
                        }
                    case 2: // Close distance
                        {
                            return CloseDistance;
                        }
                    default: // Normal Distance. Takes case 3 into account.
                        {
                            return NormalDistance;
                        }
                    case 4: // Far distance
                        {
                            return FarDistance;
                        }
                    case 5: // Very far distance
                        {
                            return VeryFarDistance;
                        }
                }
            }
        }

        public string VeryCloseDistance => LocalizeProperty("VeryClose");
        public string CloseDistance => LocalizeProperty("Close");
        public string NormalDistance => LocalizeProperty("Normal");
        public string FarDistance => LocalizeProperty("Far");
        public string VeryFarDistance => LocalizeProperty("VeryFar");

        public float DamageMultiplier
        {
            get
            {

                switch (Level)
                {
                    case 1: // Very close distance
                        return BepinexPlugin.distanceMultiplier1.Value;
                    case 2: // Close distance
                        return BepinexPlugin.distanceMultiplier2.Value;
                    case 3: // Normal distance
                        return BepinexPlugin.distanceMultiplier3.Value;
                    case 4: // Far distance
                        return BepinexPlugin.distanceMultiplier4.Value;
                    case 5: // Very far distance
                        return BepinexPlugin.distanceMultiplier5.Value;
                    default: // Fallback
                        return BepinexPlugin.distanceMultiplier3.Value; // Use normal distance as default
                }
            }
        }
        /// <summary>
        /// Returns double the inverse of the far distance multiplier. 0.85 becomes 1.3, 0.7 becomes 1.6
        /// </summary>
        /// <param name="distanceLevel"></param>
        /// <returns></returns>
        public static float GetInverseDistanceMultiplier(int distanceLevel)
        {
            switch (distanceLevel)
            {
                case 4:
                case 5:
                    // 1 - 0.7 becomes 0.3, multiply by 2 becomes 0.6. Add it to 1 it becomes 1.6.
                    float distanceMultiplier = KomachiModDistanceSe.GetDistanceDamageMultiplier(distanceLevel);
                    float inverseMultiplier = (1 + (1 - distanceMultiplier) * 2);
                    return inverseMultiplier;
                default:
                    Debug.LogWarning("You are trying to use the inverse distance multiplier on an enemy whose distance isn't far.");
                    return GetDistanceDamageMultiplier(distanceLevel);
            }
        }

        public string MultiplierPercentage // unused
        {
            get
            {
                float percentage = (DamageMultiplier - 1) * 100;
                if (percentage == 0) return "0%";
                string sign = percentage > 0 ? "+" : "";
                return $"{sign}{percentage:0}%";
                // Return "-30%", "-15%", "0%", "+50%", "+100%"
            }
        }

        public string MultiplierDescription // used for the description of the status
        {
            get
            {
                float percentage = (DamageMultiplier - 1) * 100;

                if (DamageMultiplier > 1)
                {
                    return $"+{percentage:0}%";
                }
                else if (DamageMultiplier < 1)
                {
                    return $"−{-percentage:0}%"; // (Quick effect); Negate to avoid double negative ("-30%" to "Reduced by 30%")
                }
                else
                {
                    return "+0%";
                }
            }
        }

        /// <summary>
        /// Happens when the status is first added to a unit, before it is added.
        /// </summary>
        /// <param name="unit"></param>
        protected override void OnAdding(Unit unit)
        {
            Debug.Log($"On Adding before clamping is happening. Current distance is {Level}");
            ClampLevel();
            Debug.Log($"On Adding is happening. Current distance is {Level}");
        }

        /// <summary>
        /// Happens when the status is first added to a unit, after it is added.
        /// </summary>
        /// <param name="unit"></param>
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DamageEventArgs>(unit.DamageReceiving, new GameEventHandler<DamageEventArgs>(this.OnDamageReceiving));
            base.HandleOwnerEvent<DamageDealingEventArgs>(unit.DamageDealing, new GameEventHandler<DamageDealingEventArgs>(this.OnDamageDealing));
            ClampLevel();
            Debug.Log($"On Added is happening. Clamped Level to (1, 5). Current distance is {Level}");
        }

        private void OnDamageReceiving(DamageEventArgs args)
        {
            DamageInfo damageInfo = args.DamageInfo;
            float damageReceiveMultiplier = DamageMultiplier;
            if (args.ActionSource is KomachiCard)
            {
                var cardSource = args.ActionSource as KomachiCard;
                if (Level > 3 && cardSource.farDistanceInverseDamage)
                {
                    damageReceiveMultiplier = GetInverseDistanceMultiplier(Level);
                }
            }
            
            if (damageInfo.DamageType == DamageType.Attack)
            {
                damageInfo.Damage = damageInfo.Amount * damageReceiveMultiplier;
                args.DamageInfo = damageInfo;
                args.AddModifier(this);
            }
        }

        private void OnDamageDealing(DamageDealingEventArgs args)
        {
            DamageInfo damageInfo = args.DamageInfo;
            if (damageInfo.DamageType == DamageType.Attack)
            {
                damageInfo.Damage = damageInfo.Amount * DamageMultiplier;
                args.DamageInfo = damageInfo;
                args.AddModifier(this);
            }
        }

        /// <summary>
        /// Happens when the status is added to a unit that already has it. Will clamp the status.
        /// Unused because everything happens through DistanceChangeAction
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        //public override bool Stack(StatusEffect other)
        //{
        //    _level += other.Level;
        //    ClampLevel();
        //    Debug.Log($"Stack is happening. Changing distance. Final distance is {Level}");
        //    NotifyChanged();
        //    return true;
        //}

        public int ClampLevel()
        {
            int result = Math.Clamp(_level, minDistance, maxDistance);
            _level = result;
            return Level;
        }

        public static IEnumerable<BattleAction> ChangeDistanceLevel(IEnumerable<Unit> targets, int levelChange)
        {
            foreach (Unit target in targets)
            {
                yield return new DistanceChangeAction(target, levelChange);
            }
        }

        /// <summary>
        /// Use this to get the distance level of a target. Returns 3 if it has no status.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int GetDistanceLevel(Unit target)
        {
            KomachiModDistanceSe distance;
            target.TryGetStatusEffect(out distance);
            if (distance == null)
            {
                return 3;
            }
            else return distance.Level;
        }

        public static float GetDistanceDamageMultiplier(int level)
        {
            switch (level)
            {
                case 1: // Very close distance
                    {
                        return 2;
                    }
                case 2: // Close distance
                    {
                        return 1.5f;
                    }
                case 3: // Normal Distance. Takes case 3 into account.
                    {
                        return 1;
                    }
                case 4: // Far distance
                    {
                        return 0.85f;
                    }
                case 5: // Very far distance
                    {
                        return 0.7f;
                    }
                default:
                    {
                        Debug.LogError($"Error in the function GetDistanceDamageMultiplier. The argument level {level} is not between 1 and 5.");
                        return 1;
                    }
            }
        }
    }
}
