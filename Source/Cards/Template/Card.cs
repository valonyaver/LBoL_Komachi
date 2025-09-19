using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using System.Linq;

namespace KomachiMod.Cards.Template
{
    public class KomachiCard : Card
    {
        //KomachiCard can be used to give additional properties to all the cards.
        //For instance, this can be used to give every card a new custom parameter called Value3. 
        //Custom value for display purposes.
        protected virtual int BaseValue3 {get; set;} = 0;
        protected virtual int BaseUpgradedValue3 {get; set;} = 0; 
        public int Value3
        {
            get
            {
                if (this.IsUpgraded)
                {
                    return BaseUpgradedValue3;
                }
                return BaseValue3;
            }
        }

        public virtual bool farDistanceInverseDamage => false;


        public string CardDialogue1
        {
            get
            {
                return this.LocalizeProperty("CardDialogue1", true, true);
            }
        }
        public string CardDialogue2
        {
            get
            {
                return this.LocalizeProperty("CardDialogue2", true, true);
            }
        }

        #region AUTO EXILE CODE
        public virtual bool isAutoDiscard => false;
        protected override void OnEnterBattle(BattleController battle)
        {
            if (isAutoDiscard)
            {
                base.HandleBattleEvent<CardEventArgs>
                    (base.Battle.Predraw,
                    new GameEventHandler<CardEventArgs>(this.OnPlayerDrawing), GameEventPriority.ConfigDefault);
                base.HandleBattleEvent<CardsEventArgs>
                    (base.Battle.CardsAddingToHand,
                    new GameEventHandler<CardsEventArgs>(this.OnPlayerAddingMany), GameEventPriority.ConfigDefault);
                base.HandleBattleEvent<CardMovingEventArgs>
                    (base.Battle.CardMoving,
                    new GameEventHandler<CardMovingEventArgs>(this.OnPlayerAdding), GameEventPriority.ConfigDefault);
            }
        }

        void OnPlayerDrawing(CardEventArgs args)
        {
            AutoDiscard(1);
        }
        void OnPlayerAddingMany(CardsEventArgs args)
        {
            AutoDiscard(args.Cards.Length);
        }

        void OnPlayerAdding(CardMovingEventArgs args)
        {
            if (args.DestinationZone == CardZone.Hand)
            {
                AutoDiscard(1);
            }
        }
        void AutoDiscard(int cardAmount)
        {
            int projectedHandsize = Battle.HandZone.Count + cardAmount;
            if (projectedHandsize >= Battle.MaxHand + 1 && Zone == CardZone.Hand)
            {
                // Get all Spider Lily cards in hand
                var autoDiscardInHand = Battle.HandZone
                    .Where(card => card is KomachiCard && (card as KomachiCard).isAutoDiscard)
                    .Take(cardAmount) // Only consider first 'cardAmount' lilies
                    .ToList();

                // Check if this card is in the first 'cardAmount' lilies
                if (autoDiscardInHand.Contains(this))
                {
                    React(new DiscardAction(this) { Cause = ActionCause.AutoExile });
                }
            }
        }
    }
    #endregion
}