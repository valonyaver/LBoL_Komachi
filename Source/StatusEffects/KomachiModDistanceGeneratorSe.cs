using KomachiMod.BattleActions;
using KomachiMod.Cards;
using KomachiMod.Patches;
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
    public sealed class KomachiModDistanceGeneratorSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModDistanceGeneratorSeDef))]
    public sealed class KomachiModDistanceGeneratorSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002598 File Offset: 0x00000798
        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            // sees how many tokens in hand.
            List<KomachiModManDistance> list = base.Battle.HandZone.OfType<KomachiModManDistance>().ToList();
            // If status level higher than token amount, add until there are as many.
            if (list.Count < base.Level)
            {
                base.NotifyActivating();
                yield return new AddCardsToHandAction(Library.CreateCards<KomachiModManDistance>(base.Level - list.Count, false), AddCardsType.Normal);
            }
            yield break;
        }
    }
}