using KomachiMod.BattleActions;
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
    public sealed class KomachiModSpiritDrawDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1, Any =1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Vengeful gain
            config.Value1 = 2;
            // Guided gain
            config.Value2 = 3;

            config.Illustrator = "ふんぼ";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() 
            { 
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiModGuidedSpiritSe),
                nameof(KomachiModDivineSpiritSe)
            };
            config.UpgradedRelativeEffects = new List<string>() 
            {
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiModGuidedSpiritSe),
                nameof(KomachiModDivineSpiritSe)
            };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSpiritDrawDef))]
    public sealed class KomachiModSpiritDraw : KomachiCard
    {
        // Divine gain
        protected override int BaseValue3 { get => 4; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 4; set => base.BaseUpgradedValue3 = value; }

        int draw = 3;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            DrawManyCardAction drawAction = new DrawManyCardAction(draw);
            yield return drawAction;
            IReadOnlyList<Card> drawnCards = drawAction.DrawnCards;
            int common = drawnCards.Count((Card card) => card.Config.Rarity == Rarity.Common);
            int uncommon = drawnCards.Count((Card card) => card.Config.Rarity == Rarity.Uncommon);
            int rare = drawnCards.Count((Card card) => card.Config.Rarity == Rarity.Rare);
            if (common > 0)
            {
                foreach(var enemy in Battle.AllAliveEnemies)
                {
                    yield return new ApplyVengefulSpiritAction(this,enemy, Value1 * common);
                }
            }
            if (uncommon > 0)
            {
                yield return base.BuffAction<KomachiModGuidedSpiritSe>(base.Value2 * uncommon, 0, 0, 0, 0.2f);
            }
            if (rare > 0)
            {
                yield return base.BuffAction<KomachiModDivineSpiritSe>(base.Value3 * rare, 0, 0, 0, 0.2f);
            }
            yield break;
        }
    }
}


