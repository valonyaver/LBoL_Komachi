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
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModStrongestSpiritDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(14130);


            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Blue = 1, Any = 2 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;
        
            config.Damage = 9;

            // Release cost
            config.Value1 = 9;

            // Firepower gain
            config.Value2 = 9;

            config.UpgradedKeywords = Keyword.Retain | Keyword.Accuracy;

            config.RelativeEffects = new List<string>() 
            { 
                nameof(KomachiModVengefulSpiritSe), 
                nameof(KomachiModReleaseKeyword),
                nameof(TempFirepower)
            };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiModVengefulSpiritSe)};


            config.Illustrator = "harada (sansei rain)";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModStrongestSpiritDef))]
    public sealed class KomachiModStrongestSpirit : KomachiCard
    {
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, Value1);
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
			if (args.Cause == ActionCause.Card && args.ActionSource == this)
			{
				DamageInfo damageInfo = args.DamageInfo;
				if (damageInfo.Damage > 0f)
                {
                    yield return new ApplyVengefulSpiritAction(this, args.Target, (int) damageInfo.Damage);
                }
			}
			yield break;
		}

        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value1);
        }


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector, base.GunName);
            Card releaseChoice = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(releaseChoice))
            {
                yield return new KomachiReleaseAction(this, Value1);
                List<Card> list = base.Battle.HandZone.Where((Card card) => card != this).ToList<Card>();
                var action = new ExileManyCardAction(list);
                yield return action;
                var cardsExiled = action.Cards.Length;
                for(int i = 0; i <  cardsExiled; i++)
                {
                    yield return base.AttackAction(selector, base.GunName);
                }
                if (cardsExiled >= Value2)
                {
                    yield return BuffAction<Firepower>(Value2);
                }
            }
            yield break;
        }
    }
}


