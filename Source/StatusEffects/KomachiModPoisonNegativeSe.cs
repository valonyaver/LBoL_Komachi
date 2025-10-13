using KomachiMod.BattleActions;
using KomachiMod.Source.BattleActions.EventManager;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModPoisonNegativeSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(Poison) };
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModPoisonNegativeSeDef))]
    public sealed class KomachiModPoisonNegativeSe : StatusEffect, IOpposing<Poison>
    {
        public OpposeResult Oppose(Poison other)
        {
            var result = other.Level - base.Level;
            if (result <= 0)
            {
                return OpposeResult.Neutralize;
            }
            other.Level = result;
            return OpposeResult.KeepOther;
        }
    }
}