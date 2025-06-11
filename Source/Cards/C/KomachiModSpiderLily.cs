using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.Cards.Character.Marisa;

namespace KomachiMod.Cards
{
    public sealed class KomachiModSpiderLilyDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            //If IsPooled is false then the card cannot be discovered or added to the library at the end of combat.
            config.IsPooled = false;

            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            config.Value1 = 2;
            config.Mana = new ManaGroup() { Red = 2 };
            config.UpgradedMana = new ManaGroup() { Red = 2, Philosophy = 1 };

            config.Keywords = Keyword.Exile | Keyword.Retain;
            //Setting Upgrading Keyword only provides the keyword when the card is upgraded.    
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;

            config.Illustrator = "";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSpiderLilyDef))]
    public sealed class KomachiModSpiderLily : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new GainManaAction(base.Mana);
			yield break;
		}
    }
}


