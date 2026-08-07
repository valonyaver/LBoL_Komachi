using KomachiMod.BattleActions;
using KomachiMod.Source.StatusEffects.Spirits;
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
    public sealed class KomachiModBoundSpiritsOfEarthSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModBoundSpiritsOfEarthSeDef))]
    public sealed class KomachiModBoundSpiritsOfEarthSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnEnded, new EventSequencedReactor<UnitEventArgs>(this.OnTurnEnded));
        }

        private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();
                foreach(var enemy in Battle.AllAliveEnemies)
                {
                    if (enemy.HasStatusEffect(typeof(KomachiModVengefulSpiritSe)))
                    {
                        yield return new ApplyVengefulSpiritAction(this, enemy, Level);
                    }
                }
            }
            yield break;
        }
    }
}