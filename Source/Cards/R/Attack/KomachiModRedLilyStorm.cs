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
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModRedLilyStormDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            // I want an aoe red gun. Possible gunnames: 12200, 23010, 23051, 60000, 60003
            config.GunName = GunNameID.GetGunFromId(4150);

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            //Mana to consume aside from X
            config.Cost = new ManaGroup() { Red = 2, Any = 0 };
            config.Rarity = Rarity.Uncommon;

            //The XCost has to be set.
            config.IsXCost = true;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.AllEnemies;

            config.Damage = 12;
            config.UpgradedDamage = 5;

            // Amount of firepower per X mana cost
            config.Value1 = 1;

            // Amount of vengeful or divine spirits per mana
            config.Value2 = 4;
            config.UpgradedValue2 = 6;

            config.Mana = new ManaGroup() { Red = 2 };
            config.UpgradedMana = new ManaGroup() { Red = 1 };

            config.RelativeCards = new List<string>()
            {
                nameof(KomachiModSpiderLily)
            };
            config.UpgradedRelativeCards = new List<string>()
            {
                nameof(KomachiModSpiderLily)
            };

            config.RelativeEffects = new List<string>()
            {
                nameof(TempFirepower)
            };

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModRedLilyStormDef))]
    public sealed class KomachiModRedLilyStorm : KomachiCard
    {
        public int redManaSpent
        {
            get
            {
                if (Battle == null || PendingManaUsage == null) return 0;
                return SynergyAmount(PendingManaUsage.Value, ManaColor.Red, 1) * Value1;
            }
        }

        public override ManaGroup GetXCostFromPooled(ManaGroup pooledMana)
        {
            ManaGroup result = default(ManaGroup);
            result.Red = pooledMana.Red;
            result.Philosophy = pooledMana.Philosophy;
            return result;
        }

        public override Interaction Precondition()
        {
            if (Battle.HandZone.Any(c => c.GetType() == typeof(KomachiModSpiderLily)) || Battle.DiscardZone.Any(c => c.GetType() == typeof(KomachiModSpiderLily)))
            {
                List<Card> cards = new List<Card>();

                KomachiModRedLilyStorm refuse = Library.CreateCard<KomachiModRedLilyStorm>(IsUpgraded);
                refuse.SetBattle(Battle);
                refuse.ChoiceCardIndicator = 1;

                KomachiModRedLilyStorm accept = Library.CreateCard<KomachiModRedLilyStorm>(IsUpgraded);
                accept.SetBattle(Battle);
                accept.ChoiceCardIndicator = 2;

                cards.Add(refuse);
                cards.Add(accept);

                return new MiniSelectCardInteraction(cards);
            }
            else return null;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // Exile lilies
            Card precon = KomachiModUtility.GetPreconditionCard(precondition);
            int redliliesCount = 0;
            int redLiliesFP = 0;
            if (precon != null && precon.ChoiceCardIndicator == 2)
            {
                List<Card> lilies = Battle.HandZone.Concat(Battle.DiscardZone)
                  .Where(card => card.GetType() == typeof(KomachiModSpiderLily)).ToList();

                foreach (var lily in lilies)
                {
                    if (lily.IsUpgraded)
                    {
                        redLiliesFP += 3;
                    }
                    else redLiliesFP++;
                    redliliesCount++;
                    yield return new ExileCardAction(lily);
                }
                //yield return new ExileManyCardAction(lilies);
            }

            if (IsUpgraded) redliliesCount *= 2;

            // Get the synergy
            int synergyCost = Mana.Total;

            int red = base.SynergyAmount(consumingMana, ManaColor.Red, synergyCost);

            // For every synergy and for every lily banished do the thingy thing
            yield return BuffAction<TempFirepower>((red + redLiliesFP) * Value1);
            Guns guns = new Guns(base.GunName, red + redliliesCount);
            foreach (GunPair gunPair in guns.GunPairs)
            {
                yield return AttackAction(selector, gunPair);
            }
            if (consumingMana.Total > 3 || IsUpgraded)
            {
                var discardLily = Library.CreateCard<KomachiModSpiderLily>();
                yield return new AddCardsToDiscardAction(discardLily);
            }
            yield break;
        }
    }
}


