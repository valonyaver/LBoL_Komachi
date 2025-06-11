using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using KomachiMod.ImageLoader;
using KomachiMod.Localization;
using KomachiMod.Config;

namespace KomachiMod.StatusEffects
{
    public class KomachiStatusEffectTemplate : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return KomachiDefaultConfig.DefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.StatusEffectsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return KomachiImageLoader.LoadStatusEffectLoader(status: this);
        }

        public override StatusEffectConfig MakeConfig()
        {
            return GetDefaultStatusEffectConfig();
        }

        public static StatusEffectConfig GetDefaultStatusEffectConfig()
        {
            return KomachiDefaultConfig.DefaultStatusEffectConfig();
        }        
    }
}