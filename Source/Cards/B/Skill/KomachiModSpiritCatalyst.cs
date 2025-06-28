using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
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

namespace KomachiMod.Cards
{
    public sealed class KomachiModSpiritCatalystDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2, Any = 1 };
            config.UpgradedCost = new ManaGroup() { Black = 1 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            // Duration increase
            config.Value1 = 3;

            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;

            config.RelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModVengefulSpiritSe) };


            config.Illustrator = "Wholesome_illustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSpiritCatalystDef))]
    public sealed class KomachiModSpiritCatalyst : KomachiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            KomachiModVengefulSpiritSe spirits = selector.SelectedEnemy.GetStatusEffect<KomachiModVengefulSpiritSe>();
            if (spirits == null)
            {
                yield break;
            }
            else
            {
                yield return new ApplyVengefulSpiritAction(selector.SelectedEnemy, spirits.Count);
                spirits.Duration += 3;
            }
        }
    }
}


