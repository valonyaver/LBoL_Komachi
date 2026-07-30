using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
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
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace KomachiMod.Cards
{
    public sealed class KomachiModHalfLife3Def : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(4721);
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Red = 2, Black = 2 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 3, HybridColor = 7, Any = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;


            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile | Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Accuracy;

            config.Damage = 0;

            // Normal release cost
            config.Value1 = 10;
            config.UpgradedValue1 = 7;

            // Named release cost
            config.Value2 = 18;
            config.UpgradedValue2 = 15;

            config.RelativeEffects = new List<string>()
            {
                nameof(KomachiModReleaseKeyword),
                nameof(Spirit)
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(KomachiModReleaseKeyword)
            };

            config.Illustrator = "gibuchoko";
            // config.Unfinished = true;

            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModHalfLife3Def))]
    public sealed class KomachiModHalfLife3 : KomachiCard
    {
        public override bool Triggered
        {
            get
            {
                foreach (var enemy in Battle.AllAliveEnemies)
                {
                    // if any enemy that doesnt have a name exists, take the cheaper cost to glow it
                    if (!enemy.Config.RealName)
                    {
                        return KomachiModUtility.CanReleaseSpirits(Battle.Player, Value1);
                    }
                }
                // otherwise, take the higher cost to glow it
                return KomachiModUtility.CanReleaseSpirits(Battle.Player, Value2);
            }
        }
        // Spirit Loss
        protected override int BaseValue3 { get => 3; set => base.BaseValue3 = value; }
        protected override int BaseUpgradedValue3 { get => 0; set => base.BaseUpgradedValue3 = value; }

        bool targetingNamed;
        int halfTargetHP;
        //public override DamageInfo Damage
        //{
        //    get
        //    {
        //        if (Battle == null) return base.Damage;
        //        return DamageInfo.Attack(halfTargetHP, IsAccuracy);
        //    }
        //}

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<DamageDealingEventArgs>
                (base.Battle.Player.DamageDealing,
                new GameEventHandler<DamageDealingEventArgs>(this.OnPlayerDamageDealing), GameEventPriority.Lowest);
        }

        /// <summary>
        /// Checks the target to see if it has a name
        /// </summary>
        /// <param name="args"></param>
        private void OnPlayerDamageDealing(DamageDealingEventArgs args)
        {
            if (args.ActionSource == this && args.Targets != null)
            {
                EnemyUnit target = (EnemyUnit) args.Targets[0];
                if (target.Config.RealName)
                {
                    targetingNamed = true;
                }
                else targetingNamed = false;
                halfTargetHP = (int)(target.Hp / 2f).Round(System.MidpointRounding.AwayFromZero);
                args.DamageInfo = new DamageInfo(halfTargetHP, DamageType.Attack, isAccuracy:IsAccuracy);
                args.AddModifier(this);
            }
        }

        int releaseValue
        {
            get
            {
                if (targetingNamed) { return Value2; } else { return Value1; }
            }
        }
        public override Interaction Precondition()
        {
            int cost = releaseValue;
            if (!KomachiModUtility.CanReleaseSpirits(Battle.Player, cost))
            {
                return null;
            }
            // Create list for interaction
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModReleaseNone releaseNone = Library.CreateCard<KomachiModReleaseNone>();
            KomachiModHalfLife3 releaseCost1 = Library.CreateCard<KomachiModHalfLife3>(IsUpgraded);
            releaseCost1.targetingNamed = targetingNamed; // So that the card shows the correct cost.
            releaseCost1.ChoiceCardIndicator = 1; // uses extra description 1
            // dk what these do tbh.
            releaseNone.SetBattle(Battle);
            releaseCost1.SetBattle(Battle);
            // add em to the list
            list.Add(releaseNone);
            list.Add(releaseCost1);
            return new MiniSelectCardInteraction(list);
        }
        // consider guns: 4520 (junko aura), 40020 (reimu aura), 13091 (multiple knives), 13141 (knife slash) or (15110 to 15113), 13201 (deflation?) then end with 4721 (pretty butterflies)
        public string gun1 = GunNameID.GetGunFromId(4520);
        public string gun2 = GunNameID.GetGunFromId(40020);
        public string gun3 = GunNameID.GetGunFromId(13141);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card choiceCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(choiceCard))
            {
                yield return PerformAction.Spell(Battle.Player, "KomachiModUltHalfLife");
                yield return PerformAction.Gun(Battle.Player, selector.SelectedEnemy, gun1, 2.5f);
                yield return PerformAction.Gun(Battle.Player, selector.SelectedEnemy, gun2, 2);
                float occupationTime = 1.5f;
                float decreaseTime = 0.1f;
                if (!targetingNamed) decreaseTime = 0.2f;
                int poemIndex = 0;
                for (int i = 0; i < releaseValue; i++)
                {
                    Debug.Log(poem[poemIndex]);
                    poemIndex++;
                    yield return new KomachiReleaseAction(this, 1);
                    yield return PerformAction.Gun(Battle.Player, selector.SelectedEnemy, gun3, occupationTime);
                    if (occupationTime > 0.1f) occupationTime -= 0.1f;
                }
                if (IsUpgraded)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Debug.Log(poem[poemIndex]);
                        poemIndex++;
                        yield return PerformAction.Gun(Battle.Player, selector.SelectedEnemy, gun3, 0.1f);
                    }
                }
                // Another one for good measure
                Debug.Log(poem[poemIndex]);
                yield return PerformAction.Gun(Battle.Player, selector.SelectedEnemy, gun3, 0.1f);
                //yield return new KomachiReleaseAction(this, releaseValue);
                int damage = (int) (selector.SelectedEnemy.Hp / 2f).Round(System.MidpointRounding.AwayFromZero);
                yield return AttackAction(selector.SelectedEnemy, DamageInfo.Attack(damage, IsAccuracy), GunName);
            }
            if (Value3 > 0)
            {
                yield return BuffAction<SpiritNegative>(Value3);
            }
            yield break;
        }

        public string[] poem = new string[]
        {
            "There she stands, who rules the dreary coast-",
            "A sordid god: down from her hoary chin",
            "A length of hair descends, uncomb'd, unclean,",
            "Her eyes, like hollow furnaces on fire;",
            "An obi, foul with grease, binds her lazy attire.",
            "She spreads her canvas; with her pole she steers;",
            "The freights of flitting ghosts in her thin bottom bears.",
            "She look'd in years; yet in her years were seen", 
            "A youthful vigor and autumnal green.", 
            "An airy crowd came rushing where she stood, ",
            "Which fill'd the margin of the fatal flood: ", // end of unupgraded nonnamed 11
            "Husbands and wives, boys and unmarried maids,",
            "And mighty heroes' more majestic shades,",
            "And youths, intomb'd before their fathers' eyes,",
            "With hollow groans, and shrieks, and feeble cries.",
            "Thick as the leaves in autumn strow the woods,",
            "Or fowls, by winter forc'd, forsake the floods,",
            "And wing their hasty flight to happier lands;",
            "Such, and so thick, the shiv'ring army stands,",
            "And press for passage with extended hands.",
            "Now these, now those, the surly boatman bore.", // end of unupgraded named 21
            "The rest she drove to distance from the shore.",
            "The hero, who beheld with wond'ring eyes",
            "The tumult mix'd with shrieks, laments, and cries,",
            "Ask'd of her guide, what the rude concourse meant;",
            "Why to the shore the thronging people bent;",
            "What forms of law among the ghosts were us'd;",
            "Why some were ferried o'er, and some refus'd.", // end of upgraded named 28
            "\"Daughter of Hakurei, offspring of the gods,\"",
            "The Sibyl said, \"you see the Stygian floods,",
            "The sacred stream which heav'n's imperial state",
            "Attests in oaths, and fears to violate.",
            "The ghosts rejected are th' abandoned crew",
            "Depriv'd of coin, none who remembered knew:",
            "The boatman, Onozuka; those, the wealthy host,",
            "She ferries over to the farther coast; " // end
        };
    }
}


