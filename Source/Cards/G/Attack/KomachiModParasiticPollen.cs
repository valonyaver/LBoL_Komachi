using KomachiMod.BattleActions;
using KomachiMod.Cards;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KomachiMod.Source.Cards
{
    public sealed class KomachiModParasiticPollenDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(7190);
            config.GunNameBurst = GunNameID.GetGunFromId(7191);

            // config.ImageId = nameof(KomachiModAttackB);

            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            config.Cost = new ManaGroup() { Red = 1, Green = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 1, Any = 1, HybridColor = 9 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;
        
            config.Damage = 6;
            config.UpgradedDamage = 6;

            // Amount of Attacks
            config.Value1 = 2;
            config.UpgradedValue1 = 3;

            // Amount of Poison applied and lost
            config.Value2 = 3;
            config.UpgradedValue2 = 4;

            config.RelativeEffects = new List<string>() { nameof(Poison)};
            config.UpgradedRelativeEffects = new List<string>() { nameof(Poison)};

            config.RelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };
            config.UpgradedRelativeCards = new List<string>() { nameof(KomachiModSpiderLily) };

            // config.UpgradedKeywords = Keyword.Accuracy;


            config.Illustrator = "Iced_Lemon";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModParasiticPollenDef))]
    public sealed class KomachiModParasiticPollen : KomachiCard
    {


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // Deal damage times
            for (int i = 0; i < Value1; i++)
            {
                yield return AttackAction(selector, GunName);
            }
            // If enemy has poison, get lily. Otherwise, apply poison.
            yield return new AddCardsToDrawZoneAction(Library.CreateCards<KomachiModSpiderLily>(1, false), DrawZoneTarget.Random);

            if (Battle.Player.TryGetStatusEffect<Poison>(out var playerPoison))
            {
                int poisonLevel = playerPoison.Level;
                poisonLevel = Math.Min(3, poisonLevel);
                playerPoison._level -= 3;
                playerPoison.NotifyChanged();
                if (playerPoison._level <= 0)
                {
                    yield return new RemoveStatusEffectAction(playerPoison);
                }

                yield return new ApplyStatusEffectAction<Poison>
                    (selector.SelectedEnemy, poisonLevel * 2);
            }
            yield break;
        }
    }
}


