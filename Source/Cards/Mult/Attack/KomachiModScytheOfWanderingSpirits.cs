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

            // config.ImageId = "KomachiAttackB";

            config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 7, Black = 1, Red = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 7;
            config.UpgradedDamage = 10;

            // Gain 1 life
            config.Value1 = 1;

            // Per 2 spirits detonated
            config.Value2 = 2;

            config.Keywords = Keyword.Accuracy | Keyword.Exile;
            config.UpgradedKeywords = Keyword.Accuracy | Keyword.Exile;

            config.RelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword) };


            config.Illustrator = "ryosios";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModScytheHealDef))]
    public sealed class KomachiModScytheHeal : KomachiCard  
    {

        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModScytheHeal noBoom = Library.CreateCard<KomachiModScytheHeal>();
            KomachiModScytheHeal boom = Library.CreateCard<KomachiModScytheHeal>();
            // indicate them
            noBoom.ChoiceCardIndicator = 1; // uses extra description 1
            boom.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            noBoom.SetBattle(base.Battle);
            boom.SetBattle(base.Battle);
            // add em to the list
            list.Add(noBoom);
            list.Add(boom);
            return new MiniSelectCardInteraction(list);
        }
        protected override void OnEnterBattle(BattleController battle)
		{
			base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageDealt, new EventSequencedReactor<DamageEventArgs>(this.OnPlayerDamageDealt));
            //foreach (var enemy in battle.AllAliveEnemies)
            //{
            //    base.HandleBattleEvent<DamageEventArgs>
            //        (enemy.DamageReceiving, new GameEventHandler<DamageEventArgs>(OnEnemyDamageReceiving), GameEventPriority.Lowest);
            //}
            //HandleBattleEvent<UnitEventArgs>
            //    (battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
        }

  //      void OnEnemySpawned(UnitEventArgs args)
  //      {
  //          base.HandleBattleEvent<DamageEventArgs>
  //              (args.Unit.DamageReceiving, new GameEventHandler<DamageEventArgs>(OnEnemyDamageReceiving), GameEventPriority.Lowest);
  //      }

  //      void OnEnemyDamageReceiving(DamageEventArgs args)
  //      {
  //          if (base.Battle.BattleShouldEnd)
  //          {
  //              return;
  //          }
  //          if (args.Cause == ActionCause.Card && args.ActionSource == this)
  //          {
  //              DamageInfo damageInfo = args.DamageInfo;
  //              if (damageInfo.Amount > 0f)
  //              {
  //                  React(new ApplyVengefulSpiritAction(args.Target, (int)damageInfo.Amount));
  //              }
  //          }
		//}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00019DAE File Offset: 0x00017FAE
		private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
		{
			if (base.Battle.BattleShouldEnd)
			{
				yield break;
			}
			if (args.Cause == ActionCause.Card && args.ActionSource == this)
			{
				DamageInfo damageInfo = args.DamageInfo;
				if (damageInfo.Amount > 0f)
                {
                    yield return new ApplyVengefulSpiritAction(args.Target, (int) damageInfo.Amount);
                }
			}
			yield break;
		}


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector, base.GunName);
            // look above for vengeful spirit attack

            Card choiceCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (choiceCard != null && choiceCard.ChoiceCardIndicator != 1)
            {
                var detonation = new DetonateVengefulSpiritAction(this, selector.SelectedEnemy);
                yield return detonation;
                int enemySpiritsCount = detonation.Args.amountDetonated;
                yield return HealAction(enemySpiritsCount / 2);
            }
            yield break;
        }
    }
}


