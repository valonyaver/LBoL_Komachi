using KomachiMod.BattleActions;
using KomachiMod.Cards;
using KomachiMod.Patches;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModShinigamiTacticsSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModShinigamiTacticsSeDef))]
    public sealed class KomachiModShinigamiTacticsSe : StatusEffect
    {
        public ManaGroup Mana
        {
            get
            {
                return ManaGroup.Single(ManaColor.Any);
            }
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00002470 File Offset: 0x00000670
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        // Token: 0x06000026 RID: 38 RVA: 0x0000248F File Offset: 0x0000068F
        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd && args.Card is KomachiModManDistance)
            {
                KomachiModManDistance manDist = (KomachiModManDistance)args.Card;
                base.NotifyActivating();
                Card[] array;
                // if you dont change distance, you dont get card
                if (manDist.lastDistanceChange == 0) yield break;
                // GET OVER HERE. Stab.
                else if (manDist.lastDistanceChange < 0)
                {
                    array = base.Battle.RollCards(
                        new CardWeightTable(
                            RarityWeightTable.BattleCard, 
                            OwnerWeightTable.Valid, 
                            CardTypeWeightTable.OnlyAttack, false), base.Level, null);
                }
                // Nevermind go back dont hurt me.
                else
                {
                    array = base.Battle.RollCards(
                        new CardWeightTable(
                            RarityWeightTable.BattleCard,
                            OwnerWeightTable.Valid,
                            CardTypeWeightTable.OnlyDefense, false), base.Level, null);
                }
                if (array.Length > 0)
                {
                    foreach (Card card in array)
                    {
                        if (card.Cost.Amount > 0) card.SetTurnCost(this.Mana);
                        card.IsEthereal = true;
                        card.IsExile = true;
                    }
                    yield return new AddCardsToHandAction(array);
                }
            }
            yield break;
        }
    }
}