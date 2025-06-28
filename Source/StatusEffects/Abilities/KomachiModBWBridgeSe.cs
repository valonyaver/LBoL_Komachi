using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModBWBridgeSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModBWBridgeSeDef))]
    public sealed class KomachiModBWBridgeSe : StatusEffect
    {
        public ManaGroup Mana
        {
            get
            {
                return ManaGroup.Single(ManaColor.White);
            }
        }
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();
                yield return new GainManaAction(ManaGroup.Whites(base.Level));
            }
            yield break;
        }
    }
}