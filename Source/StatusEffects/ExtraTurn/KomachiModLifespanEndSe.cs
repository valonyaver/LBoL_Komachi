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
    public sealed class KomachiModLifespanEndSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Special;
            config.HasCount = true;
            config.CountStackType = StackType.Add;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModLifespanEndSeDef))]
    public sealed class KomachiModLifespanEndSe : StatusEffect
    {
        public ManaGroup manaAmount = new ManaGroup();
        public bool startOfExtraTurn;
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
            base.ReactOwnerEvent<GameEventArgs>(base.Battle.RoundEnded, new EventSequencedReactor<GameEventArgs>(this.OnRoundEnded), GameEventPriority.Lowest);

        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002598 File Offset: 0x00000798
        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            startOfExtraTurn = true;
            if (Count > 0)
            {
                yield return new ApplyStatusEffectAction<Firepower>(Owner, Count);
                manaAmount += new ManaGroup() { Colorless = Count };
                yield return new GainManaAction(manaAmount);
                yield return new DrawManyCardAction(Count);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnRoundEnded(GameEventArgs args)
        {
            if (base.Battle.BattleShouldEnd || !startOfExtraTurn)
            {
                yield break;
            }
            //int halfHP = Battle.Player.Hp / 2;
            //yield return new DamageAction(Battle.Player, Battle.Player, new DamageInfo(halfHP, DamageType.HpLose));
            yield return new DamageAction(Battle.Player, Battle.Player, new DamageInfo(Level, DamageType.Reaction));
            yield return new RemoveStatusEffectAction(this);
            yield break;
        }
    }
}