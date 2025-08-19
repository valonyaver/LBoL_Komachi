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
    public sealed class KomachiModSanzuFareDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Illustrator = "";
            //If IsPooled is false then the card cannot be discovered or added to the library at the end of combat.
            config.IsPooled = false;

            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Common;

            config.Type = CardType.Status;
            config.TargetType = TargetType.Nobody;

            config.Keywords = Keyword.Exile | Keyword.Ethereal;
            //Setting Upgrading Keyword only provides the keyword when the card is upgraded.    
            config.UpgradedKeywords = Keyword.Exile | Keyword.Ethereal;

            config.MoneyCost = 25;

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSanzuFareDef))]
    public sealed class KomachiModSanzuFare : KomachiCard
    {
        public override bool CanUse => Battle.GameRun.Money >= MoneyCost;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
            List<EnemyUnit> list = base.Battle.EnemyGroup.Where((EnemyUnit u) => u is Enemies.KomachiMod && u.IsAlive).ToList();
            if (list.Count > 1)
            {
                Debug.LogWarning("Multiple Komachi exists");
                yield break;
            }

            if (list.Count == 0)
            {
                Debug.LogWarning("Bribery is used while no Komachi");
                yield break;
            }

            EnemyUnit komachiUnit = list.First();
            if (base.Battle.BattleCardUsageHistory.Count((Card card) => card is KomachiModSanzuFare) < 1)
            {
                yield return PerformAction.Chat(komachiUnit, CardDialogue1, 3f);
            }
            else
            {
                yield return PerformAction.Chat(komachiUnit, CardDialogue2, 3f);
            }
            yield return DebuffAction<LongEscape>(komachiUnit, MoneyCost);
            yield return DebuffAction<FirepowerNegative>(komachiUnit, 1);

            yield break;
            Debug.Log(nameof(Bribery));
        }
    }
}


