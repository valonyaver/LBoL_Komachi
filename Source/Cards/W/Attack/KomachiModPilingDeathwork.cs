using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.StatusEffects.Spirits;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModPilingDeathworkDef : KomachiCardTemplate
    {


        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(6140);
            // config.ImageId = nameof(KomachiModAttackR);

            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 1, Any = 1 };

            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 10;
            config.UpgradedDamage = 14;

            config.Value1 = 5;
            config.UpgradedValue1 = 7;

            config.Illustrator = "Neruzou";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.RelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModPilingDeathworkDef))]
    public sealed class KomachiModPilingDeathwork : KomachiCard
    {
        public int attackTimes
        {
            get
            {
                if (Battle != null && Battle.Player.TryGetStatusEffect<KomachiModPilingDeathworkSe>(out var status))
                {
                    return status.Level + 1;
                }
                return 1;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for (int i = 0; i < attackTimes; i++)
            {
                yield return AttackAction(selector.SelectedEnemy);
                yield return new ApplyVengefulSpiritAction(selector.SelectedEnemy, Value1);
            }
        }

        public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
        {
            yield return BuffAction<KomachiModPilingDeathworkSe>(1);
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            ReactBattleEvent(Battle.Player.TurnStarting, OnPlayerTurnStarting);
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            yield return new MoveCardToDrawZoneAction(this, DrawZoneTarget.Random);
        }
    }
}


