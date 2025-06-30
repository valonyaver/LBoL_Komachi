using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.EntityLib.EnemyUnits.Normal.Guihuos;
using LBoL.EntityLib.EnemyUnits.Normal.Shenlings;
using LBoL.EntityLib.Exhibits;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Exhibits
{
    public sealed class KomachiExhibitBDef : KomachiExhibitTemplate
    {   
        public override ExhibitConfig MakeConfig()
        {

            ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
            exhibitConfig.Value1 = 1; // Spirit Gain
            exhibitConfig.Value2 = 1; // Vulnerable inflict
            exhibitConfig.Value3 = 2; // Debuff on spirits
            exhibitConfig.Mana = new ManaGroup() { Black = 1 };
            exhibitConfig.BaseManaColor = ManaColor.Black;

            exhibitConfig.RelativeEffects = new List<string>() 
            { 
                nameof(Spirit), 
                nameof(Vulnerable), 
                nameof(Weak),
                nameof(Amulet)
            };
            
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


        private IEnumerable<BattleAction> OnPlayerTurnStarted(GameEventArgs args)
        {
            if (base.Battle.Player.TurnCounter == 1)
            {
                base.NotifyActivating();
                yield return new ApplyStatusEffectAction<Spirit>(base.Owner, Value1, null, null, null, 0f, false);
                foreach (var enemy in Battle.AllAliveEnemies)
                {
                    if (enemy is Shenling || enemy is Guihuo)
                    {
                        var amulet = enemy.GetStatusEffect<Amulet>();
                        if (amulet != null)
                        {
                            yield return new RemoveStatusEffectAction(amulet);
                        }
                        yield return new ApplyStatusEffectAction<Weak>(enemy, duration: Value3);
                        yield return new ApplyStatusEffectAction<Vulnerable>(enemy, duration: Value3);
                    }
                }
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