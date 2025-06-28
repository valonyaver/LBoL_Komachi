using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.EventManager;
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
    public sealed class KomachiModResidualAetherDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.SingleEnemy;


            config.Mana = new ManaGroup() { Black = 1 }; 
            // Mana increaase
            config.Value1 = 1;

            config.RelativeEffects = new List<string>() { nameof(KomachiDetonationKeyword) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiDetonationKeyword) };

            config.Keywords = Keyword.Exile | Keyword.Retain;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;


            config.Illustrator = "Wholesome_illustrator";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModResidualAetherDef))]
    public sealed class KomachiModResidualAether : KomachiCard
    {
        public ManaGroup baseMana = new ManaGroup() { Black = 1 };
        public ManaGroup upgradedBaseMana = new ManaGroup() { Philosophy = 1 };

        public ManaGroup ManaReal
        {
            get
            {
                if (IsUpgraded) return upgradedBaseMana;
                else return baseMana;
            }
        }
        public ManaGroup deltaMana = new ManaGroup();
        public ManaGroup totalMana
        {
            get => ManaReal + deltaMana;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            deltaMana.Philosophy += deltaMana.Black;
            deltaMana.Black = 0;
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<DetonateVengefulSpiritEventArgs>
                (KomachiEventsManager.DetonatedSpirits, new GameEventHandler<DetonateVengefulSpiritEventArgs>(this.OnDetonation));
        }

        private void OnDetonation(DetonateVengefulSpiritEventArgs args)
        {
            if (base.Zone == CardZone.Hand && args.noFizzle)
            {
                if (IsUpgraded)
                {
                    deltaMana.Philosophy += Value1;
                }
                else
                {
                    deltaMana.Black += Value1;
                }
                this.NotifyChanged();
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainManaAction(ManaReal);
        } 
    }
}


