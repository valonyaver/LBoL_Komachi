using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.Source.StatusEffects.Spirits;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModScytheHealDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);
            config.FindInBattle = false;
            // config.ImageId = "KomachiAttackB";

            config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 7, Black = 1, Red = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 3;

            // Hit number
            config.Value1 = 4;
            config.UpgradedValue1 = 5;

            // Vengeful Spirit number
            config.Value2 = 2;

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.RelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };


            config.Illustrator = "ryosios";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModScytheHealDef))]
    public sealed class KomachiModScytheHeal : KomachiCard  
    {
        /// <summary>
        /// Heal amount
        /// </summary>
        protected override int BaseValue3 { get => 2;}
        protected override void OnEnterBattle(BattleController battle)
		{
			base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageDealt, new EventSequencedReactor<DamageEventArgs>(this.OnPlayerDamageDealt));
        }

		private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
		{
			if (base.Battle.BattleShouldEnd)
			{
				yield break;
			}
			if (args.ActionSource == this && !args.DamageInfo.IsGrazed)
            {
                yield return new ApplyVengefulSpiritAction(this, args.Target, Value2);
                DamageInfo damageInfo = args.DamageInfo;
				if (damageInfo.Damage > 0f)
                {
                    yield return HealAction(Value3);
                }
			}
			yield break;
		}


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for (int i = 0; i < Value1; i++)
            {
                yield return base.AttackAction(selector, base.GunName);
            }

            yield break;
        }
    }
}


