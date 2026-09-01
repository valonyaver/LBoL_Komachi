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

            config.Damage = 15;
            config.UpgradedDamage = 10;

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

            config.Illustrator = "Akyuun";

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

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // Calculate Mana Synergy
            int redManaSpent = base.SynergyAmount(consumingMana, ManaColor.Red, Mana.Total);

            // PERFORM ATTACK
            // Total hits = (Hits from Red Mana)
            Guns guns = new Guns(base.GunName, redManaSpent);
            foreach (GunPair gunPair in guns.GunPairs)
            {
                yield return AttackAction(selector, gunPair);
            }

            // APPLY MANA-BASED FIREPOWER AFTER ATTACK
            if (redManaSpent > 0)
            {
                yield return BuffAction<TempFirepower>(redManaSpent * Value1);
            }

            // Post-combat lily generation
            if (consumingMana.Total >= 3 || IsUpgraded)
            {
                yield return new AddCardsToDiscardAction(Library.CreateCard<KomachiModSpiderLily>());
            }

            yield break;
        }
    }
}


