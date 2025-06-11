using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using KomachiMod.Config;
using KomachiMod.ImageLoader;
using KomachiMod.Localization;


namespace KomachiMod.Cards.Template
{
    public abstract class KomachiCardTemplate : CardTemplate
    {
        public override IdContainer GetId()
        {
            return KomachiDefaultConfig.DefaultID(this);
        }

        public override CardImages LoadCardImages()
        {
            return KomachiImageLoader.LoadCardImages(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.CardsBatchLoc.AddEntity(this);
        }

        public CardConfig GetCardDefaultConfig()
        {
            return KomachiDefaultConfig.CardDefaultConfig();
        }
    }


}


