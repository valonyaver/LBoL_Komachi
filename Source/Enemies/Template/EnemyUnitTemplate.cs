using System;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using KomachiMod.Config;
using KomachiMod.Localization;


namespace KomachiMod.Enemies.Template
{
    public class KomachiEnemyUnitTemplate : EnemyUnitTemplate
    {
        public override IdContainer GetId()
        {
            return KomachiDefaultConfig.DefaultID(this);
        }

        public override EnemyUnitConfig MakeConfig()
        {
            return KomachiDefaultConfig.EnemyUnitDefaultConfig();
        }

        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.EnemiesUnitBatchLoc.AddEntity(this);
        }

        public override Type TemplateType()
        {
            return typeof(EnemyUnitTemplate);
        }

        public EnemyUnitConfig GetEnemyUnitDefaultConfig()
        {
            return KomachiDefaultConfig.EnemyUnitDefaultConfig();
        }


    }
}