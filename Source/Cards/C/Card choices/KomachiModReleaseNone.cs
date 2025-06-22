using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using KomachiMod.StatusEffects;

namespace KomachiMod.Cards
{
    public sealed class KomachiModReleaseNoneDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Illustrator = "";
            //If IsPooled is false then the card cannot be discovered or added to the library at the end of combat.
            config.HideMesuem = true;
            config.IsPooled = false;
            config.FindInBattle = false;

            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            config.RelativeEffects = new List<string>()
            {
                nameof(KomachiModReleaseKeyword),
                nameof(KomachiModGuidedSpiritSe)
            };
            config.UpgradedRelativeCards = new List<string>()
            {
                nameof(KomachiModReleaseKeyword),
                nameof(KomachiModGuidedSpiritSe)
            };

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModReleaseNoneDef))]
    public sealed class KomachiModReleaseNone : KomachiCard
    {
    }
}


