using KomachiMod.BattleActions;
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
using UnityEngine;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModBossReviveSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            config.HasLevel = false;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModBossReviveSeDef))]
    public sealed class KomachiModBossReviveSe : StatusEffect
    {
        bool revived = false;
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DieEventArgs>(base.Owner.Dying, new GameEventHandler<DieEventArgs>(this.OnDying));
        }

        private void OnDying(DieEventArgs args)
        {
            if (!revived)
            {
                base.NotifyActivating();
                Owner.Hp = Owner.MaxHp;
                args.CancelBy(this);
                revived = true;
                Highlight = true;
                if (base.GameRun.Battle != null)
                {
                    this.React(new ApplyStatusEffectAction<InvincibleEternal>(base.Owner));
                }
            }
        }

        protected override string GetBaseDescription()
        {
            if (!revived) return BaseDescription;
            else return ExtraDescription;
        }
    }
}