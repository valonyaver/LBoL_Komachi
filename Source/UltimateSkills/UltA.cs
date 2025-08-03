using KomachiMod.BattleActions;
using KomachiMod.Cards;
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
//using KomachiMod.BattleActions;

namespace KomachiMod.KomachiUlt
{
    public sealed class KomachiModUltADef : KomachiUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 12;
            config.Value1 = 1; // Value of distance set.
            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.Keywords = Keyword.Accuracy;
            return config;
        }
    }

    /// <summary>
    /// Shinigami "Higan Retour"
    /// </summary>
    [EntityLogic(typeof(KomachiModUltADef))]
    public sealed class KomachiModUltA : UltimateSkill
    {
        public KomachiModUltA()
        {
            base.TargetType = TargetType.AllEnemies;
            base.GunName = GunNameID.GetGunFromId(4650);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(Owner, nameof(KomachiModUltA));
            foreach (Unit enemy in selector.GetUnits(base.Battle))
            {
                yield return new DistanceChangeAction(enemy, 1 - KomachiModDistanceSe.GetDistanceLevel(enemy));
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModManDistance>() });
            }
            yield return new DamageAction(base.Owner, selector.GetUnits(base.Battle), this.Damage, base.GunName, GunType.Single);
            yield break;
        }
    }
}
