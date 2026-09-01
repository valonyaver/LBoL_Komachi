using KomachiMod.Cards;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Exhibits
{
    public sealed class KomachiModExhibitRDef : KomachiExhibitTemplate
    {
        public override ExhibitConfig MakeConfig()
        {
            ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();

            exhibitConfig.Value1 = 10; // percent of gold increase
            exhibitConfig.Mana = new ManaGroup() { Red = 1 };
            exhibitConfig.BaseManaColor = ManaColor.Red;
            exhibitConfig.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily), nameof(KomachiModManDistance) };

            return exhibitConfig;
        }
    }

    /// <summary>
    /// The titanic
    /// </summary>
    [EntityLogic(typeof(KomachiModExhibitRDef))]
    public sealed class KomachiModExhibitR : ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarting));
        }

        /// <summary>
        /// Adds Manipulate Distance
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (base.Battle.Player.TurnCounter == 1)
            {
                base.NotifyActivating();
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModManDistance>() });
            }
            else if (base.Battle.Player.TurnCounter == 3)
            {

                base.NotifyActivating();
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModSpiderLily>() });
            }
        }

        #region MONEY MONEY MONEY
        private float GetMultiplier()
        {
            return (float)(100 + base.Value1) / 100f;
        }

        // Token: 0x0600069A RID: 1690 RVA: 0x0000F080 File Offset: 0x0000D280
        protected override void OnAdded(PlayerUnit player)
        {
            base.GameRun.RewardMoneyMultiplier *= this.GetMultiplier();
        }

        // Token: 0x0600069B RID: 1691 RVA: 0x0000F09A File Offset: 0x0000D29A
        protected override void OnRemoved(PlayerUnit player)
        {
            base.GameRun.RewardMoneyMultiplier /= this.GetMultiplier();
        }
        #endregion
    }
}