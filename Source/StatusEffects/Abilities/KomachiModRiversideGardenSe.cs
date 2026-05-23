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
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModRiversideGardenSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;
            config.LevelStackType = StackType.Add;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModRiversideGardenSeDef))]
    public sealed class KomachiModRiversideGardenSe : StatusEffect
    {
        private int SpiderLilyCount
        {
            get
            {
                if (base.Battle == null)
                {
                    return 0;
                }
                return base.Battle.EnumerateAllCards().Count((Card card) => card is KomachiModSpiderLily);
            }
        }

        int playerLifeOnRecord;
        int healCeiling => playerLifeOnRecord + Level * 3;
        protected override void OnAdded(Unit unit)
        {
            playerLifeOnRecord = Owner.Hp;
            // Card upgrade stuffs
            base.ReactOwnerEvent<CardsEventArgs>(base.Battle.CardsAddedToDiscard, new EventSequencedReactor<CardsEventArgs>(this.OnAddCard));
            base.ReactOwnerEvent<CardsEventArgs>(base.Battle.CardsAddedToHand, new EventSequencedReactor<CardsEventArgs>(this.OnAddCard));
            base.ReactOwnerEvent<CardsEventArgs>(base.Battle.CardsAddedToExile, new EventSequencedReactor<CardsEventArgs>(this.OnAddCard));
            base.ReactOwnerEvent<CardsAddingToDrawZoneEventArgs>
                (base.Battle.CardsAddedToDrawZone, new EventSequencedReactor<CardsAddingToDrawZoneEventArgs>(this.OnCardsAddedToDrawZone));
            // Heal stuffs
            base.ReactOwnerEvent<GameEventArgs>(base.Battle.BattleEnding, new EventSequencedReactor<GameEventArgs>(this.OnBattleEnding));
        }

        private IEnumerable<BattleAction> OnAddCard(CardsEventArgs args)
        {
            yield return this.Upgrade(args.Cards);
            yield break;
        }

        // Token: 0x0600007E RID: 126 RVA: 0x00002D7C File Offset: 0x00000F7C
        private IEnumerable<BattleAction> OnCardsAddedToDrawZone(CardsAddingToDrawZoneEventArgs args)
        {
            yield return this.Upgrade(args.Cards);
            yield break;
        }

        // Token: 0x0600007F RID: 127 RVA: 0x00002D94 File Offset: 0x00000F94
        private BattleAction Upgrade(IEnumerable<Card> cards)
        {
            List<Card> list = cards.Where((Card card) => card.CanUpgradeAndPositive && card is KomachiModSpiderLily).ToList<Card>();
            if (list.Count == 0)
            {
                return null;
            }
            base.NotifyActivating();
            return new UpgradeCardsAction(list);
        }

        private IEnumerable<BattleAction> OnBattleEnding(GameEventArgs args)
        {
            var player = base.Battle.Player;

            if (player.IsAlive)
            {
                base.NotifyActivating();
                int rawHealAmount = base.Level * SpiderLilyCount;

                // Ensure the total life after healing does not exceed the ceiling
                // Final Heal = Min(Raw Heal, Ceiling - Current Life)
                int finalHeal = Math.Min(rawHealAmount, Math.Max(0, healCeiling - player.Hp));

                if (finalHeal > 0)
                {
                    yield return new HealAction(player, player, finalHeal, HealType.Normal, 0.2f);
                }
            }
            yield break;
        }
    }
}