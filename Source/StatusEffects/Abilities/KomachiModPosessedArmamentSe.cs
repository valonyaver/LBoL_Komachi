using KomachiMod.BattleActions;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.TwoColor;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModPosessedArmamentSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModPosessedArmamentSeDef))]
    public sealed class KomachiModPosessedArmamentSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent(base.Owner.DamageDealt, OnDamageDealt);
        }

        public IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd && args.Target.IsAlive)
            {
                DamageInfo damageInfo = args.DamageInfo;
                if (damageInfo.DamageType == DamageType.Attack && damageInfo.Damage > 0f)
                {
                    NotifyActivating();
                    yield return new ApplyVengefulSpiritAction(this, args.Target, base.Level);
                }
            }
        }
    }
}