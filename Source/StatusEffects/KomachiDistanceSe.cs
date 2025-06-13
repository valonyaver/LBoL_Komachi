using Cysharp.Threading.Tasks.Triggers;
using KomachiMod.BattleActions;
using KomachiMod.Patches;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiDistanceSeDef : KomachiStatusEffectTemplate
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

    [EntityLogic(typeof(KomachiDistanceSeDef))]
    public sealed class KomachiDistanceSe : StatusEffect
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
                            return "Very close";
                        }
                    case 2: // Close distance
                        {
                            return "Close";
                        }
                    default: // Normal Distance. Takes case 3 into account.
                        {
                            return "Normal Distance";
                        }
                    case 4: // Far distance
                        {
                            return "Far";
                        }
                    case 5: // Very far distance
                        {
                            return "Very far";
                        }
                }
            }
        }
        public float DamageMultiplier
        {
            get
            {
                switch (Level)
                {
                    case 1: // Very close distance
                        {
                            return 2;
                        }
                    case 2: // Close distance
                        {
                            return 1.5f;
                        }
                    default: // Normal Distance. Takes case 3 into account.
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
                }
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
                    return $"increased by {percentage:0}%";
                }
                else if (DamageMultiplier < 1)
                {
                    return $"reduced by {-percentage:0}%"; // (Quick effect); Negate to avoid double negative ("-30%" to "Reduced by 30%")
                }
                else
                {
                    return "unchanged";
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
            if (damageInfo.DamageType == DamageType.Attack)
            {
                damageInfo.Damage = damageInfo.Amount * DamageMultiplier;
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
            KomachiDistanceSe distance;
            target.TryGetStatusEffect(out distance);
            if (distance == null)
            {
                return 3;
            }
            else return distance.Level;
        }
    }
}
