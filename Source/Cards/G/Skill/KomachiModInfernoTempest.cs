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
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModInfernoTempestDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Green, ManaColor.Black, ManaColor.Red};
            config.Cost = new ManaGroup() { Green = 1, Black = 1, Red = 1, Any = 2 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Spirit gain
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            // Firepower amount
            config.Value2 = 4;

            config.Illustrator = "Valonadthe";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() 
            { 
                nameof(KomachiModGuidedSpiritSe),
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiModDivineSpiritSe)
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(KomachiModGuidedSpiritSe),
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiModDivineSpiritSe)
            };
            config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModInfernoTempestDef))]
    public sealed class KomachiModInfernoTempest : KomachiCard
    {
        public override bool RemoveFromBattleAfterPlay { get => true; set => base.RemoveFromBattleAfterPlay = value; }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card[] discardPile = Battle.DiscardZone.ToArray();
            Card[] drawPile = Battle.DrawZone.ToArray();
            Card[] handPile = Battle.HandZone.ToArray();

            int di = discardPile.Length, dr = drawPile.Length, hn = handPile.Length;

            List<Card> allCards = new List<Card>();
            allCards.AddRange(discardPile);
            allCards.AddRange(drawPile);
            allCards.AddRange(handPile);

            yield return new ExileManyCardAction(allCards);
            foreach(var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(enemy, di * Value1);
            }
            yield return BuffAction<KomachiModGuidedSpiritSe>(dr * Value1);
            yield return BuffAction<KomachiModDivineSpiritSe>(hn * Value1);
            yield return BuffAction<KomachiModInfernoTempestSe>(Value2);

            yield break;
        }
    }
}


