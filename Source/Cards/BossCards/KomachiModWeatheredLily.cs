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
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModWeatheredLilyDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Illustrator = "";
            config.ImageId = nameof(KomachiModSpiderLily);
            //If IsPooled is false then the card cannot be discovered or added to the library at the end of combat.
            config.IsPooled = false;

            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 2 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Status;
            config.TargetType = TargetType.Nobody;

            // Poison amount
            config.Value1 = 3;

            config.Mana = ManaGroup.Reds(1);

            // Might remove ethereal if too weak
            config.Keywords = Keyword.Exile;
            //Setting Upgrading Keyword only provides the keyword when the card is upgraded.    
            config.UpgradedKeywords = Keyword.Exile;

            config.RelativeEffects = new List<string>()
            {
                nameof(Poison)
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(Poison)
            };

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModWeatheredLilyDef))]
    public sealed class KomachiModWeatheredLily : KomachiCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(battle.Player.TurnEnding, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnEnding));
        }

        // Token: 0x06000C68 RID: 3176 RVA: 0x000182EA File Offset: 0x000164EA
        private IEnumerable<BattleAction> OnPlayerTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (base.Zone == CardZone.Hand)
            {
                base.NotifyActivating();
                yield return BuffAction<Poison>(base.Value1);
            }
            yield break;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
            yield return new GainManaAction(Mana);
            yield break;
        }
    }
}


