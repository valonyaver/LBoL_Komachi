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
    public sealed class KomachiUltADef : KomachiUltTemplate
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
    [EntityLogic(typeof(KomachiUltADef))]
    public sealed class KomachiUltA : UltimateSkill
    {
        public KomachiUltA()
        {
            base.TargetType = TargetType.AllEnemies;
            base.GunName = GunNameID.GetGunFromId(4158);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            foreach (Unit enemy in selector.GetUnits(base.Battle))
            {
                yield return new DistanceChangeAction(enemy, 1);
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModManDistance>() });
            }
            yield return new DamageAction(base.Owner, selector.GetUnits(base.Battle), this.Damage, base.GunName, GunType.Single);
            yield break;
        }
    }
}
