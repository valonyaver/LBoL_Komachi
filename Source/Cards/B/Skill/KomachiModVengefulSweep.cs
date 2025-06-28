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
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModVengefulSweepDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Spirits inflicted
            config.Value1 = 6;
            config.UpgradedValue1 = 9;

            // Release cost
            config.Value2 = 4;

            config.RelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };


            config.Illustrator = "Wholesome_illustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModVengefulSweepDef))]
    public sealed class KomachiModVengefulSweep : KomachiCard
    {
        public override bool Triggered
        {
            get
            {
                return KomachiModUtility.CanReleaseSpirits(Battle.Player, Value2);
            }
        }
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2);
        }
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(enemy, Value1);
            }
            Card releaseChoice = KomachiModUtility.GetPreconditionCard(precondition);
            if (releaseChoice != null && releaseChoice.GetType() != typeof(KomachiModReleaseNone))
            {
                yield return new KomachiReleaseAction(Battle.Player, Value2);
                foreach (var enemy in Battle.AllAliveEnemies)
                {
                    yield return new ApplyVengefulSpiritAction(enemy, Value1);
                }
            }
            yield break;
        } 
    }
}


