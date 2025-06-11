using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;

namespace KomachiMod.Localization
{
    public sealed class KomachiLocalization
    {
        public static string Cards = "Cards";
        public static string Exhibits = "Exhibits";
        public static string PlayerUnit = "PlayerUnit";
        public static string EnemiesUnit = "EnemyUnit";
        public static string UnitModel = "UnitModel";
        public static string UltimateSkills = "UltimateSkills";
        public static string StatusEffects = "StatusEffects";

        public static BatchLocalization CardsBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(CardTemplate), Cards);
        public static BatchLocalization ExhibitsBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(ExhibitTemplate), Exhibits);
        public static BatchLocalization PlayerUnitBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(PlayerUnitTemplate), PlayerUnit);
        public static BatchLocalization EnemiesUnitBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(EnemyUnitTemplate), EnemiesUnit);
        public static BatchLocalization UnitModelBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(UnitModelTemplate), UnitModel);
        public static BatchLocalization UltimateSkillsBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(UltimateSkillTemplate), UltimateSkills);
        public static BatchLocalization StatusEffectsBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(StatusEffectTemplate), StatusEffects);


        // maybe it's better to have controlled file discovery tah
        public static void Init()
        {
            CardsBatchLoc.DiscoverAndLoadLocFiles(Cards);
            ExhibitsBatchLoc.DiscoverAndLoadLocFiles(Exhibits);
            PlayerUnitBatchLoc.DiscoverAndLoadLocFiles(PlayerUnit);
            EnemiesUnitBatchLoc.DiscoverAndLoadLocFiles(EnemiesUnit);
            UnitModelBatchLoc.DiscoverAndLoadLocFiles(UnitModel);
            UltimateSkillsBatchLoc.DiscoverAndLoadLocFiles(UltimateSkills);
            StatusEffectsBatchLoc.DiscoverAndLoadLocFiles(StatusEffects);
        }
    }
}