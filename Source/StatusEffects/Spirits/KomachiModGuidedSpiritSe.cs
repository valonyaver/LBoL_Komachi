using KomachiMod.BattleActions;
using KomachiMod.GunName;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Source.StatusEffects.Spirits
{
    public sealed class KomachiModGuidedSpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = true;
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModGuidedSpiritSeDef))]
    public sealed class KomachiModGuidedSpiritSe : StatusEffect, IHasTopLeftText
    {
        public static string gunName
        {
            get => GunNameID.GetGunFromId(6061);
        }

        public string EnemyDescription => LocalizeProperty("EnemyDescription");
        protected override string GetBaseDescription()
        {
            if (Owner is PlayerUnit) return BaseDescription;
            return EnemyDescription;
        }

        /// <summary>
        /// Calls both damage dealing of player and damage receiving of target to get the correct damage on the UI.
        /// </summary>
        public string damageDealt
        {
            get
            {
                int calculatedDamage = Battle.CalculateDamage(this, Owner, target, new DamageInfo(Level, DamageType.Attack, isAccuracy: isAccurate));
                if (target == null) return 0.ToString();
                string color = KomachiModUtility.GetColorFromDamage(calculatedDamage, Level);
                return $"<color=#{color}>{calculatedDamage}</color>"; 
            }
        }

        public string TopLeftText => damageDealt;

        public bool isAccurate
        {
            get
            {
                if (Owner is PlayerUnit) return true;
                return false;
            }
        }

        public Unit target
        {
            get
            {
                if (Battle.BattleShouldEnd) return null;
                if (Owner is PlayerUnit) return Battle.EnemyGroup.Alives.MinBy((unit) => unit.Hp);
                else
                {
                    return Battle.Player;
                }
            }
        }
         
        public string targetName
        {
            get => target.SelfName;
        }


        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent
                (Owner.TurnEnding, new EventSequencedReactor<UnitEventArgs>(OnUnitTurnEnding));
        }

        private IEnumerable<BattleAction> OnUnitTurnEnding(GameEventArgs args)
        {
            if (!Battle.BattleShouldEnd && Battle.EnemyGroup.Alives != null && Owner == Battle.Player)
            {
                NotifyActivating();
                // Get the target
                Unit unit = target;
                // Shoot damaage
                yield return new 
                    DamageAction(Owner, unit, DamageInfo.Attack(Level, isAccuracy:isAccurate), gunName);
                // Reduce level and remove it if 0
                int num = Level - 1;
                Level = num;
                if (Level == 0)
                {
                    yield return new RemoveStatusEffectAction(this, true, 0.1f);
                }
            }
            yield break;
        }
    }
}