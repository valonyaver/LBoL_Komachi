using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using KomachiMod.Config;
using KomachiMod.ImageLoader;
using KomachiMod.Localization;

namespace KomachiMod.Exhibits
{
    public class KomachiExhibitTemplate : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return KomachiDefaultConfig.DefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.ExhibitsBatchLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            return KomachiImageLoader.LoadExhibitSprite(exhibit: this);
        }

        public override ExhibitConfig MakeConfig()
        {
            return GetDefaultExhibitConfig();
        }

        public ExhibitConfig GetDefaultExhibitConfig()
        {
            return KomachiDefaultConfig.DefaultExhibitConfig();
        }

    }
}