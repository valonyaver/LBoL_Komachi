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

namespace KomachiMod.GunName
{
    public sealed class KomachiModTestGunShootW4Def : KomachiGunTemplate
    {
        public override GunConfig MakeConfig()
        {
            GunConfig config = DefaultGunConfig();
            config.Id = 80004;
            config.Name = "KomachiGunTestShootW4";
            return config;
        }
    }
}


