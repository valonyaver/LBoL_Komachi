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
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System;
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

            // config.ImageId = nameof(KomachiModAttackB);

            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            config.Cost = new ManaGroup() { Red = 1, Green = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 1, Any = 1, HybridColor = 9 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;
        
            config.Damage = 5;
            config.UpgradedDamage = 4;

            // Amount of Attacks
            config.Value1 = 2;
            config.UpgradedValue1 = 3;

            // Amount of Poison applied and lost
            config.Value2 = 3; 

            config.RelativeEffects = new List<string>() { nameof(Poison)};
            config.UpgradedRelativeEffects = new List<string>() { nameof(Poison)};

            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };

            // config.UpgradedKeywords = Keyword.Accuracy;


            config.Illustrator = "Iced_Lemon";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModParasiticPollenDef))]
    public sealed class KomachiModParasiticPollen : KomachiCard
    {
        public float poisonApplied;
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
                    poisonApplied += damageInfo.Damage;
                }
			}
			yield break;
		}


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            poisonApplied = 0;
            // Check if Enemy Has Poison
            bool hasPoison = false;
            if (selector.SelectedEnemy.HasStatusEffect<Poison>())
            {
                hasPoison = true;
            }
            // Deal damage times
            for (int i = 0; i < Value1; i++)
            {
                yield return base.AttackAction(selector, base.GunName);
            }
            // If enemy has poison, get lily. Otherwise, apply poison.
            if (hasPoison) yield return new AddCardsToHandAction(Library.CreateCards<KomachiModSpiderLily>(1, false));
            else
            {
                yield return new ApplyStatusEffectAction<Poison>
                    (selector.SelectedEnemy, Value2);
            }

            if (Battle.Player.TryGetStatusEffect<Poison>(out var playerPoison))
            {
                playerPoison._level -= 3;
                playerPoison.NotifyChanged();
                if (playerPoison._level <= 0)
                {
                    yield return new RemoveStatusEffectAction(playerPoison);
                }
                // yield return new ApplyStatusEffectAction<KomachiModPoisonNegativeSe>(Battle.Player, Value2);

                yield return new ApplyStatusEffectAction<Poison>
                    (selector.SelectedEnemy, (poisonApplied).RoundToInt(MidpointRounding.AwayFromZero));
            }
            yield break;
        }
    }
}


