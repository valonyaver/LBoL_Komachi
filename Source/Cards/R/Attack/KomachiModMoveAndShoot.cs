using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModMoveAndShootDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(6061);
            config.IsPooled = true;

            // config.ImageId = nameof(KomachiModAttackR);

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1, Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 12;
            config.UpgradedDamage = 16;

            // Value of the Displacement. Can displace up to +/- value1
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            // config.Keywords = Keyword.Displace;
            // config.UpgradedKeywords = Keyword.Displace;

            config.Illustrator = "sakimiya@土曜つ28a";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.Unfinished = true;
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModMoveAndShootDef))]
    public sealed class KomachiModMoveAndShoot : KomachiCard
    {
        public static Card lastTargeter;

        // Bruh all this work just to have correct damage calculations for enemies when selecting the distance
        protected override void OnEnterBattle(BattleController battle)
        {
            // ttach to player targeting
            base.HandleBattleEvent<DamageDealingEventArgs>
                (base.Battle.Player.DamageDealing,
                new GameEventHandler<DamageDealingEventArgs>(this.OnPlayerDamageDealing), GameEventPriority.Lowest);

        }
        // You see, I want the damage when the card is targeting an enemy, including all the modifications
        // This only gives off the base damage of the card.
        private void OnPlayerDamageDealing(DamageDealingEventArgs args)
        {
            if (args.Targets == null || args.ActionSource == null) return;
            if (args.ActionSource == this)
            {
                lastTarget = args.Targets[0];
            }
        }

        Unit lastTarget;

        /// <summary>
        /// From the damages we got in the array, we calculate what the damage will be in every possible distance that this card can bring the enemy to.
        /// You better appreciate this card
        /// </summary>
        /// <returns></returns>
        public override Interaction Precondition()
        {
            List<Card> list1 = new List<Card>();

            // Create arrays of card types, indicators, and damage levels
            // Indicator 1 is pull, 2 is push
            var cardConfigs = new List<(Type cardType, int indicator, int damageIndex, bool upgraded)>
            {
              (typeof(KomachiModManDistance), 1, 1, false),   // Pull 1
              (typeof(KomachiModManDistance0), 1, 2, false), // Distance 0
              (typeof(KomachiModManDistance), 2, 3, false)   // Push 1
            };

            // Add upgraded cards if applicable
            if (this.IsUpgraded)
            {
                cardConfigs.Insert(0, (typeof(KomachiModManDistance2), 1, 0, true)); // Pull 2
                cardConfigs.Add((typeof(KomachiModManDistance2), 2, 4, true));      // Push 2
            }

            Unit target = lastTarget;
            int distanceLevel = KomachiModDistanceSe.GetDistanceLevel(target);

            int baseDamageDealt = Battle.CalculateDamage(this, Battle.Player, target, DamageInfo.Attack(Damage.Damage));

            // Process all cards in a loop
            foreach (var config in cardConfigs)
            {
                Card card;

                card = Library.CreateCard(config.cardType, upgraded: config.upgraded);

                card.Keywords = Keyword.None;

                int distanceLevelPossibility = Math.Clamp(distanceLevel + config.damageIndex - 2, 1, 5);
                // divides the current distance multiplier so that we can apply the hypothetical multiplier by itself.
                float realDamage = MathF.Round(
                    baseDamageDealt * KomachiModDistanceSe.GetDistanceDamageMultiplier(distanceLevelPossibility)
                    / KomachiModDistanceSe.GetDistanceDamageMultiplier(distanceLevel)
                    , MidpointRounding.AwayFromZero); 

                card.ChoiceCardIndicator = config.indicator;
                string damageColor = KomachiModUtility.GetColorFromDamage(realDamage, Damage.Damage);
                string damageColoredText = KomachiModUtility.GetColoredText(realDamage.ToString(), damageColor);
                string damageText = ExtraDescription1;
                damageText = damageText.Replace("VALUE", damageColoredText);

                ((KomachiModManDistanceTemplate)card).extraDescriptionAddition = damageText;
                card.SetBattle(base.Battle);
                list1.Add(card);
            }

            return new MiniSelectCardInteraction(list1);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = ((miniSelectCardInteraction != null) ? miniSelectCardInteraction.SelectedCard : null);
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
            yield return base.AttackAction(selector, base.GunName);
            Debug.Log(Description);
            
            yield break;
        }
    }
}


