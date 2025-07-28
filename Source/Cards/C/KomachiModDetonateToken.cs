using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.Cards
{
    public sealed class KomachiModDetonateTokenDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);
            //If IsPooled is false then the card cannot be discovered or added to the library at the end of combat.
            config.IsPooled = false;

            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;

            config.Keywords = Keyword.Exile | Keyword.Retain;
            //Setting Upgrading Keyword only provides the keyword when the card is upgraded.    
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain | Keyword.Echo;


            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.RelativeEffects = new List<string>() { nameof(KomachiDetonationKeyword), nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDetonationKeyword), nameof(KomachiModVengefulSpiritSe) };
            config.Illustrator = "Valonadthe";
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModDetonateTokenDef))]
    public sealed class KomachiModDetonateToken : KomachiCard
    {
        public bool chooseDontDetonate;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new DetonateVengefulSpiritAction(this, selector.SelectedEnemy);
        }
    }
}


