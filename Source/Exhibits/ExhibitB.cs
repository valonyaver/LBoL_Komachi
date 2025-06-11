using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Exhibits
{
    public sealed class KomachiExhibitBDef : KomachiExhibitTemplate
    {   
        public override ExhibitConfig MakeConfig()
        {

            ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
            exhibitConfig.Value1 = 2; // Spirit Gain
            exhibitConfig.Value2 = 2; // Vulnerable inflict
            exhibitConfig.Value3 = 2; // Debuff on spirits
            exhibitConfig.Mana = new ManaGroup() { Black = 1 };
            exhibitConfig.BaseManaColor = ManaColor.Black;

            exhibitConfig.RelativeEffects = new List<string>() { nameof(Spirit), nameof(Vulnerable) };
            
            return exhibitConfig;
        }
    }

    [EntityLogic(typeof(KomachiExhibitBDef))]
    public sealed class KomachiExhibitB : ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarted));
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnEnding, new EventSequencedReactor<UnitEventArgs>(this.OnTurnEnding));
        }

        // Token: 0x060003F5 RID: 1013 RVA: 0x0000AED8 File Offset: 0x000090D8
        private IEnumerable<BattleAction> OnPlayerTurnStarted(GameEventArgs args)
        {
            if (base.Battle.Player.TurnCounter == 1)
            {
                base.NotifyActivating();
                yield return new ApplyStatusEffectAction<Spirit>(base.Owner, Value1, null, null, null, 0f, false);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.Player.TurnCounter == 3)
            {
                base.NotifyActivating();
                yield return new ApplyStatusEffectAction<SpiritNegative>(base.Owner, Value1, null, null, null, 0f, false);
                foreach (EnemyUnit enemyUnit in base.Battle.EnemyGroup)
                {
                    if (enemyUnit.IsAlive)
                    {
                        yield return new ApplyStatusEffectAction<Vulnerable>(enemyUnit, new int?(1), new int?(base.Value2), null, null, 0f, true);
                    }
                }
            }
            yield break;
        }
    }
}