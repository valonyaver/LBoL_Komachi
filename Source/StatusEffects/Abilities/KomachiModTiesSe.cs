using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core.Tokens;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModTiesSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModTiesSeDef))]
    public sealed class KomachiModTiesSe : StatusEffect
    {
        public ManaGroup Mana => ManaGroup.Anys(1);

        public Queue<Card> lastPlayedCards = new Queue<Card>();
        protected override void OnAdded(Unit unit)
        {
            HandleOwnerEvent(Battle.CardUsed, OnCardPlayed);
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarting), GameEventPriority.Highest);
        }

        private void OnCardPlayed(CardUsingEventArgs args)
        {
            if (args.Card.CardType != CardType.Ability && !args.Card.HasKeyword(Keyword.Copy))
            {
                lastPlayedCards.Enqueue(args.Card);
                while (lastPlayedCards.Count > Level)
                {
                    lastPlayedCards.Dequeue();
                }
            }
        }

        private IEnumerable<BattleAction> OnTurnStarting(UnitEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();

                while (lastPlayedCards.Count > 0)
                {
                    Card card = lastPlayedCards.Dequeue();
                    if (card == null) continue;
                    Card copy = card.CloneBattleCard();
                    copy.IsExile = true;
                    copy.IsEthereal = true;
                    copy.SetTurnCost(Mana);
                    yield return new AddCardsToHandAction(copy);
                }
            }
        }
    }
}