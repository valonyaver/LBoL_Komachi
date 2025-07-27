using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModGrudgingStrikeDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(7001);

            // config.ImageId = nameof(KomachiModAttackB);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 14;
            config.UpgradedDamage = 16;

            // Spirit apply
            config.Value1 = 4;
            config.UpgradedValue1 = 7;

            // Release cost
            config.Value2 = 3;
            config.UpgradedValue2 = 2;

            config.RelativeEffects = new List<string>() 
            { nameof(KomachiModVengefulSpiritSe), nameof(KomachiModReleaseKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe), nameof(KomachiModReleaseKeyword) };

            config.RelativeCards = new List<string>() { nameof(KomachiModDetonateToken) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModDetonateToken) };


            config.Illustrator = "北公爵小三";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModGrudgingStrikeDef))]
    public sealed class KomachiModGrudgingStrike : KomachiCard
    {
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(this, Value2);
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector, base.GunName);
            yield return new ApplyVengefulSpiritAction(this, selector.SelectedEnemy, Value1);

            Card choiceCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(choiceCard))
            {
                yield return new KomachiReleaseAction(this, Value2);
                Card[] detonate =  { Library.CreateCard<KomachiModDetonateToken>() };
                yield return new AddCardsToHandAction(detonate);
            }
            yield break;
        }
    }
}


