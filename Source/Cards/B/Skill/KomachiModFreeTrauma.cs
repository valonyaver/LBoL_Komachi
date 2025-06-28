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
    public sealed class KomachiModFreeTraumaDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Release cost1
            config.Value1 = 4;

            // Release cost2
            config.Value2 = 8;

            config.Mana = new ManaGroup() { Black = 1 };

            config.RelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };


            config.Illustrator = "Wholesome_illustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModFreeTraumaDef))]
    public sealed class KomachiModFreeTrauma : KomachiCard
    {
        // Vengeful spirits inflicted.
        protected override int BaseValue3 { get => 3; }
        protected override int BaseUpgradedValue3 { get => 3; }

        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value1, Value2);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new ApplyVengefulSpiritAction(selector.SelectedEnemy, Value3);
            yield return new GainManaAction(Mana);
            Card card = KomachiModUtility.GetPreconditionCard(precondition);
            if (card == null || card.GetType() == typeof(KomachiModReleaseNone)) yield break;
            if (card.ChoiceCardIndicator == 1)
            {
                yield return new KomachiReleaseAction(Battle.Player, Value1);
                yield return new GainManaAction(Mana);
            }
            else
            {
                yield return new KomachiReleaseAction(Battle.Player, Value2);
                yield return new GainManaAction(Mana *2);
            } 
        } 
    }
}


