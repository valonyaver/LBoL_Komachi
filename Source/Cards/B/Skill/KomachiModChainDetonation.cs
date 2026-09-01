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
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModChainDetonationDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2 };
            config.UpgradedCost = new ManaGroup() { Black = 1, Any = 1 };

            config.Rarity = Rarity.Rare;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Spirits inflicted
            config.Value1 = 4;
            config.UpgradedValue1 = 6;

            // Release cost
            config.Value2 = 4;
            config.UpgradedValue2 = 3;

            config.Mana = new ManaGroup() { Black = 1 };

            config.RelativeEffects = new List<string>()
            {nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword), nameof(KomachiModReleaseKeyword) };
            config.UpgradedRelativeEffects = new List<string>() 
            {nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword), nameof(KomachiModReleaseKeyword) };


            config.Illustrator = "@RE_yomawari";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModChainDetonationDef))]
    public sealed class KomachiModChainDetonation : KomachiCard
    {
        // Second Release cost
        protected override int BaseValue3 { get => 8; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 8; }
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, Value2);
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2, Value3);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // Apply spirits
            yield return new ApplyVengefulSpiritAction(this, selector.SelectedEnemy, Value1);
            // Boom them
            var boom1 = new DetonateVengefulSpiritAction(this, selector.SelectedEnemy);
            yield return boom1;
            // If spirits did boomed, apply them again
            if (boom1.Args.noFizzle)
            {
                yield return new ApplyVengefulSpiritAction(this, selector.SelectedEnemy, boom1.Args.amountDetonated);
            }
            Card choiceCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(choiceCard, Value2, Value3, out int costResult))
            {
                // If you chose to release, boom them again.
                yield return new KomachiReleaseAction(this, costResult);
                var boom2 = new DetonateVengefulSpiritAction(this, selector.SelectedEnemy);
                yield return boom2;
                // If boom happened again, apply it again.
                if (choiceCard.ChoiceCardIndicator == 2 && boom2.Args.noFizzle)
                {
                    yield return new ApplyVengefulSpiritAction(this, selector.SelectedEnemy, boom2.Args.amountDetonated);
                }
            }
        } 
    }
}


