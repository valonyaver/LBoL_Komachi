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
using UnityEngine;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModExileDoppelgangerSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModExileDoppelgangerSeDef))]
    public sealed class KomachiModExileDoppelgangerSe : StatusEffect
    {
        int count2;
        // If it is 0, nothing happens, if it is 1, make a copy, if it is too, take the copy from exile.
        int effectUse;

        ManaGroup Mana = new ManaGroup() { Any = 1 };
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, delegate (UnitEventArgs _)
            {
                base.Count = base.Level;
                count2 = base.Level;
                base.Highlight = true;
            });
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
            base.HandleOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new GameEventHandler<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
        {
            if (args.Card.HasKeyword(Keyword.Copy)) yield break;
            // Sees if any card with the same name exist in exile.
            List<Card> sameCardList = Battle.ExileZone.Where(card => card.GetType() == args.Card.GetType()).ToList();
            Debug.Log($"Using ExileDoppelganger. The amount of cards with the same name as {args.Card.Name} in exile is {sameCardList.Count}");
            // If yes, and effect2 works.
            if (count2 > 0 && sameCardList.Count > 0)
            {
                // You can select one of those cards, and if you do, add it to yer hand.
                // It can be canceled rn but may be nerfed to be uncancellable.
                base.NotifyActivating();
                var interaction = new MiniSelectCardInteraction(sameCardList);
                yield return new InteractionAction(interaction, canCancel: true) { Source = this };
                if (!interaction.IsCanceled)
                {
                    yield return PerformAction.Sfx("二重身回合开始", 0f);
                    yield return new MoveCardAction(interaction.SelectedCard, CardZone.Hand);
                    effectUse = 2; // if you want it to disappear even if the player cancels, put it outside the if
                }
            }
            // Else, IF there is no exiled similar card AND the effect1 count is higher than 0.
            else if (base.Count > 0 && sameCardList.Count == 0)
            {
                // Clone the card and add it to exile.
                base.NotifyActivating();
                yield return PerformAction.Sfx("二重身回合开始", 0f);
                yield return PerformAction.Effect(base.Battle.Player, "JinziMirror", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                Card exiledCopy = args.Card.CloneBattleCard();
                if (exiledCopy.Cost.Amount > 0)
                {
                    exiledCopy.SetBaseCost(Mana);
                }
                exiledCopy.RemoveFromBattleAfterPlay = true;
                yield return new AddCardsToExileAction(new List<Card>() { exiledCopy });
                effectUse = 1;
            }
            yield break;
        }

        private void OnCardUsed(CardUsingEventArgs args)
        {
            // Depending on which effect was used, reduce its respective count. Remove highlight if both effects are used.
            if (effectUse == 1 && Count > 0)
            {
                Count--;
                
            }
            if (effectUse == 2 && count2 > 0)
            {
                count2--;
            }
            if (Count == 0 && count2 == 0)
            {
                base.Highlight = false;
            }
            effectUse = 0;
        }
    }
}