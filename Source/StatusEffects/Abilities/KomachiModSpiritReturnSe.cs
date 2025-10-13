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
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core.Tokens;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModSpiritReturnSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModSpiritReturnSeDef))]
    public sealed class KomachiModSpiritReturnSe : StatusEffect
    {
        // amount for card to be increased in cost
        ManaGroup Mana = new ManaGroup() { Any = 1 };
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();
                // bottom out the cards in hand
                //for (int i = 1; i <= Level; i++)
                //{
                //    yield return new MoveCardToDrawZoneAction(Battle.HandZone[^i], DrawZoneTarget.Bottom);
                //}
                // if exile is empty end effect
                if (base.Battle.ExileZone.Count <= 0)
                {
                    yield break;
                } 
                // filter cards to pick out 
                List<Card> list = Battle.ExileZone.Where(card => card.Cost.Amount < 2).ToList();
                // pick the cards
                var interaction = new SelectCardInteraction(0, Level, list, SelectedCardHandling.DoNothing);
                yield return new InteractionAction(interaction)
                {
                    Source = this
                };
                IReadOnlyList<Card> cards = (interaction).SelectedCards;

                foreach (Card card in cards)
                {
                    yield return new MoveCardAction(card, CardZone.Hand);
                    card.IncreaseBaseCost(Mana);
                    card.RemoveFromBattleAfterPlay = true;
                }
            }
            yield break;
        }
    }
}