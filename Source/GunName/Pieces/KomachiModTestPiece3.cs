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
            config.GInterval = 2;
            config.Group = 6;
            config.Way = new int[][] { new int[] { 2 }, new int[] { 2 } };

            config.StartTime = 120;
            config.StartSpeed = new float[][] { new float[] { 3 } };
            config.EvStart = new int[][][] { new int[][] { new int[] { 60 } } };
            config.EvDuration = new int[][][] { new int[][] { new int[] { 60 } } };
            config.EvNumber = new float[][][] { new float[][] { new float[] { 4 } } };
            config.EvType = new int[][] { new int[] { 1 } };
            config.HitAmount = 1;
            config.HitInterval = 60;

            config.Color = new int[][] { new int[] { 4 } };

            return config;
        }
    }

    //[EntityLogic(typeof(KomachiModTestPiece1Def))]
    //public sealed class KomachiModTestPiece1 : Piece
    //{

    //}
}


