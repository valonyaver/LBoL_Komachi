using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModGoldSarcophagusDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black};
            //Hybrid colors:
            //0 = W/U
            //1 = W/B
            //2 = W/R
            //3 = W/G
            //4 = U/B
            //5 = U/R
            //6 = U/G
            //7 = B/R
            //8 = B/G
            //9 = R/G
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 1};
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;


            // Exiled cards added to hand
            config.Value1 = 1;

            // turn amount
            config.UpgradedValue2 = 2;

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Initial | Keyword.Replenish;

            config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModGoldSarcophagusDef))]
    public sealed class KomachiModGoldSarcophagus : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Battle.DrawZone.Count == 0) yield break;
            MiniSelectCardInteraction drawPileInteraction = new MiniSelectCardInteraction(Battle.DrawZone) { Source = this };
            yield return new InteractionAction(drawPileInteraction);
            yield return new MoveCardAction(drawPileInteraction.SelectedCard, CardZone.Exile);
            yield return BuffAction<KomachiModGoldSarcophagusSe>(1);
            Battle.Player.GetStatusEffect<KomachiModGoldSarcophagusSe>().AddCardToCoffin(drawPileInteraction.SelectedCard, 2);
            yield break;
        }
    }
}


