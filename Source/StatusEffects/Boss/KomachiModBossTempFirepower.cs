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
    public sealed class KomachiModBossTempFirepowerSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            config.RelativeEffects = new List<string>()
            {
                nameof(TempFirepower)
            };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModBossTempFirepowerSeDef))]
    public sealed class KomachiModBossTempFirepowerSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<GameEventArgs>(Battle.RoundStarted, new EventSequencedReactor<GameEventArgs>(this.OnRoundStarted));
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002598 File Offset: 0x00000798
        private IEnumerable<BattleAction> OnRoundStarted(GameEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            base.NotifyActivating();
            // sees how many tokens in hand.
            yield return new ApplyStatusEffectAction<TempFirepower>(Owner, Level, startAutoDecreasing:false);
            yield return new RemoveStatusEffectAction(this);
            yield break;
        }
    }
}