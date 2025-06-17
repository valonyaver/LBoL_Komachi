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
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Others;

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

            // Firepower gain
            config.Value1 = 1;
            config.UpgradedValue1 = 3;

            // Poison gain
            config.Value2 = 3;

            config.Mana = new ManaGroup() { Red = 2 };
            config.UpgradedMana = new ManaGroup() { Philosophy = 2 };

            config.Keywords = Keyword.Exile | Keyword.Retain | Keyword.Replenish;
            //Setting Upgrading Keyword only provides the keyword when the card is upgraded.    
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain | Keyword.Replenish;

            config.RelativeEffects = new List<string>()
            {
                nameof(Poison), nameof(TempFirepower)
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(Poison), nameof(TempFirepower)
            };

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
            yield return BuffAction<TempFirepower>(Value1);
            yield return new ApplyStatusEffectAction<Poison>(Battle.Player, level: Value2);
			yield break;
		}
    }
}


