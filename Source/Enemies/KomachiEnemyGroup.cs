using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using KomachiMod.Enemies.Template;


namespace KomachiMod.Enemies
{
    public sealed class KomachiEnemyGroupDef : KomachiEnemyGroupTemplate
    {
        public override IdContainer GetId() => nameof(KomachiModBoss);

        public override EnemyGroupConfig MakeConfig()
        {
            EnemyGroupConfig config = GetEnemyGroupDefaultConfig();
            config.Name = nameof(KomachiModBoss);
            config.FormationName = VanillaFormations.Single;
            config.Enemies = new List<string>() { nameof(KomachiModBoss) };
            config.EnemyType = EnemyType.Boss;
            config.RollBossExhibit = true;

            return config;
        }
    }
}