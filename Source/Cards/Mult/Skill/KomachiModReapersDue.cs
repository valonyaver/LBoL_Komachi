using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using KomachiMod.BattleActions;
using KomachiMod.Source.StatusEffects.Spirits;

namespace KomachiMod.Cards
{
    public sealed class KomachiModReapersDueDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();

            config.GunName = GunNameID.GetGunFromId(400);

            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            //Mana to consume aside from X
            config.Cost = new ManaGroup() { Black = 1, White = 1, Any = 0 };
            config.Rarity = Rarity.Uncommon;

            //The XCost has to be set.
            config.IsXCost = true;

            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;

            // Amount of Guided Spirits per mana
            config.Value1 = 3;
            config.UpgradedValue1 = 4;

            // Amount of vengeful or divine spirits per mana
            config.Value2 = 4;
            config.UpgradedValue2 = 5;

            config.Mana = new ManaGroup() { Any = 1 };

            config.Illustrator = "Kanta";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModReapersDueDef))]
    public sealed class KomachiModReapersDue : KomachiCard
    {
        //Create another ManaGroup that can be used to display the W mana on the card.
        public ManaGroup WhiteMana = new ManaGroup() { White = 1 };
        public ManaGroup BlackMana = new ManaGroup() { Black = 1 };

        public int anyManaSpirits
        {
            get
            {
                if (Battle == null || PendingManaUsage == null) return 0;
                return SynergyAmount(PendingManaUsage.Value, ManaColor.Any, 1) * Value1;
            }
        }
        public int whiteManaSpirits
        {
            get
            {
                if (Battle == null || PendingManaUsage == null) return 0;
                return SynergyAmount(PendingManaUsage.Value, ManaColor.White, 1) * Value2;
            }
        }
        public int blackManaSpirits
        {
            get
            {
                if (Battle == null || PendingManaUsage == null) return 0;
                return SynergyAmount(PendingManaUsage.Value, ManaColor.Black, 1) * Value2;
            }
        }


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
            //Blue mana: Gain life.
            int anyMana = base.SynergyAmount(consumingMana, ManaColor.Any, 1);
            int whiteMana = base.SynergyAmount(consumingMana, ManaColor.White, 1);
            int blackMana = base.SynergyAmount(consumingMana, ManaColor.Black, 1);

            yield return BuffAction<KomachiModGuidedSpiritSe>(anyMana * Value1);
            yield return BuffAction<KomachiModDivineSpiritSe>(whiteMana * Value2);
            foreach (var enemy in Battle.AllAliveEnemies)
            {
                yield return new ApplyVengefulSpiritAction(this, enemy, blackMana *Value2);
            }
            yield break;
		}
    }
}


