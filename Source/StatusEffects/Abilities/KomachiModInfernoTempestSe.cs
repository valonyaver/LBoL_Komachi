using KomachiMod.BattleActions;
using KomachiMod.Cards;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core.Tokens;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModInfernoTempestSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModInfernoTempestSeDef))]
    public sealed class KomachiModInfernoTempestSe : StatusEffect
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

            List<Card> list = Battle.ExileZone.Where(card => !card.HasKeyword(Keyword.Basic)).ToList();
            var interaction = new SelectCardInteraction(0, Level, list, SelectedCardHandling.DoNothing);
            yield return new InteractionAction(interaction);

            IReadOnlyList<Card> cards = interaction.SelectedCards;
            Random rng = new Random();
            var shuffledcards = cards.OrderBy(_ => rng.Next()).ToList();

            List<Card> drawPileCards = new List<Card>();
            List<Card> discardPileCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            if (shuffledcards.Count >= 3)
            {
                drawPileCards.Add(shuffledcards[0]);
                discardPileCards.Add(shuffledcards[1]);
                handCards.Add(shuffledcards[2]);

                for (int i = 3; i < shuffledcards.Count; i++)
                {
                    int location = rng.Next(3);
                    switch(location)
                    {
                        case 0:
                            drawPileCards.Add(shuffledcards[i]);
                            break;
                        case 1:
                            discardPileCards.Add(shuffledcards[i]);
                            break;
                        case 2:
                            handCards.Add(shuffledcards[i]);
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < shuffledcards.Count; i++)
                {
                    int location = rng.Next(3);
                    switch (location)
                    {
                        case 0:
                            drawPileCards.Add(shuffledcards[i]);
                            break;
                        case 1:
                            discardPileCards.Add(shuffledcards[i]);
                            break;
                        case 2:
                            handCards.Add(shuffledcards[i]);
                            break;
                    }
                }
            }

            foreach(var card in drawPileCards)
            {
                yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Random);
            }

            foreach(var card in handCards)
            {
                yield return new MoveCardAction(card, CardZone.Hand);
            }

            foreach (var card in discardPileCards)
            {
                yield return new MoveCardAction(card, CardZone.Discard);
            }


            List<Card> basicMisfortuneList = Battle.ExileZone.Where(card => card.HasKeyword(Keyword.Basic) || card.CardType == CardType.Misfortune).ToList();
            foreach(var card in basicMisfortuneList)
            {
                yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Random);
            }

            yield return new RemoveStatusEffectAction(this); 

            yield break;
        }
    }
}