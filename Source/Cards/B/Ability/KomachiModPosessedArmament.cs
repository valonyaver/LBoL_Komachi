using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModPosessedArmamentDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // spirits inflicted
            config.Value1 = 2;
            config.UpgradedValue1 = 3;

            config.RelativeEffects = new List<string>()
            { 
                nameof(KomachiModVengefulSpiritSe)
            };
            config.UpgradedRelativeEffects = new List<string>() 
            { 
                nameof(KomachiModVengefulSpiritSe)
            };

            config.Illustrator = "yuuki eishi";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModPosessedArmamentDef))]
    public sealed class KomachiModPosessedArmament : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModPosessedArmamentSe>(base.Value1);
            yield break;
        }
    }
}


