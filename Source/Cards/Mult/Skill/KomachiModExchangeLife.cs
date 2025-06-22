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
using System.ComponentModel.Design;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModExchangeLifeDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Red = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 7 };
            config.Rarity = Rarity.Rare;
            config.FindInBattle = false;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModExchangeLifeDef))]
    public sealed class KomachiModExchangeLife : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int enemyOriginalHP = selector.SelectedEnemy.Hp;
            int playerOriginalHP = Battle.Player.Hp;
            int difference = Mathf.Abs(enemyOriginalHP - playerOriginalHP);
            if (enemyOriginalHP <= Battle.Player.MaxHp && difference > 0)
            {
                if (enemyOriginalHP > playerOriginalHP)
                {
                    yield return new DamageAction(Battle.Player, selector.SelectedEnemy, DamageInfo.HpLose(difference));
                    yield return HealAction(difference);

                    Card deckCardByInstanceId = base.GameRun.GetDeckCardByInstanceId(base.InstanceId);
                    if (deckCardByInstanceId != null)
                    {
                        base.GameRun.RemoveDeckCard(deckCardByInstanceId, false);
                    }
                    yield return new RemoveCardAction(this);
                    yield break;
                }
                else
                {
                    yield return DamageSelfAction(difference);
                    yield return new HealAction(Battle.Player, selector.SelectedEnemy, difference);
                }
            }
            yield break;
        }
    }
}


