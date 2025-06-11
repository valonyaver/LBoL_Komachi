using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using KomachiMod.Config;


namespace KomachiMod.Enemies.Template
{
    public abstract class KomachiEnemyGroupTemplate : EnemyGroupTemplate
    {
        public override IdContainer GetId()
        {
            return KomachiDefaultConfig.DefaultID(this);
        }

        public override EnemyGroupConfig MakeConfig()
        {
            return KomachiDefaultConfig.EnemyGroupDefaultConfig();
        }

        public EnemyGroupConfig GetEnemyGroupDefaultConfig()
        {
            return KomachiDefaultConfig.EnemyGroupDefaultConfig();
        }
    }
}