using KomachiMod.BattleActions;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.EventManager;
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
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModDetonateInfiniteSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModDetonateInfiniteSeDef))]
    public sealed class KomachiModDetonateInfiniteSe : StatusEffect
    {

        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DetonateVengefulSpiritEventArgs>
                (KomachiEventsManager.DetonatedSpirits, new GameEventHandler<DetonateVengefulSpiritEventArgs>(this.OnDetonatingVengefulSpirits));
        }

        public void OnDetonatingVengefulSpirits(DetonateVengefulSpiritEventArgs args)
        {
            if (args.noFizzle)
            {
                React(new ApplyVengefulSpiritAction(args.Target, Level));
            }
        }
    }
}