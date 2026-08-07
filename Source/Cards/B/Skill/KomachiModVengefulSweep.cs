using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.Source.StatusEffects.Spirits;
using KomachiMod.StatusEffects;
using LBoL.Base;
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
    public sealed class KomachiModVengefulSweepDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Spirits inflicted
            config.Value1 = 6;
            config.UpgradedValue1 = 8;

            // Release cost
            config.Value2 = 6;

            config.RelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };

            config.RelativeCards = new List<string>()
            {
                nameof(KomachiModDetonateToken)
            };
            config.UpgradedRelativeCards = new List<string>()
            {
                nameof(KomachiModDetonateToken)
            };

            config.Illustrator = "松岡二";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModVengefulSweepDef))]
    public sealed class KomachiModVengefulSweep : KomachiCard
    {
        protected override int BaseValue3 { get => Value2 + 3; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => Value2 + 2; set => base.BaseUpgradedValue3 = value; }
        public override bool Triggered
        {
            get
            {
                return KomachiModUtility.CanReleaseSpirits(Battle.Player, Value2);
            }
        }
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2, Value3);
        }
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(this, enemy, Value1);
            }
            Card releaseChoice = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(releaseChoice))
            {
                int releaseAmount = Value2;
                if (releaseChoice.ChoiceCardIndicator == 2) releaseAmount = Value3;
                yield return new KomachiReleaseAction(Battle.Player, releaseAmount);
                foreach (var enemy in Battle.AllAliveEnemies)
                {
                    yield return new ApplyVengefulSpiritAction(this, enemy, Value1);
                }
                if (releaseAmount == Value3)
                {
                    Card[] detonate = { Library.CreateCard<KomachiModDetonateToken>() };
                    yield return new AddCardsToHandAction(detonate);
                }
            }
            yield break;
        } 
    }
}


