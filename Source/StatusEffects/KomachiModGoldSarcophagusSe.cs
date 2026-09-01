using KomachiMod.Source.BattleActions.Helpers;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Utils;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModGoldSarcophagusSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModGoldSarcophagusSeDef))]
    public sealed class KomachiModGoldSarcophagusSe : StatusEffect
    {
        public const int returnTime = 0;
        public string SarcophagusContent
        {
            get
            {
                if (coffinedCards.Count == 0)
                    return "";

                string result = "";
                string keywordColor = KomachiModUtility.KeywordColor;
                string valueColor = KomachiModUtility.normalValueColor;

                foreach (CoffinedCard coffin in coffinedCards)
                {
                    // Replace placeholders in the template
                    string coffinString = ExtraDescription
                        .Replace("CARDNAME", KomachiModUtility.GetColoredText(coffin.chosenCard.SelfName, keywordColor))
                        .Replace("DURATION", KomachiModUtility.GetColoredText(coffin.duration.ToString(), valueColor));

                    result += "\n" + coffinString;
                }

                return result;
            }
        }
        public class CoffinedCard
        {
            public Card chosenCard;
            public int duration;
        }
        public List<CoffinedCard> coffinedCards = new List<CoffinedCard>();

        public void AddCardToCoffin(Card card, int duration)
        {
            if (duration < 0)
            {
                Debug.LogError("When trying to add a card from Gold Sarcophagus, the duration was less than 0.");
                duration = 2;
            }
            CoffinedCard coffin = new CoffinedCard() { chosenCard = card, duration = duration };
            coffinedCards.Add(coffin);
        }
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();
                // make a shallow copy
                List<CoffinedCard> coffinedCards2 = new List<CoffinedCard>(coffinedCards);
                foreach (CoffinedCard coffin in coffinedCards2)
                {
                    coffin.duration--;
                    if (coffin.duration <= 0)
                    {
                        if (coffin.chosenCard.Zone != CardZone.Hand)
                        {
                            if (coffin.chosenCard.CanUpgrade)
                            {
                                yield return new UpgradeCardAction(coffin.chosenCard);
                            }
                            coffin.chosenCard.SetKeyword(Keyword.Replenish, true);
                            yield return new MoveCardAction(coffin.chosenCard, CardZone.Hand);
                        }
                        coffinedCards.Remove(coffin);
                        Level -= 1;
                    }
                }
                if (coffinedCards.Count == 0)
                {
                    yield return new RemoveStatusEffectAction(this);
                }
            }
            yield break;
        }
    }
}