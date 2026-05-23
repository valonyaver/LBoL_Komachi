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
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModEqualizeDistanceDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Draw per displacement.
            config.Value1 = 2;
            config.UpgradedValue1 = 2;

            // Draws if Distance = 3
            config.Value2 = 3;
            config.UpgradedValue2 = 4;

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };

            config.UpgradedKeywords = Keyword.Echo;


            config.Illustrator = "しなぷう";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModEqualizeDistanceDef))]
    public sealed class KomachiModEqualizeDistance : KomachiCard
    {
        public int three = 3;
        // Discard if distance = 3
        protected override int BaseValue3 { get => 2; }
        protected override int BaseUpgradedValue3 { get => 2; }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int currentDistance = KomachiModDistanceSe.GetDistanceLevel(selector.SelectedEnemy);
            if (currentDistance == 3)
            {
                yield return new DrawManyCardAction(Value2);
                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
                if (base.Battle.HandZone.Count > base.Value3)
                {
                    SelectHandInteraction interaction = new SelectHandInteraction(base.Value3, base.Value3, base.Battle.HandZone)
                    {
                        Source = this
                    };
                    yield return new InteractionAction(interaction, false);
                    yield return new DiscardManyAction(interaction.SelectedCards);
                    interaction = null;
                }
                else
                {
                    yield return new DiscardManyAction(base.Battle.HandZone);
                }
            }
            else
            {
                int displacementAmount = 3 - currentDistance;
                yield return new DistanceChangeAction(selector.SelectedEnemy, displacementAmount);
                yield return new DrawManyCardAction(Mathf.Abs(displacementAmount) * Value1);
            }
            yield break;
        }
    }
}


