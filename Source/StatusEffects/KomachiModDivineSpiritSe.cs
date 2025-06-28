using KomachiMod.BattleActions;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
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

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModDivineSpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = true;
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModDivineSpiritSeDef))]
    public sealed class KomachiModDivineSpiritSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>
                (base.Battle.Player.TurnEnding, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnEnding));
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnding(GameEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd && base.Battle.EnemyGroup.Alives != null)
            {
                base.NotifyActivating();
                yield return new CastBlockShieldAction(Owner, new ShieldInfo(Level, BlockShieldType.Direct), false);
                Level = (int) Math.Floor(Level / 2f);
                if (Level == 0)
                {
                    yield return new RemoveStatusEffectAction(this);
                }
            }
            yield break;
        }
    }
}