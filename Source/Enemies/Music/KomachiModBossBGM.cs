using Cysharp.Threading.Tasks;
using KomachiMod.Localization;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KomachiMod.Source.Enemies.Music
{
    public sealed class KomachiModBossBgm : BgmTemplate
    {
        public override IdContainer GetId() => nameof(KomachiModBossBgm);

        public override UniTask<AudioClip> LoadAudioClipAsync()
        {
            return ResourceLoader.LoadAudioClip(
                "HiganRetour.ogg",
                AudioType.OGGVORBIS,
                BepinexPlugin.directorySource,
                "file://"
            );
        }

        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.BgmBatchLoc.AddEntity(this);
        }

        public override BgmConfig MakeConfig()
        {
            var config = new BgmConfig(
                ID: GetId(),
                No: BepinexPlugin.sequenceTable.Next(typeof(BgmConfig)),
                Folder: "",
                Path: "",
                Volume: 1f,
                LoopStart: 1.4f,
                LoopEnd: null,
                ExtraDelay: null,
                Artist: "Rowster ツ"
            );
            return config;
        }
    }
}
