using System.Collections.Generic;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using KomachiMod.Source.BattleActions.EventManager;

namespace KomachiMod.BattleActions
{
    public sealed class BuffAttackCardAction : EventBattleAction<BuffAttackEventArgs>
    {             
        internal BuffAttackCardAction(Card card = null, int amount = 0)
		{
			base.Args = new BuffAttackEventArgs
			{ 
                Card = card,
                Amount = amount,
			};
		}

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<BuffAttackEventArgs>("PreCustomEvent", Args, KomachiEventsManager.PreCustomEvent);

            yield return base.CreatePhase("Main", delegate
			{
                if(Args.Card != null && Args.Card.Config.Type == CardType.Attack)
                {
                    Args.Card.DeltaDamage += Args.Amount;
                }
            }, hasViewer: true);
    
            yield return base.CreateEventPhase<BuffAttackEventArgs>("PostCustomEvent", Args, KomachiEventsManager.PostCustomEvent);
            yield break;
        }
    }
}