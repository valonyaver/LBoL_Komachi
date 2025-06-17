using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using KomachiMod.ImageLoader;
using KomachiMod.Localization;
using KomachiMod.Config;

namespace KomachiMod.KomachiUlt
{
    public class KomachiUltTemplate : UltimateSkillTemplate
    {
        public override IdContainer GetId()
        {
            return KomachiDefaultConfig.DefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.UltimateSkillsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return KomachiImageLoader.LoadUltLoader(ult: this);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            throw new System.NotImplementedException();
        }

        public UltimateSkillConfig GetDefaulUltConfig()
        {
            return KomachiDefaultConfig.DefaultUltConfig();
        }
    }
}