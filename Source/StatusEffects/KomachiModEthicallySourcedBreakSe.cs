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
using LBoL.Presentation.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModEthicallySourcedBreakSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            // Level will be the draw amount
            config.HasLevel = false;
            config.HasCount = true;
            config.CountStackType = StackType.Add;
            config.HasDuration = true;
            config.DurationStackType = StackType.Keep;
            config.DurationDecreaseTiming = DurationDecreaseTiming.TurnStart;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModEthicallySourcedBreakSeDef))]
    public sealed class KomachiModEthicallySourcedBreakSe : StatusEffect
    {
        public ManaGroup manaAmount;
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002598 File Offset: 0x00000798
        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd || Duration < 1)
            {
                yield break;
            }
            yield return new RequestEndPlayerTurnAction();
            Battle.Player.GetView<UnitView>().Chat("Mimimimimimimimimimimimimimimimi", 2);
            yield break;
        }

        protected override void OnRemoved(Unit unit)
        {
            if (Battle.BattleShouldEnd) return;
            React(new DrawManyCardAction(Count));
            React(new GainManaAction(manaAmount));
        }
    }
}