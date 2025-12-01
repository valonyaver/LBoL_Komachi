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
    public sealed class KomachiModTestPiece3Def : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();
            config.Id = PieceTemplate.ConvertGunId(80003);
            config.ShootType = 0;
            config.GInterval = 30;
            config.Group = 10;
            config.Way = new int[][] { new int[] { 3 }, new int[] { 0 } };
            config.StartTime = 6;
            config.StartSpeed = new float[][] { new float[] { 2 } };
            config.EvStart = new int[][][] { new int[][] { new int[] { 60 } } };
            config.EvDuration = new int[][][] { new int[][] { new int[] { 999 } } };
            config.EvNumber = new float[][][] { new float[][] { new float[] { 0.01f } } };
            config.EvType = new int[][] { new int[] { 99 } };

            // config.StartAcc = new float[][] { new float[] { 1 } };
            //config.StartAccAngle = new float[][] { new float[] { 60f } };

            //config.X = new float[][] { new float[] { 0 } };
            //config.Y = new float[][] { new float[] { -2 }, new float[] { 0.5f } };

            // config.Radius = new float[][] { new float[] { 0 }, new float[] { 0.5f } };
            //config.RadiusA = new float[][] { new float[] { 10 }
            ////, new float[] { -0.5f }, new float[] { }, new float[] { 0.5f } 
            //};
            //config.Scale = new float[][] { new float[] { 1 }, new float[] { 0.1f } };

            //config.Type = true;
            //config.Projectile = "JunkoLily";
            //config.HitInterval = 60;
            config.Range = new float[][] { new float[] { 30 }, new float[] { 0 }, new float[] { 0 } };
            config.ZeroHitNotDie = true;

            config.Color = new int[][] { new int[] { 1 }, new int[] { 1, 2, 3, 4, 5 } };
            return config;
        }
    }

    //[EntityLogic(typeof(KomachiModTestPiece1Def))]
    //public sealed class KomachiModTestPiece1 : Piece
    //{

    //}
}


