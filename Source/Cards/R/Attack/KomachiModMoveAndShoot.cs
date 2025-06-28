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
            config.GunName = GunNameID.GetGunFromId(400);
            config.IsPooled = true;

            config.ImageId = "KomachiAttackR";

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1, Any = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 14;
            config.UpgradedDamage = 20;

            // Value of the Displacement. Can displace up to +/- value1
            config.Value1 = 1;
            config.UpgradedValue1 = 2;

            // config.Keywords = Keyword.Displace;
            // config.UpgradedKeywords = Keyword.Displace;

            config.Illustrator = "@TheIllustrator";

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
        int[] damageLevels = new int[5];

        // Bruh all this work just to have correct damage calculations for enemies when selecting the distance
        protected override void OnEnterBattle(BattleController battle)
        {
            // ttach to player targeting
            base.HandleBattleEvent<DamageDealingEventArgs>
                (base.Battle.Player.DamageDealing,
                new GameEventHandler<DamageDealingEventArgs>(this.OnPlayerDamageDealing), GameEventPriority.Lowest);

            // attach to enemy being targeted
            foreach (var enemy in battle.AllAliveEnemies)
            {
                base.HandleBattleEvent<DamageEventArgs>
                    (enemy.DamageReceiving, new GameEventHandler<DamageEventArgs>(OnEnemyDamageReceiving), GameEventPriority.Lowest);
            }
            HandleBattleEvent<UnitEventArgs>
                (battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
        }
        // attach again to enemies being spawned
        void OnEnemySpawned(UnitEventArgs args)
        {
            base.HandleBattleEvent<DamageEventArgs>
                (args.Unit.DamageReceiving, new GameEventHandler<DamageEventArgs>(OnEnemyDamageReceiving), GameEventPriority.Lowest);
        }
        // You see, I want the damage when the card is targeting an enemy, including all the modifications
        // This only gives off the base damage of the card.
        private void OnPlayerDamageDealing(DamageDealingEventArgs args)
        {
            if (args.Targets == null || args.ActionSource == null) return;
            if (args.ActionSource == this)
            {
                lastTargeter = this;
            }
            else if (args.ActionSource.GetType() != typeof(KomachiModMoveAndShoot)) lastTargeter = null;
        }
        // BUT FOR SOME REASON, THIS ONE DOESNT HAVE AN ACTION CAUSE. its cause is "only calculating". tf do you mean only calculating?
        // SO i have to make sure this class remembers that the last card that targeted an enemy is this card, and if this card is deadling the damage
        // we store that damage in the array
        private void OnEnemyDamageReceiving(DamageEventArgs args)
        {
            if (lastTargeter == this)
            {
                Unit target = args.Target;
                int distanceLevel = KomachiDistanceSe.GetDistanceLevel(target);
                int[] distanceLevelPossibilities = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    distanceLevelPossibilities[i] = Math.Clamp(distanceLevel + i - 2, 1, 5);
                    damageLevels[i] = Mathf.RoundToInt(
                        args.DamageInfo.Damage * KomachiDistanceSe.GetDistanceDamageMultiplier(distanceLevelPossibilities[i])
                        / KomachiDistanceSe.GetDistanceDamageMultiplier(distanceLevel)); // divides the current distance multiplier so that we can apply the hypothetical multiplier by itself.
                }
            }
        }
        

        /// <summary>
        /// From the damages we got in the array, we calculate what the damage will be in every possible distance that this card can bring the enemy to.
        /// You better appreciate this card
        /// </summary>
        /// <returns></returns>
        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list1 = new List<Card>();
            if (this.IsUpgraded)
            {
                // notice how they are MAN DISTANCE 2?
                KomachiModManDistance2 manipulateDistancePull2 = Library.CreateCard<KomachiModManDistance2>(upgraded: true);
                manipulateDistancePull2.ChoiceCardIndicator = 1; // uses extra description 1 of mandistance2
                manipulateDistancePull2.SetBattle(base.Battle);
                string damageColor0 = KomachiModUtility.GetColorFromDamage(damageLevels[0], Damage.Damage);
                manipulateDistancePull2.extraDescriptionAddition = $"Deal <color=#{damageColor0}>{damageLevels[0]}</color> damage.";
                list1.Add(manipulateDistancePull2);
            }
            // make the 2 cards
            KomachiModManDistance manipulateDistancePull1 = Library.CreateCard<KomachiModManDistance>();
            KomachiModManDistance0 manipulateDistance0 = Library.CreateCard<KomachiModManDistance0>();
            KomachiModManDistance manipulateDistancePush1 = Library.CreateCard<KomachiModManDistance>();
            // indicate them
            manipulateDistancePull1.ChoiceCardIndicator = 1; // uses extra description 1
            manipulateDistance0.ChoiceCardIndicator = 1; // uses extra description 1
            manipulateDistancePush1.ChoiceCardIndicator = 2; // uses extra description 2
            // extra description
            string damageColor1 = KomachiModUtility.GetColorFromDamage(damageLevels[1], Damage.Damage);
            manipulateDistancePull1.extraDescriptionAddition = $"Deal <color=#{damageColor1}>{damageLevels[1]}</color> damage.";
            string damageColor2 = KomachiModUtility.GetColorFromDamage(damageLevels[2], Damage.Damage);
            manipulateDistance0.extraDescriptionAddition = $"Deal <color=#{damageColor2}>{damageLevels[2]}</color> damage.";
            string damageColor3 = KomachiModUtility.GetColorFromDamage(damageLevels[3], Damage.Damage);
            manipulateDistancePush1.extraDescriptionAddition = $"Deal <color=#{damageColor3}>{damageLevels[3]}</color> damage.";
            // dk what these do tbh.
            manipulateDistancePull1.SetBattle(base.Battle);
            manipulateDistance0.SetBattle(base.Battle);
            manipulateDistancePush1.SetBattle(base.Battle);
            // add em to the list
            list1.Add(manipulateDistancePull1);
            list1.Add(manipulateDistance0);
            list1.Add(manipulateDistancePush1);
            if (this.IsUpgraded)
            {
                KomachiModManDistance2 manipulateDistancePush2 = Library.CreateCard<KomachiModManDistance2>(upgraded: true);
                manipulateDistancePush2.ChoiceCardIndicator = 2; // uses extra description 2
                string damageColor4 = KomachiModUtility.GetColorFromDamage(damageLevels[4], Damage.Damage);
                manipulateDistancePush2.extraDescriptionAddition = $"Deal <color=#{damageColor4}>{damageLevels[4]}</color> damage.";
                manipulateDistancePush2.SetBattle(base.Battle);
                list1.Add(manipulateDistancePush2);
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


