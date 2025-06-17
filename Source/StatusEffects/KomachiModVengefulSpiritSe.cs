using KomachiMod.BattleActions;
using KomachiMod.GunName;
using KomachiMod.Patches;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

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
        public string damageDealt
        {
            get
            {
                return (Count * 2).ToString();
            }
        }
        protected override void OnRemoved(Unit unit)
        {
            React(new DamageAction(Battle.Player, unit, new DamageInfo(Count * 2, DamageType.Reaction, false), GunNameID.GetGunFromId(4081)));
        }
    }
}