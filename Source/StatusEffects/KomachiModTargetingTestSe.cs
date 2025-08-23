using KomachiMod.BattleActions;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.EventManager;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
using LBoL.Presentation.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModTargetingTestSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModTargetingTestSeDef))]
    public sealed class KomachiModTargetingTestSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            HandleOwnerEvent<DamageDealingEventArgs>(Owner.DamageDealing, OnDamageDealing);
            HandleOwnerEvent<DamageEventArgs>(Owner.DamageGiving, OnDamageDealt);

        }

        void OnDamageDealing(DamageDealingEventArgs args)
        {
            if (args.Targets != null)
            {
                Debug.Log($"The targets before the attack are {args.Targets[0]}");
                args.Targets = new Unit[] { Battle.Player };
                args.GunName = GunNameID.GetGunFromId(7170);
                Debug.Log($"The targets after the attack are {args.Targets[0]}");
            }
        }

        void OnDamageDealt(DamageEventArgs args)
        {
            if (args.Cause != ActionCause.OnlyCalculate && args.Target != null)
            {
                Debug.Log($"The targets before the attackdealt are {args.Target}");
                args.Target = Battle.Player;
                args.GunName = GunNameID.GetGunFromId(7170);
                Debug.Log($"The targets after the attackdealt are {args.Target}");
            }
        }
    }
}