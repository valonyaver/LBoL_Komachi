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
using LBoL.Core.Units;
using KomachiMod.BattleActions;
using LBoL.Core.StatusEffects;

namespace KomachiMod.Cards
{
    /// <summary>
    /// Unused card. Used for early testing of the distance mechanic.
    /// </summary>
    public sealed class KomachiModLowTierShinigamiDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig() 
        {
            CardConfig config = GetCardDefaultConfig();
            // 4532 (Junko aura) then 25000 (thunder attack)
            config.GunName = GunNameID.GetGunFromId(25000);
            config.GunNameBurst = GunNameID.GetGunFromId(25001);

            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            config.Cost = new ManaGroup() { Hybrid = 2, Any = 1, HybridColor = 7 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 20;
            config.UpgradedDamage = 26;

            // Value of the Displacement. Can displace up to +/- value1
            config.Value1 = 99;

            config.Keywords = Keyword.Accuracy | Keyword.Exile;
            config.UpgradedKeywords = Keyword.Accuracy | Keyword.Exile;
            config.RelativeEffects = new List<string>() { nameof(Vulnerable) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(Vulnerable) };

            config.Illustrator = "@TheIllustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModLowTierShinigamiDef))]
    public sealed class KomachiModLowTierShinigami : KomachiCard
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // I want Komachi to tell me to kill myself
            yield return PerformAction.Gun(Battle.Player, selector.SelectedEnemy, GunNameID.GetGunFromId(4532));
            yield return DebuffAction<Vulnerable>(selector.SelectedEnemy, 0, Value1, occupationTime:1);
            yield return base.AttackAction(selector, base.GunName);
            yield break;
        }
    }
}


