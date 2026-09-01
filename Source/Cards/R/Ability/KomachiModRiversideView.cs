using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModRiversideViewDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 3, Any = 2 };
            config.Rarity = Rarity.Rare; 
            //config.IsPooled = false;
            //config.HideMesuem = true;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Buff amount. Also firepower amount.
            config.Value1 = 1;

            // Poison application. Only for description purposes. The functional value is in the se class.
            config.Value2 = 3;

            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };

            config.RelativeEffects = new List<string>
            {
                nameof(Poison),
                nameof(Firepower)
            };
            config.UpgradedRelativeEffects = new List<string>
            {
                nameof(Poison),
                nameof(Firepower)
            };

            config.Illustrator = "蒼穹＠葵衣";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModRiversideViewDef))]
    public sealed class KomachiModRiversideView : KomachiCard
    {
        // Poison Lost
        protected override int BaseValue3 { get => 2; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 3; }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (IsUpgraded) {
                yield return new AddCardsToHandAction(Library.CreateCards<KomachiModSpiderLily>(Value1, false));
            }
            yield return BuffAction<KomachiModRiversideViewSe>(base.Value1, count:Value3);
            yield break;
        }
    }
}


