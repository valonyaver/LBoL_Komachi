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
    public sealed class KomachiModChainDetonationDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1, Any = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Spirits inflicted
            config.Value1 = 5;
            config.UpgradedValue1 = 7;

            // Release cost
            config.Value2 = 5;
            config.UpgradedValue2 = 3;

            config.Mana = new ManaGroup() { Black = 1 };

            config.RelativeEffects = new List<string>()
            {nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword), nameof(KomachiModReleaseKeyword) };
            config.UpgradedRelativeEffects = new List<string>() 
            {nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword), nameof(KomachiModReleaseKeyword) };


            config.Illustrator = "Wholesome_illustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModChainDetonationDef))]
    public sealed class KomachiModChainDetonation : KomachiCard
    {
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, Value2);
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new ApplyVengefulSpiritAction(selector.SelectedEnemy, Value1);
            var boom1 = new DetonateVengefulSpiritAction(this, selector.SelectedEnemy);
            yield return boom1;
            if (boom1.Args.noFizzle)
            {
                yield return new ApplyVengefulSpiritAction(selector.SelectedEnemy, boom1.Args.amountDetonated);
            }
            Card choiceCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(choiceCard))
            {
                yield return new KomachiReleaseAction(this, Value2);
                yield return new DetonateVengefulSpiritAction(this, selector.SelectedEnemy);
            }
        } 
    }
}


