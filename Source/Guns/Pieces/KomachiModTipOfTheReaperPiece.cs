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
    // Single bullet that is parent to the slash. Hidden within barrage.
    public sealed class TipReaperParentDef : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();

            config.Id = PieceTemplate.ConvertGunId(444002, 0);
            config.Projectile = "DanmaCopper";
            config.ShootType = 0;
            config.Group = 1;
            config.StartTime = 6;
            config.Way = BulletMatrixHelper.Way(1);
            config.Range = BulletMatrixHelper.Matrix(0);
            config.StartSpeed = BulletMatrixHelper.Constant(6);
            config.StartAcc = BulletMatrixHelper.Constant(5);
            config.Scale = BulletMatrixHelper.Constant(0.5f);
            config.LastWave = false;
            config.Color = BulletColorHelper.Constant(BulletColor.Orange);
            return config;
        }
    }
    #region FOLLOW UP PIECE
    // Slash that spawns when parent coin is dead
    public sealed class TipReaperFollowDef : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();

            config.Id = PieceTemplate.ConvertGunId(444002, 1);
            config.Projectile = "MeleeKan";
            config.ParentPiece = 0;
            config.ShootType = 3;
            config.RootType = 0;
            config.GInterval = 10;
            config.Group = 2;
            config.Way = BulletMatrixHelper.Way(1);
            config.Scale = BulletMatrixHelper.Constant(0.5f);
            config.StartTime = 0;
            config.ZeroHitNotDie = true;
            config.Color = BulletColorHelper.Constant(BulletColor.Red);
            config.LastWave = true;
            config.GAngle = BulletMatrixHelper.Matrix(0, 30, perGroupRandom: 30);
            return config;
        }
    }
    #endregion

    // Barrage of bullets aimed at target
    public sealed class TipReaperMultiDef : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();

            config.Id = PieceTemplate.ConvertGunId(444002, 2);
            config.Projectile = "DanmaCopper";
            config.ShootType = 0;
            config.Group = 9;
            config.GInterval = 2;
            config.Way = BulletMatrixHelper.Way(1);
            config.Range = BulletMatrixHelper.Matrix(0, perGroup: 0);
            config.GAngle = BulletMatrixHelper.Matrix(0, perGroup: 0);
            config.StartSpeed = BulletMatrixHelper.Constant(6);
            config.StartAcc = BulletMatrixHelper.Constant(5);
            config.Scale = BulletMatrixHelper.Constant(0.5f);
            config.LastWave = false;
            config.Color = BulletColorHelper.Constant(BulletColor.Orange);
            return config;
        }
    }
}


