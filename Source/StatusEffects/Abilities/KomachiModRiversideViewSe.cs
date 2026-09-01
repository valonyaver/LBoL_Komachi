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
using LBoL.EntityLib.Cards.Neutral.TwoColor;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.Neutral.TwoColor;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModRiversideViewSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            config.RelativeEffects = new List<string>
            {
                nameof(Poison),
                nameof(Firepower)
            };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModRiversideViewSeDef))]
    public sealed class KomachiModRiversideViewSe : StatusEffect
    {
        // How many poison is applied to all enemies per spider lily use?
        public int poisonMultiplier = 3;

        public int poisonApply { get => Level * poisonMultiplier; }
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            // sees how many tokens in hand.
            List<KomachiModSpiderLily> list = base.Battle.HandZone.OfType<KomachiModSpiderLily>().ToList();
            if (list.Count < 1)
            {
                base.NotifyActivating();
                yield return new AddCardsToHandAction(Library.CreateCards<KomachiModSpiderLily>(1, false), AddCardsType.Normal);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd && args.Card is KomachiModSpiderLily)
            {
                base.NotifyActivating();
                yield return BuffAction<Firepower>(Level);
                foreach(var enemy in Battle.AllAliveEnemies)
                {
                    yield return DebuffAction<Poison>(enemy, poisonApply);
                }
            }
            yield break;
        }
    }
}