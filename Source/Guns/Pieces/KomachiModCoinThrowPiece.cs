using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.GunName;
using KomachiMod.Source.GunName.GunTests;
using KomachiMod.Source.Guns;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Presentation.Bullet;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;

namespace KomachiMod.GunName
{
    public sealed class KomachiModCoinThrowPiece : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();
            config.Id = PieceTemplate.ConvertGunId(444001);
            config.Projectile = "DanmaCopper";
            config.GInterval = 2;
            config.Group = 10;
            config.Way = BulletMatrixHelper.Way(baseCount: 1);
            config.StartTime = 6;
            config.StartSpeed = BulletMatrixHelper.Matrix(6);
            config.StartAcc = BulletMatrixHelper.Matrix(1);
            config.Range = BulletMatrixHelper.Matrix(0);
            config.GAngle = BulletMatrixHelper.Matrix(-30, perGroup: 7);

            config.HitAmount = 1;
            config.ZeroHitNotDie = true;

            BulletEventBuilder events = new BulletEventBuilder();

            BulletEventBuilder.ApplyEvents(config, events);

            config.Color = BulletColorHelper.CycleByGroup(BulletColor.Grayscale, BulletColor.Orange, BulletColor.RedOrange);
            return config;
        }
    }

    //[EntityLogic(typeof(KomachiModTestPiece1Def))]
    //public sealed class KomachiModTestPiece1 : Piece
    //{

    //}
}


