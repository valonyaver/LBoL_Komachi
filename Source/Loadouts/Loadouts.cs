using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using KomachiMod.Cards;
using KomachiMod.Exhibits;
using KomachiMod.KomachiUlt;
namespace KomachiMod
{
    public class KomachiLoadouts
    {
        public static string UltimateSkillA = nameof(KomachiUltA);
        public static string UltimateSkillB = nameof(KomachiUltB);

        public static string ExhibitA = nameof(KomachiExhibitR);
        public static string ExhibitB = nameof(KomachiExhibitB);
        public static List<string> DeckA = new List<string>{
            nameof(Shoot),
            nameof(Shoot),
            nameof(Boundary),
            nameof(Boundary),
            nameof(KomachiAttackR),
            nameof(KomachiAttackR), 
            nameof(KomachiBlockB), 
            nameof(KomachiBlockB), 
            nameof(KomachiModShootAndMove)
        };

        public static List<string> DeckB = new List<string>{
            nameof(Shoot),
            nameof(Shoot),
            nameof(Boundary),
            nameof(Boundary),
            nameof(KomachiAttackB),
            nameof(KomachiAttackB), 
            nameof(KomachiBlockR), 
            nameof(KomachiBlockR), 
            nameof(KomachiAoeAttack),
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
            InitialMoney: 120,
            InitialPower: 0,
            BasicRingOrder: 0,
            LeftColor: ManaColor.Red,
            RightColor: ManaColor.Black,
            UltimateSkillA: KomachiLoadouts.UltimateSkillA,
            UltimateSkillB: KomachiLoadouts.UltimateSkillB,
            ExhibitA: KomachiLoadouts.ExhibitA,
            ExhibitB: KomachiLoadouts.ExhibitB,
            DeckA: KomachiLoadouts.DeckA,
            DeckB: KomachiLoadouts.DeckB,
            DifficultyA: 3,
            DifficultyB: 2
        );
    }
}
