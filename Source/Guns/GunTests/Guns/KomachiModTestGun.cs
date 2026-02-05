using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using LBoL.Core.Battle;
using LBoL.Core;
using KomachiMod.Source.GunName.GunTests;
using LBoL.Presentation.Bullet;
using UnityEngine;

namespace KomachiMod.GunName
{
    // The name of the gun in debug mode seems to be based on the class name itself rather than the config name.
    public sealed class KomachiModTestGunShootWDef : KomachiGunTemplate
    {
        public override GunConfig MakeConfig()
        {
            Debug.Log("Logging the config for gunpiece1");
            GunConfig config = DefaultGunConfig();
            // The Only thing that matters for the guns themselves is the id. Everything else is in the pieces.
            config.Id = 80001;
            config.Name = "KomachiGunTestShootW";
            return config;
        }
    }
}


