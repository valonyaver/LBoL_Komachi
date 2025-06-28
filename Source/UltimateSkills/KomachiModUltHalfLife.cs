using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.GunName;
using LBoL.Core.StatusEffects;

namespace KomachiMod.KomachiUlt
{
    public sealed class KomachiModUltHalfLifeDef : KomachiUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 1;

            // Add the relative status effects in the description box.   
            config.RelativeEffects = new List<string>() { nameof(Weak) };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModUltHalfLifeDef))]
    public sealed class KomachiModUltHalfLife : UltimateSkill
    {
        public KomachiModUltHalfLife()
        {
            base.TargetType = TargetType.SingleEnemy;
            base.GunName = GunNameID.GetGunFromId(13201);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield break;
        }
    }
}
