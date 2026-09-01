using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoL.Presentation.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModEthicallySourcedBreakDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.ImageId = "KomachiModQuickNap";
            config.Colors = new List<ManaColor>() { ManaColor.Green};
            config.Cost = new ManaGroup() { Green = 1, Any = 2 };
            config.Rarity = Rarity.Uncommon;
            config.FindInBattle = false;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.Nobody;

            config.Shield = 40;
            config.UpgradedShield = 45; 
            // Life gain
            config.Value1 = 10;
            config.UpgradedValue1 = 15;

            // Firepower amount
            config.Value2 = 3;
            config.UpgradedValue2 = 4;

            config.Mana = new ManaGroup() { Philosophy = 3 };
            config.UpgradedMana = new ManaGroup() { Philosophy = 3, Colorless = 1 };

            config.Illustrator = "Valonadthe";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.RelativeEffects = new List<string>() { nameof(Firepower)};
            config.UpgradedRelativeEffects = new List<string>() { nameof(Firepower)};
            config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModEthicallySourcedBreakDef))]
    public sealed class KomachiModEthicallySourcedBreak : KomachiCard
    {
        // Draw amount
        protected override int BaseValue3 { get => 2; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 3; }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Battle.Player.GetView<UnitView>().Chat("Mimimimimimimimi", 2);
            yield return DefenseAction();
            yield return base.HealAction(base.Value1);
            yield return BuffAction<Firepower>(Value2);
            yield return BuffAction<KomachiModEthicallySourcedBreakSe>(count:Value3, duration: 2);
            Battle.Player.GetStatusEffect<KomachiModEthicallySourcedBreakSe>().manaAmount += Mana;
            yield return new RequestEndPlayerTurnAction();
            yield break;
        }
    }
}


