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
    public sealed class KomachiModTestPiece2Def : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();
            config.Id = PieceTemplate.ConvertGunId(80002);
            config.ShootType = 0;
            config.GInterval = 10;
            config.Group = 5;
            config.Way = new int[][] { new int[] { 3 }, new int[] { 0 } };
            config.StartTime = 0;
            config.StartSpeed = new float[][] { new float[] { 1 } };
            // Events
            config.EvType = new int[][]
            { new int[] { 2 }, new int[] { 1 }, new int[] { 2 } };
            config.EvStart = new int[][][] 
            { new int[][] { new int[] { 0 } }, new int[][] { new int[] { 90 } }, new int[][] { new int[] { 90 } } };
            config.EvDuration = new int[][][] 
            { new int[][] { new int[] { 30 } }, new int[][] { new int[] { 30 } }, new int[][] { new int[] { 30 } }  };
            config.EvNumber = new float[][][] 
            { new float[][] { new float[] { 30 } }, new float[][] { new float[] { 0 }, new float[] { 1 } }, new float[][] { new float[] { -60 } } };
            

            config.StartAcc = new float[][] { new float[] { 1 } };
            // config.StartAccAngle = new float[][] { new float[] { 30 }, new float[] { 0 }, new float[] { }, new float[] { -10f } };
            //config.X = new float[][] { new float[] { 0 }, new float[] { 1f } };
            //config.Y = new float[][] { new float[] { 0 }, new float[] { 1 } };

            //config.Radius = new float[][] { new float[] { 0 }, new float[] { 0.5f } };
            //config.RadiusA = new float[][] { new float[] { 1 }
            //, new float[] { -5f }, new float[] { }, new float[] { 5f } 
            //};
            config.ShootEnd = 60;
            config.Life = new int[][] { new int[] { 600 } };

            config.RootType = 0;
            config.Aim = 2;

            config.Range = new float[][] { new float[] { 45 }, new float[] { 0 }, new float[] { 0 } };

            config.Color = new int[][] { new int[] { 1 }, new int[] { 1, 2, 3, 4, 5 } };
            return config;
        }
    }

    #region FOLLOW UP PIECE
    //public sealed class KomachiModTestPiece2FollowDef : KomachiPieceTemplate
    //{
    //    public override PieceConfig MakeConfig()
    //    {
    //        PieceConfig config = GetDefaultGunConfig();

    //        config.Id = PieceTemplate.ConvertGunId(80002, 1);
    //        config.ShootType = 3;
    //        config.GInterval = 15;
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
            

    //        config.Projectile = "DanmaRice";


    //        // config.FollowPiece = PieceTemplate.ConvertGunId(80001, 0);

    //        config.HitAmount = 10;
    //        config.ZeroHitNotDie = true;

    //        config.AddParentAngle = true;
    //        config.Aim = 3;
    //        config.RootType = 0;
    //        config.Range = new float[][] { new float[] { 180 }, new float[] { 0 }, new float[] { 0 } };


    //        config.Color = new int[][] { new int[] { 2 }, new int[] { 2, 3, 4, 5, 6 } };
    //        return config;
    //    }
    //}
    #endregion
}


