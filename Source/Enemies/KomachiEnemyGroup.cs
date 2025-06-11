using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using KomachiMod.Enemies.Template;


namespace KomachiMod.Enemies
{
    public sealed class KomachiEnemyGroupDef : KomachiEnemyGroupTemplate
    {
        public override IdContainer GetId() => nameof(KomachiMod);

        public override EnemyGroupConfig MakeConfig()
        {
            EnemyGroupConfig config = GetEnemyGroupDefaultConfig();
            config.Name = nameof(KomachiMod);
            config.FormationName = VanillaFormations.Single;
            config.Enemies = new List<string>() { nameof(KomachiMod) };
            config.EnemyType = EnemyType.Boss;
            config.RollBossExhibit = true;

            return config;
        }
    }
}