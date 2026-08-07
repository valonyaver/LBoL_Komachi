using BepInEx;
using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.Source.StatusEffects.Spirits;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    /// <summary>
    /// It's actually an ability not a block card
    /// </summary>
    public sealed class KomachiModDivineSpiritDefenceDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();

            // config.ImageId = nameof(KomachiModBlockB);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1};
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Defense;
            config.TargetType = TargetType.Nobody;

            config.Block = 4;
            config.UpgradedBlock = 6;

            // Divine spirit gain
            config.Value1 = 6;
            config.UpgradedValue1 = 8;

            // Release cost
            config.Value2 = 4;

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;

            config.RelativeEffects = new List<string>()
            { nameof(KomachiModDivineSpiritSe), nameof(KomachiModReleaseKeyword) };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiModDivineSpiritSe), nameof(KomachiModReleaseKeyword) };

            config.Illustrator = "雪降ノ森(S.F.)";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModDivineSpiritDefenceDef))]
    public sealed class KomachiModDivineSpiritDefence : KomachiCard
    {
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, Value2);
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return BuffAction<KomachiModDivineSpiritSe>(base.Value1, 0, 0, 0, 0.2f);
            Card choiceCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(choiceCard))
            {
                yield return new KomachiReleaseAction(this, Value2);
                yield return BuffAction<KomachiModDivineSpiritSe>(base.Value1, 0, 0, 0, 0.2f);
            }
            yield break;
        }
    }
}


