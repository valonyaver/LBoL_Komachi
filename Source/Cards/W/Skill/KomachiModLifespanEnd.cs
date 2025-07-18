using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Cards;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using KomachiMod.StatusEffects;

namespace KomachiMod.Cards
{
    public sealed class KomachiModLifespanEndDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Damage
            config.Value1 = 99;

            // Mana and firepower gain.
            config.Value2 = 0;
            config.UpgradedValue2 = 4;

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.Mana = new ManaGroup() { Colorless = 4 };

            config.UpgradedRelativeEffects = new List<string>() { nameof(Firepower) };
            // config.UpgradedRelativeEffects = new List<string>() { nameof(TimeIsLimited) };

            config.Illustrator = "Aiden Guo";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModLifespanEndDef))]
    //TimeStopCards inhehit from LimitedStopTimeCard instead of Card
    public sealed class KomachiModLifespanEnd : LimitedStopTimeCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<ExtraTurn>(1, 0, 0, 0, 0.2f);
            yield return BuffAction<TurnStartDontLoseBlock>(1);
            yield return BuffAction<KomachiModLifespanEndSe>(Value1, count:Value2);
            yield return new RequestEndPlayerTurnAction();
            
            yield break;
        }
    }
}


