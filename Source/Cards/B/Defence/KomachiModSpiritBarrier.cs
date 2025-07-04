using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
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
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModSpiritBarrierDef : KomachiCardTemplate
    {


        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();

            // config.ImageId = nameof(KomachiModBlockB);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.Nobody;

            config.Block = 14;
            config.UpgradedBlock = 17;

            // Minimum amount of release needed
            config.Value1 = 1;

            // Barrier per spirit
            config.Value2 = 1;
            config.UpgradedValue2 = 2;

            config.Illustrator = "イセ川";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);

            config.RelativeEffects = new List<string>() 
            { nameof(KomachiModGuidedSpiritSe), nameof(KomachiModReleaseKeyword) };
            config.UpgradedRelativeEffects = new List<string>()
            { nameof(KomachiModGuidedSpiritSe), nameof(KomachiModReleaseKeyword) };
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSpiritBarrierDef))]
    public sealed class KomachiModSpiritBarrier : KomachiCard
    {
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, Value1);
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value1);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card releaseChoice = KomachiModUtility.GetPreconditionCard(precondition);
            yield return DefenseAction(true);
            if (releaseChoice != null && releaseChoice.GetType() != typeof(KomachiModReleaseNone))
            {
                int spiritLevel = Battle.Player.GetStatusEffect<KomachiModGuidedSpiritSe>().Level;
                yield return new KomachiReleaseAction(Battle.Player, spiritLevel);
                yield return DefenseAction(0, spiritLevel*Value2, BlockShieldType.Direct);
            }
        }
    }
}


