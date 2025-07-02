using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModParryDef : KomachiCardTemplate
    {


        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            //config.ImageId = nameof(KomachiModBlockR);

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.Nobody;

            config.Block = 8;
            config.UpgradedBlock = 12;

            // Reflect
            config.Value1 = 8;
            config.UpgradedValue1 = 12;

            config.Illustrator = "きんたろう";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.RelativeEffects = new List<string>() { nameof(Reflect) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(Reflect) };
            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModParryDef))]
    public sealed class KomachiModParry : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction(true);
            yield return BuffAction<Reflect>(Value1);
            yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModManDistance>() });
        }
    }
}


