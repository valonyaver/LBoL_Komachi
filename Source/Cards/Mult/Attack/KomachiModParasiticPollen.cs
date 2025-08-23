using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModParasiticPollenDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(7190);
            config.GunNameBurst = GunNameID.GetGunFromId(7191);
            config.Owner = null;

            // config.ImageId = nameof(KomachiModAttackB);

            config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Green };
            config.Cost = new ManaGroup() { Black = 1, Green = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 1, Any = 1, HybridColor = 8 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;
        
            config.Damage = 7;
            config.UpgradedDamage = 10;

            // Amount of Red Lilies given
            config.Value1 = 1;

            config.RelativeEffects = new List<string>() { nameof(Poison)};
            config.UpgradedRelativeEffects = new List<string>() { nameof(Poison)};

            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };

            config.UpgradedKeywords = Keyword.Accuracy;


            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModParasiticPollenDef))]
    public sealed class KomachiModParasiticPollen : KomachiCard
    {
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
			if (args.Cause == ActionCause.Card && args.ActionSource == this && !args.Target.HasStatusEffect<Poison>())
			{
				DamageInfo damageInfo = args.DamageInfo;
				if (damageInfo.Damage > 0f)
                {
                    yield return new ApplyStatusEffectAction<Poison>(args.Target, (int) damageInfo.Damage);
                }
			}
			yield break;
		}


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            bool hasPoison = false;
            if (selector.SelectedEnemy.HasStatusEffect<Poison>())
            {
                hasPoison = true;
            }
            yield return base.AttackAction(selector, base.GunName);
            if (hasPoison) yield return new AddCardsToHandAction(Library.CreateCards<KomachiModSpiderLily>(Value1, false));

            yield break;
        }
    }
}


