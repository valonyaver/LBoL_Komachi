using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace KomachiMod.Cards
{
    /// <summary>
    /// Unused card. Used for early testing of the distance mechanic.
    /// </summary>
    public sealed class KomachiModSliceOfHiganDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(6160);
            config.IsPooled = true;
            config.ImageId = "KomachiAttackR";

            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1, Any = 3 };
            config.Rarity = Rarity.Rare;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 15;
            config.UpgradedDamage = 18;


            // Amount of attacks
            config.Value1 = 2;

            // Damage increase per distance
            config.Value2 = 1;
            config.UpgradedValue2 = 2;

            config.Keywords = Keyword.Accuracy | Keyword.Exile | Keyword.Retain;
            config.UpgradedKeywords = Keyword.Accuracy | Keyword.Exile | Keyword.Retain;
            config.RelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDisplacementKeyword), nameof(KomachiDistanceKeyword) };

            config.Illustrator = "@TheIllustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            config.Unfinished = true;
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModSliceOfHiganDef))]
    public sealed class KomachiModSliceOfHigan : KomachiCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<DistanceChangedEventArgs>(KomachiEventsManager.DistanceChanged, new GameEventHandler<DistanceChangedEventArgs>(this.OnDistanceChange), (GameEventPriority)0);
        }

        private void OnDistanceChange(DistanceChangedEventArgs args)
        {
            if (base.Zone == CardZone.Hand)
            {
                base.DeltaDamage += base.Value2 * args.distanceChangeAbs;
                this.NotifyChanged();
            }
        }

        protected override void SetGuns()
        {
            base.CardGuns = new Guns(base.GunName, base.Value1, true);
        }
    }
}


