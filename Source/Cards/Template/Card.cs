using LBoL.Core.Cards;

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
    }
}