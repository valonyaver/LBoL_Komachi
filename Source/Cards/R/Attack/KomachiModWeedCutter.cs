using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModWeedCutterDef : KomachiCardTemplate
    {


        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(25181);
            // config.ImageId = nameof(KomachiModAttackR);

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };

            config.Rarity = Rarity.Common;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 8;

            // Draw amount
            config.Value1 = 1;

            config.Illustrator = "@makuwauri_uri";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.RelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModManDistance) };
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModWeedCutterDef))]
    public sealed class KomachiModWeedCutter : KomachiCard
    {
         
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector.SelectedEnemy);

            yield return new DrawManyCardAction(Value1);
            yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModManDistance>() });
        }
    }
}


