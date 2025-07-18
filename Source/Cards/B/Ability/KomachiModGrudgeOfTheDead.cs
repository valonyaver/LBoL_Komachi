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
    public sealed class KomachiModDetonateFirepowerDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2, Any = 2 };
            config.UpgradedCost = new ManaGroup() { Black = 2, Any = 1 };

            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Ability;
            config.TargetType = TargetType.Nobody;

            // Spirits inflicted
            config.Value1 = 2;
            config.UpgradedValue1 = 4;

            // Firepower gain
            config.Value2 = 1;

            config.RelativeEffects = new List<string>()
            { 
                nameof(Firepower),
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiDetonationKeyword)
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(Firepower),
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiDetonationKeyword)
            };


            config.Illustrator = "Chocotti";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModDetonateFirepowerDef))]
    public sealed class KomachiModDetonateFirepower : KomachiCard
    {
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseDetonate(this, 1);
        }
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<KomachiModDetonateFirepowerSe>(Value2);
            foreach (var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(this, enemy, Value1);
            }
            Card releaseChoice = KomachiModUtility.GetPreconditionCard(precondition);
            if (releaseChoice != null && !(releaseChoice.GetType() == typeof(KomachiModDetonateToken))) // ironicaally ye the detonate token is dont detonte
            {
                foreach (var enemy in Battle.AllAliveEnemies)
                {
                    yield return new DetonateVengefulSpiritAction(this, enemy);
                }
            }
            yield break;
        } 
    }
}


