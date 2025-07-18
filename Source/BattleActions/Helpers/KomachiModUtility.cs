using KomachiMod.Cards;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.Profiling;
using UnityEngine;

namespace KomachiMod.Source.BattleActions.Helpers
{
    public sealed class KomachiModUtility
    {
        public static string lessDamageColor = "FF99FF";
        public static string normalValueColor = "B2FFFF";
        public static string increasedDamageColor = "FF9400";
        public static string KeywordColor = "EFC751";
        /// <summary>
        /// Returns a color depending on the value of value and whether it's less than or higher than comparingValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparingValue"></param>
        /// <returns></returns>
        public static string GetColorFromDamage(float value, float comparingValue)
        {
            if (value < comparingValue) return lessDamageColor;
            else if (value == comparingValue) return normalValueColor;
            else return increasedDamageColor;
        }

        public static string GetColorFromDamage(DamageInfo damage, float comparingValue)
        {
            float value = damage.Damage;
            if (value < comparingValue) return lessDamageColor;
            else if (value == comparingValue) return normalValueColor;
            else return increasedDamageColor;
        }

        public static string GetColoredText(string text, string color)
        {
            return $"<color=#{color}>{text}</color>";
        }


        public static bool CanReleaseSpirits(Card card, int amount)
        {
            Unit unit = card.Battle.Player;
            return CanReleaseSpirits(unit, amount);
        }


        /// <summary>
        /// Returns true if player has an equal or higher level of guided spirits.
        /// Used for figuring out whether a player can release or not.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="requiredAmount"></param>
        /// <returns></returns>
        public static bool CanReleaseSpirits(Unit unit, int requiredAmount)
        {
            if (unit.HasStatusEffect<KomachiModEikiSe>()) return true;
            KomachiModGuidedSpiritSe spirits;
            KomachiModDivineSpiritSe divineSpirits;
            unit.TryGetStatusEffect(out spirits);
            unit.TryGetStatusEffect(out divineSpirits);
            int spiritLevel = 0;
            int divineSpiritLevel = 0;
            if (spirits != null) spiritLevel = spirits.Level;
            if (divineSpirits != null) divineSpiritLevel = divineSpirits.Level;
            int totalAmount = spiritLevel + divineSpiritLevel;
            if (totalAmount < requiredAmount) return false;
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Retuns a mini select card interaction with a don't release, release with cost1, and optionally, release with cost2.
        /// Cost2 should be higher than cost1.
        /// Remember to manually call the release action in the card's actions.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="cost1"></param>
        /// <param name="cost2"></param>
        /// <returns></returns>
        public static Interaction ChooseRelease(Card card, int cost1, int cost2 = 0)
        {
            var battle = card.Battle;
            if (!CanReleaseSpirits(battle.Player, cost1))
            {
                return null;
            }
            // Create list for interaction
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModReleaseNone releaseNone = Library.CreateCard<KomachiModReleaseNone>();
            var releaseCost1 = Library.CreateCard(card.GetType(), card.IsUpgraded);
            Debug.Log($"{releaseCost1.SelfName} has a type of {releaseCost1.GetType()} is having a release interaction with a cost {cost1}");
            releaseCost1.ChoiceCardIndicator = 1; // uses extra description 1
            // dk what these do tbh.
            releaseNone.SetBattle(battle);
            releaseCost1.SetBattle(battle);
            // add em to the list
            list.Add(releaseNone);
            list.Add(releaseCost1);
            if (cost2 > 0 && CanReleaseSpirits(battle.Player, cost2))
            {
                var releaseCost2 = Library.CreateCard(card.GetType(), card.IsUpgraded);
                releaseCost2.SetBattle(battle);
                releaseCost2.ChoiceCardIndicator = 2; // uses extra description 2
                list.Add(releaseCost2);
            }
            return new MiniSelectCardInteraction(list);
        }

        /// <summary>
        /// Shortcut bool that is "If this card isnt null or isnt the pick no release option"
        /// </summary>
        /// <param name="choiceCard"></param>
        /// <returns></returns>
        public static bool ChoseRelease(Card choiceCard)
        {
            return choiceCard != null && choiceCard.GetType() != typeof(KomachiModReleaseNone);
        }

        /// <summary>
        /// Shortcut function for releases that have 2 release costs. If the release happens, outsources the actual release cost to costResult.
        /// </summary>
        /// <param name="choiceCard"></param>
        /// <returns></returns>
        public static bool ChoseRelease(Card choiceCard, int cost1, int cost2, out int costResult)
        {
            bool choseReleaseResult = ChoseRelease(choiceCard);
            if (!choseReleaseResult)
            {
                costResult = 0;
                return false;
            }
            else if (choiceCard.ChoiceCardIndicator == 1)
            {
                costResult = cost1;
            }
            else
            {
                costResult = cost2;
            }
            return true;
        }

        /// <summary>
        /// Generic interaction for having an optional detonate option. 
        /// Choice card indicator is which extra description you want to use for the detonate choice.
        /// Also when making the if statement you gotta do 
        /// if (releaseChoice != null && !(releaseChoice.GetType() == typeof(KomachiModManDetonateToken)))
        /// lmao
        /// </summary>
        /// <param name="card"></param>
        /// <param name="choiceCardIndicator"></param>
        /// <returns></returns>
        public static Interaction ChooseDetonate(Card card, int choiceCardIndicator)
        {
            var battle = card.Battle;
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModDetonateToken dontexplode = Library.CreateCard<KomachiModDetonateToken>();
            var explode = Library.CreateCard(card.GetType(), card.IsUpgraded);
            // indicate them
            dontexplode.ChoiceCardIndicator = 1; // uses extra description 1
            dontexplode.chooseDontDetonate = true;
            explode.ChoiceCardIndicator = choiceCardIndicator; // uses extra description 2
            // dk what these do tbh.
            dontexplode.SetBattle(battle);
            explode.SetBattle(battle);
            // add em to the list
            list.Add(dontexplode);
            list.Add(explode);
            return new MiniSelectCardInteraction(list);
        }

        /// <summary>
        /// Assumes precondition returns a miniselectcard interaction.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public static Card GetPreconditionCard(Interaction precondition)
        {
            MiniSelectCardInteraction miniselect = (MiniSelectCardInteraction)precondition;
            if (miniselect == null) return null;
            return miniselect.SelectedCard;
        }

        public static int GetVengefulCount(Unit enemy, bool countLoneSpirit = true)
        {
            int result = 0;
            enemy.TryGetStatusEffect<KomachiModVengefulSpiritSe>(out var vengeful);
            if (vengeful != null)
            {
                result += vengeful.Count;
            }
            if (countLoneSpirit)
            {
                enemy.TryGetStatusEffect<KomachiModLonelyBoundSpiritSe>(out var lonely);
                if (lonely != null)
                {
                    result += lonely.Count;
                }
            }
            return result;
        }

        public static float FindDamageDealt(GameEntity source, Unit target, DamageInfo damage, BattleController Battle)
        {
            EnemyUnit enemyUnit = (EnemyUnit) target;
            Unit[] array = new Unit[] { enemyUnit };
            DamageDealingEventArgs DealingArgs = new DamageDealingEventArgs
            {
                Source = Battle.Player,
                Targets = array,
                DamageInfo = damage,
                ActionSource = source
            };
            Battle.Player?.DamageDealing.Execute(DealingArgs);
            DamageInfo damage2 = DealingArgs.DamageInfo;
            DamageEventArgs DamageArgs = new DamageEventArgs
            {
                Source = Battle.Player,
                Target = enemyUnit,
                DamageInfo = damage2,
                ActionSource = source
            };
            enemyUnit?.DamageReceiving.Execute(DamageArgs);

            int finalDamage = Math.Max((int)DamageArgs.DamageInfo.Damage.Round(MidpointRounding.AwayFromZero), 0);
            return finalDamage;
        }
    }
}
