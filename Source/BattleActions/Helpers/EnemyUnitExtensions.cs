using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Intentions;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Enemy;
using System;
using System.Collections.Generic;
using System.Text;

namespace KomachiMod.Source.BattleActions.Helpers
{
    public static class EnemyUnitExtensions
    {
        public static int GetCurrentAttackDamage(this EnemyUnit enemy)
        {
            if (enemy?.Battle == null || enemy.Intentions == null)
                return 0;

            int total = 0;

            foreach (Intention intention in enemy.Intentions)
            {
                switch (intention)
                {
                    case AttackIntention attack:
                        total += attack.TotalDamage;
                        break;
                    case ExplodeIntention explode:
                        total += explode.CalculateDamage(explode.Damage);
                        break;
                    case SpellCardIntention spell when spell.Damage.HasValue:
                        DamageInfo damage = spell.Damage.GetValueOrDefault();
                        total += spell.CalculateDamage(damage) * (spell.Times ?? 1);
                        break;
                    case KokoroDarkIntention kokoro:
                        total += kokoro.CalculateDamage(kokoro.Damage);
                        break;
                }
            }

            if (enemy.TryGetStatusEffect<EnemyMaid>(out var kasumi))
            {
                total += kasumi.Level;
            }
			
            return total;
        }

        public static bool HasAnyAttackTypeIntention(this EnemyUnit enemy)
        {
            if (enemy.Intentions == null) return false;

            foreach (Intention intention in enemy.Intentions)
            {
                switch (intention)
                {
                    case AttackIntention attack when attack.Damage.DamageType == DamageType.Attack:
                    case ExplodeIntention explode when explode.Damage.DamageType == DamageType.Attack:
                    case KokoroDarkIntention kokoro when kokoro.Damage.DamageType == DamageType.Attack:
                        return true;
                    case SpellCardIntention spell when spell.Damage.HasValue && spell.Damage.Value.DamageType == DamageType.Attack:
                        return true;
                }
            }

            return false;
        }
    }
}
