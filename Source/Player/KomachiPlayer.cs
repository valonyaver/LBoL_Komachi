using Cysharp.Threading.Tasks;
//using DG.Tweening;
using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using KomachiMod.ImageLoader;
using KomachiMod.Localization;
//using KomachiMod.BattleActions;

namespace KomachiMod
{
    public sealed class KomachiModDef : PlayerUnitTemplate
    {        
        public UniTask<Sprite>? LoadSpellPortraitAsync { get; private set; }

        public override IdContainer GetId()
        {
            return BepinexPlugin.modUniqueID;
        }

        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.PlayerUnitBatchLoc.AddEntity(this);
        }

        public override PlayerImages LoadPlayerImages()
        {
            return KomachiImageLoader.LoadPlayerImages(BepinexPlugin.playerName);
        }

        public override PlayerUnitConfig MakeConfig()
        {
            return KomachiLoadouts.playerUnitConfig;
        }

        [EntityLogic(typeof(KomachiModDef))]
        public sealed class KomachiMod : PlayerUnit 
        {
        }
    }
}