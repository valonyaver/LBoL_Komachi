using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    /// <summary>
    /// It's actually an ability not a block card
    /// </summary>
    public sealed class KomachiModShinigamiTacticsDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2, Any = 3 };
            config.UpgradedCost = new ManaGroup() { Red = 2, Any = 2 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Level of buff
            config.Value1 = 1;
            // mandist generated
            config.Value2 = 1;

            config.Mana = new ManaGroup() { Any = 1 };


            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModManDistance) };

            config.RelativeEffects = new List<string>() { nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDistanceKeyword) };

            config.RelativeKeyword = Keyword.Exile | Keyword.Ethereal;

            config.Illustrator = "芋鍋";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModShinigamiTacticsDef))]
    public sealed class KomachiModShinigamiTactics : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModShinigamiTacticsSe>(Value1, 0, 0, 0, 0.2f);
            yield return new AddCardsToHandAction(Library.CreateCards<KomachiModManDistance>(base.Value2, false), AddCardsType.Normal);
            yield break;
        }
    }
}


