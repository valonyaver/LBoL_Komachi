using KomachiMod.BattleActions;
using KomachiMod.GunName;
using KomachiMod.Patches;
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
using System.Collections.Generic;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModGuidingSpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModGuidingSpiritSeDef))]
    public sealed class KomachiModGuidingSpiritSe : StatusEffect
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
                EnemyUnit enemyUnit = base.Battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp);
                yield return new 
                    DamageAction(base.Owner, enemyUnit, DamageInfo.Reaction(base.Level, false), GunNameID.GetGunFromId(6061));
                int num = base.Level - 1;
                base.Level = num;
                if (base.Level == 0)
                {
                    yield return new RemoveStatusEffectAction(this, true, 0.1f);
                }
            }
            yield break;
        }
    }
}