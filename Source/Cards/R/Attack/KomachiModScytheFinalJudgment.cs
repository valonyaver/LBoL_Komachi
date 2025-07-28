using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.KomachiUlt;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.Red;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    /// <summary>
    /// Unused card. Used for early testing of the distance mechanic.
    /// </summary>
    public sealed class KomachiModScytheFinalJudgmentDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            // config.ImageId = nameof(KomachiModAttackR);
            // Other options are 4610, 4051, 6300, 39073, 15111
            // Honorable for other attacks. Putting them here for convenience, 25010
            // Really want something that comes from above.
            config.GunName = GunNameID.GetGunFromId(4660);
            config.IsPooled = true;

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2, Any = 2 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 22;
            config.UpgradedDamage = 28;

            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };

            config.Illustrator = "Xiirus";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModScytheFinalJudgmentDef))]
    public sealed class KomachiModScytheFinalJudgment : KomachiCard
    {
        public static Card lastTargeter;
        public Unit lastTargetedEnemy;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<DamageDealingEventArgs>
                (base.Battle.Player.DamageDealing, 
                new GameEventHandler<DamageDealingEventArgs>(this.OnPlayerDamageDealing), GameEventPriority.ConfigDefault);
        }

        /// <summary>
        /// This will fix the attack damage on the card when targeting a far enemy.
        /// </summary>
        /// <param name="args"></param>
        private void OnPlayerDamageDealing(DamageDealingEventArgs args)
        {
            if (args.ActionSource == this && args.Targets != null)
            {
                bool applyModifier = false;
                lastTargetedEnemy = args.Targets[0];
                // Old Modifier for when the card used to double against shielded enemies.
                //if (target.Block > 0 || target.Shield > 0)
                //{
                //    args.DamageInfo = args.DamageInfo.MultiplyBy(2);
                //    applyModifier = true;
                //}
                if (lastTargetedEnemy.TryGetStatusEffect(out KomachiModDistanceSe distanceLevel))
                {
                    switch (distanceLevel.Level)
                    {
                        case 4:
                        case 5:
                            // Divide the multiplier by 0.7 so that when later it gets multiplied by 0.7, it will cancel out. Damage * 1.6 * (0.7 / 0.7)
                            float inverseMultiplier = GetDoubleInverseDistanceMultiplier(distanceLevel.Level);
                            args.DamageInfo = args.DamageInfo.MultiplyBy(
                                inverseMultiplier / KomachiModDistanceSe.GetDistanceDamageMultiplier(distanceLevel.Level
                                ));
                            applyModifier = true;
                            break; 
                    }
                }
                if (applyModifier) args.AddModifier(this);
            }
        }

        // Returns double the inverse of the far distance multiplier. 0.85 becomes 1.3, 0.7 becomes 1.6
        public float GetDoubleInverseDistanceMultiplier(int distanceLevel)
        {
            switch (distanceLevel)
            {
                case 4:
                case 5:
                    // 1 - 0.7 becomes 0.3, multiply by 2 becomes 0.6. Add it to 1 it becomes 1.6.
                    float distanceMultiplier = KomachiModDistanceSe.GetDistanceDamageMultiplier(distanceLevel);
                    float inverseMultiplier = (1 + (1 - distanceMultiplier) * 2);
                    return inverseMultiplier;
                default:
                    return 1;
            }
        }

        public override Interaction Precondition()
        {
            List<Card> distanceOptions = new List<Card>();
            int currentDistance = KomachiModDistanceSe.GetDistanceLevel(lastTargetedEnemy);

            // Find the damage with all multipliers that would be applied if attacking this enemy.
            float regularDamage = KomachiModUtility.FindDamageDealt(this, lastTargetedEnemy, Damage, Battle);
            // If distance high, divide by the inverse multiplier since that will be what's applied rather than distance.
            if (currentDistance >= 4)
            {
                regularDamage = regularDamage / GetDoubleInverseDistanceMultiplier(currentDistance);
            }
            else // Else divide normally.
            {
                regularDamage = regularDamage / KomachiModDistanceSe.GetDistanceDamageMultiplier(currentDistance);
            }
            // Create cards for all 5 possible distances (1-5)
            for (int targetDistance = 1; targetDistance <= 5; targetDistance++)
            {
                

                // Get appropriate card type using the template
                KomachiModManDistanceTemplate distanceCard;

                // Set as "current distance" card if no movement
                if (targetDistance == currentDistance)
                {
                    distanceCard = Library.CreateCard<KomachiModManDistance0>();
                    distanceCard.ChoiceCardIndicator = 1;
                }
                else // Else find the correct displacement.
                {
                    // Determine card type based on distance difference
                    int distanceDiff = targetDistance - currentDistance;
                    int cardTypeValue = Math.Abs(distanceDiff);
                    distanceCard = (KomachiModManDistanceTemplate)KomachiModManDistanceTemplate.manDistanceType(cardTypeValue);
                    // Set direction indicator (1 = reduce distance, 2 = increase distance)
                    distanceCard.ChoiceCardIndicator = (targetDistance < currentDistance) ? 1 : 2;
                }

                // Set card
                distanceCard.SetBattle(base.Battle);

                // Copy the regular damage.
                float damage = regularDamage;
                // Get the right multiplications for the target distance.
                if (targetDistance >= 4)
                {
                    damage = damage * GetDoubleInverseDistanceMultiplier(targetDistance);
                }
                else
                {
                    damage = damage * KomachiModDistanceSe.GetDistanceDamageMultiplier(targetDistance);
                }
                // Round to int
                int finalDamage = damage.RoundToInt(MidpointRounding.AwayFromZero);
                // Get the colour if the damage is higher or lower.
                string damageColor = KomachiModUtility.GetColorFromDamage(finalDamage, Damage.Damage);

                // Finally write the description WOO
                distanceCard.extraDescriptionAddition =
                    $"Deal {KomachiModUtility.GetColoredText(finalDamage.ToString(), damageColor)} damage.";

                distanceOptions.Add(distanceCard);
            }

            return new MiniSelectCardInteraction(distanceOptions);
        }


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return PerformAction.Spell(Battle.Player, nameof(KomachiModUltFinalJudgement));
            Card card = KomachiModUtility.GetPreconditionCard(precondition);
            if (card != null || card.GetType() != typeof(KomachiModManDistance0))
            {
                // value 1 of mandistance2 is 2. value1 of mandistance 1 is 1
                // so whatever card is picked, take its value1.
                if (card.ChoiceCardIndicator == 1)
                {
                    // if it's card choice 1, pull enemy closer for a smooch
                    yield return new DistanceChangeAction(selector.SelectedEnemy, -card.Value1);
                }
                else
                {
                    // otherwise push them away like an introvert
                    yield return new DistanceChangeAction(selector.SelectedEnemy, card.Value1);
                }
            }
            yield return base.AttackAction(selector.SelectedEnemy);

            if (selector.SelectedEnemy.IsDead)
            {
                Card[] lilies = Library.CreateCards<KomachiModSpiderLily>(Value1).ToArray();
                yield return new AddCardsToHandAction(lilies);
            }
            yield break;
        }
    }
}


