using KomachiMod.BattleActions;
using KomachiMod.Cards;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModDisplaceNextTurnSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            config.LevelStackType = StackType.Keep;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModDisplaceNextTurnSeDef))]
    public sealed class KomachiModDisplaceNextTurnSe : StatusEffect
    {
        public int displaceAmount;

        protected override void OnAdding(Unit unit)
        {
            displaceAmount = Level;
            Level = 1;
        }
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
        }

        public override bool Stack(StatusEffect other)
        {
            displaceAmount += other.Level;
            return base.Stack(other);
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002598 File Offset: 0x00000798
        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            yield return new DistanceChangeAction(Owner, displaceAmount);
            yield return new RemoveStatusEffectAction(this);
            yield break;
        }
    }
}