using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using KomachiMod.Cards;
using KomachiMod.Exhibits;
using KomachiMod.KomachiUlt;
namespace KomachiMod
{
    public class KomachiModLoadouts
    {
        public static string UltimateSkillA = nameof(KomachiModUltA);
        public static string UltimateSkillB = nameof(KomachiModUltB);

        public static string ExhibitA = nameof(KomachiModExhibitR);
        public static string ExhibitB = nameof(KomachiModExhibitB);
        public static List<string> DeckA = new List<string>{
            nameof(Shoot),
            nameof(Shoot),
            nameof(Boundary),
            nameof(Boundary),
            nameof(KomachiModAttackR),
            nameof(KomachiModAttackR), 
            nameof(KomachiModBlockB), 
            nameof(KomachiModBlockB),
            nameof(KomachiModMoveAndShoot),
            nameof(KomachiModRetreat)
        };

        public static List<string> DeckB = new List<string>{
            nameof(Shoot),
            nameof(Shoot),
            nameof(Boundary),
            nameof(Boundary),
            nameof(KomachiModAttackB),
            nameof(KomachiModAttackB), 
            nameof(KomachiModBlockR), 
            nameof(KomachiModBlockR),
            nameof(KomachiModGrudgingStrike),
            nameof(KomachiModSpiritDefence)
        };

        public static PlayerUnitConfig playerUnitConfig = new PlayerUnitConfig(
            Id: BepinexPlugin.modUniqueID,
            HasHomeName: true,
            ShowOrder: 8, 
            Order: 0,
            UnlockLevel: 0,
            ModleName: "",
            NarrativeColor: "#e58c27",
            IsSelectable: true,
            MaxHp: 70,
            InitialMana: new ManaGroup() { Black = 2, Red = 2 },
            InitialMoney: 104,
            InitialPower: 0,
            BasicRingOrder: null,
            LeftColor: ManaColor.Red,
            RightColor: ManaColor.Black,
            UltimateSkillA: KomachiModLoadouts.UltimateSkillA,
            UltimateSkillB: KomachiModLoadouts.UltimateSkillB,
            ExhibitA: KomachiModLoadouts.ExhibitA,
            ExhibitB: KomachiModLoadouts.ExhibitB,
            DeckA: KomachiModLoadouts.DeckA,
            DeckB: KomachiModLoadouts.DeckB,
            DifficultyA: 2,
            DifficultyB: 3
        );
    }
}
