using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.Source.StatusEffects.Spirits;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Source.Cards
{
    public sealed class KomachiModGhostrickShotDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(7001);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Spirit apply
            config.Value1 = 8;
            config.UpgradedValue1 = 10;

            // Spirits Applied on exile
            config.Value2 = 3;
            config.UpgradedValue2 = 5;

            config.RelativeEffects = new List<string>() 
            { nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };

            config.Keywords = Keyword.Ethereal;
            config.UpgradedKeywords = Keyword.Ethereal;


            config.Illustrator = "三月アクア";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModGhostrickShotDef))]
    public sealed class KomachiModGhostrickShot : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new ApplyVengefulSpiritAction(this, selector.SelectedEnemy, Value1);
            yield break;
        }

        public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
        {
            foreach (var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(this, enemy, Value2);
            }
        }
    }
}


