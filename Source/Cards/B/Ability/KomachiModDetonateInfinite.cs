using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModDetonateInfiniteDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Black = 1, Any = 2 };

            config.Rarity = Rarity.Rare;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.SingleEnemy;

            // Spirits inflicted
            config.Value1 = 4;
            config.UpgradedValue1 = 6;

            // Buff amount
            config.Value2 = 4;
            config.UpgradedValue2 = 6;

            config.RelativeEffects = new List<string>()
            { 
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiDetonationKeyword)
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiDetonationKeyword)
            };


            config.Illustrator = "NaufalDreamer";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModDetonateInfiniteDef))]
    public sealed class KomachiModDetonateInfinite : KomachiCard
    {
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new ApplyVengefulSpiritAction(this,selector.SelectedEnemy, Value1);
            yield return BuffAction<KomachiModDetonateInfiniteSe>(Value2);
            yield break;
        } 
    }
}


