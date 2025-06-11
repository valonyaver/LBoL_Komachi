using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModSweepDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);
            config.IsPooled = true;

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1, Any = 2 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.AllEnemies;

            config.Damage = 10;
            config.UpgradedDamage = 15;

            // Value of the Displacement. Can displace up to +/- value1
            config.Value1 = 1;
            // Amount of Manipulate distance to add to hand.
            config.Value2 = 1;
            config.UpgradedValue2 = 2;

            // config.Keywords = Keyword.Displace;
            // config.UpgradedKeywords = Keyword.Displace;

            config.Illustrator = "@TheIllustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModManDistance) };

            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSweepDef))]
    public sealed class KomachiModSweep : KomachiCard
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            //Attack all enemies, selector is set to Battle.AllAliveEnemies.
            yield return base.AttackAction(selector, base.GunName);
            //If the battle were to end, skip the rest of the method.
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            //Apply Displacement to all alive enemies.
            foreach (BattleAction battleAction in KomachiDistanceSe.ChangeDistanceLevel(base.Battle.AllAliveEnemies, -base.Value1))
            {
                yield return battleAction;
            }

            for (int i = 0; i < Value2; i++)
            {
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModManDistance>() });
            }
            yield break;
        }
    }
}


