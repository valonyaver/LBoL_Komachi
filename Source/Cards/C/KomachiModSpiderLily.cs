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
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModSpiderLilyDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Illustrator = "Valonadthe";
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
            config.Value2 = 2;

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


            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSpiderLilyDef))]
    public sealed class KomachiModSpiderLily : KomachiCard
    {
        protected override int AdditionalValue2
        {
            get
            {
                if (Battle != null && Battle.Player.TryGetStatusEffect<KomachiModRiversideViewSe>(out var riversideView))
                {
                    // poisonAmount -= riversideView.Count;
                    return -1;
                }
                return 0;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new GainManaAction(base.Mana);
            yield return BuffAction<TempFirepower>(Value1);
            int poisonAmount = Value2;
            if (poisonAmount > 0)
            {
                yield return new ApplyStatusEffectAction<Poison>(Battle.Player, level: poisonAmount);
            }
			yield break;
		}
    }
}


