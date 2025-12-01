using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.GunName;
using KomachiMod.Source.GunName.GunTests;
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
    public sealed class KomachiModTestPiece1Def : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();

            config.Id = PieceTemplate.ConvertGunId(80001);
            config.ShootType = 0;
            config.GInterval = 10;
            config.Group = 5;
            config.Way = new int[][] { new int[] { 3 }, new int[] { 0 } };
            config.StartTime = 0;
            config.StartSpeed = new float[][] { new float[] { 3 }, new float[] { 0 } };
            config.EvStart = new int[][][] { new int[][] { new int[] { 60 } } };
            config.EvDuration = new int[][][] { new int[][] { new int[] { 60 } } };
            config.EvNumber = new float[][][] { new float[][] { new float[] { 45 } } };
            config.EvType = new int[][] { new int[] { 2 } };
            config.X = new float[][] { new float[] { 0 } };
            // config.Y = new float[][] { new float[] { -2 }, new float[] { 0.2f }, new float[] { 0f } };
            //config.Radius = new float[][] { new float[] { 0 }, new float[] { 0.5f } };
            // config.RadiusA = new float[][] { new float[] { 10 }, new float[] { 5f }, new float[] { }, new float[] { 5f } };
            // config.Life = new int[][] { new int[] { 100 } };
            config.Aim = 0;
            config.Range = new float[][] { new float[] { 45 }, new float[] { 0 }, new float[] { 0 }};
            config.LastWave = false;

            config.HitAmount = 1;

            config.Color = new int[][] { new int[] { 1 }, new int[] { 1 } };
            return config;
        }
    }
    #region FOLLOW UP PIECE
    public sealed class KomachiModTestPiece1FollowDef : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();

            config.Id = PieceTemplate.ConvertGunId(80001, 1);
            config.Projectile = "DanmaRice";
            config.ShootType = 2;
            config.GInterval = 10;
            config.Group = 1;
            config.Way = new int[][] { new int[] { 3 }, new int[] { 0 } };
            config.StartTime = 51;
            config.StartSpeed = new float[][] { new float[] { 3 } };
            config.EvStart = new int[][][] { new int[][] { new int[] { 60 } } };
            config.EvDuration = new int[][][] { new int[][] { new int[] { 60 } } };
            config.EvNumber = new float[][][] { new float[][] { new float[] { -45 } } };
            config.EvType = new int[][] { new int[] { 2 } };
            //config.X = new float[][] { new float[] { 0 }, new float[] { 0.5f }, };
            //config.Y = new float[][] { new float[] { -2 }, new float[] { 0.2f }, new float[] { 0f } };
            // config.Radius = new float[][] { new float[] { 1 }, new float[] { 0 } };
            // config.RadiusA = new float[][] { new float[] { 10 }, new float[] { 5f }, new float[] { }, new float[] { 5f } };
            // config.Life = new int[][] { new int[] { 600 } };

            // config.FollowPiece = PieceTemplate.ConvertGunId(80001, 0);
            config.HitAmount = 10;
            config.ZeroHitNotDie = true;

            config.AddParentAngle = false;
            //config.Aim = 3;
            //config.RootType = 0;
            config.Range = new float[][] { new float[] { 180 }, new float[] { 0 }, new float[] { 0 } };
            config.Color = new int[][] { new int[] { 2 } };
            return config;
        }
    }
    #endregion

    #region FOLLOW UP PIECE2
    //public sealed class KomachiModTestPiece1Follow2Def : KomachiPieceTemplate
    //{
    //    public override PieceConfig MakeConfig()
    //    {
    //        PieceConfig config = GetDefaultGunConfig();

    //        config.Id = PieceTemplate.ConvertGunId(80001, 2);
    //        config.Projectile = "DanmaRice";
    //        config.ShootType = 2;
    //        config.GInterval = 10;
    //        config.Group = 1;
    //        config.Way = new int[][] { new int[] { 5 }, new int[] { 0 } };
    //        config.StartTime = 51;
    //        config.StartSpeed = new float[][] { new float[] { 3 } };
    //        config.EvStart = new int[][][] { new int[][] { new int[] { 60 } } };
    //        config.EvDuration = new int[][][] { new int[][] { new int[] { 60 } } };
    //        config.EvNumber = new float[][][] { new float[][] { new float[] { 0 } } };
    //        config.EvType = new int[][] { new int[] { 1 } };
    //        //config.X = new float[][] { new float[] { 0 }, new float[] { 0.5f }, };
    //        //config.Y = new float[][] { new float[] { -2 }, new float[] { 0.2f }, new float[] { 0f } };
    //        // config.Radius = new float[][] { new float[] { 1 }, new float[] { 0 } };
    //        // config.RadiusA = new float[][] { new float[] { 10 }, new float[] { 5f }, new float[] { }, new float[] { 5f } };
    //        // config.Life = new int[][] { new int[] { 600 } };

    //        // config.FollowPiece = PieceTemplate.ConvertGunId(80001, 0);
    //        config.HitAmount = 10;
    //        config.ZeroHitNotDie = true;

    //        config.AddParentAngle = true;
    //        config.Aim = 3;
    //        config.RootType = 0;
    //        config.Range = new float[][] { new float[] { 180 }, new float[] { 0 }, new float[] { 0 } };
    //        config.Color = new int[][] { new int[] { 3 } };
    //        return config;
    //    }
    //}
    #endregion

    #region FOLLOW UP PIECE3
    //public sealed class KomachiModTestPiece3FollowDef : KomachiPieceTemplate
    //{
    //    public override PieceConfig MakeConfig()
    //    {
    //        PieceConfig config = GetDefaultGunConfig();

    //        config.Id = PieceTemplate.ConvertGunId(80001, 3);
    //        config.Projectile = "DanmaRice";
    //        config.ShootType = 2;
    //        config.GInterval = 10;
    //        config.Group = 1;
    //        config.Way = new int[][] { new int[] { 5 }, new int[] { 0 } };
    //        config.StartTime = 51;
    //        config.StartSpeed = new float[][] { new float[] { 3 } };
    //        config.EvStart = new int[][][] { new int[][] { new int[] { 60 } } };
    //        config.EvDuration = new int[][][] { new int[][] { new int[] { 60 } } };
    //        config.EvNumber = new float[][][] { new float[][] { new float[] { 0 } } };
    //        config.EvType = new int[][] { new int[] { 1 } };
    //        //config.X = new float[][] { new float[] { 0 }, new float[] { 0.5f }, };
    //        //config.Y = new float[][] { new float[] { -2 }, new float[] { 0.2f }, new float[] { 0f } };
    //        // config.Radius = new float[][] { new float[] { 1 }, new float[] { 0 } };
    //        // config.RadiusA = new float[][] { new float[] { 10 }, new float[] { 5f }, new float[] { }, new float[] { 5f } };
    //        // config.Life = new int[][] { new int[] { 600 } };

    //        // config.FollowPiece = PieceTemplate.ConvertGunId(80001, 0);
    //        config.HitAmount = 10;
    //        config.ZeroHitNotDie = true;

    //        config.AddParentAngle = false;
    //        config.Aim = 4;
    //        config.RootType = 0;
    //        config.Range = new float[][] { new float[] { 180 }, new float[] { 0 }, new float[] { 0 } };
    //        config.Color = new int[][] { new int[] { 1 } };
    //        return config;
    //    }
    //}
    #endregion
}


