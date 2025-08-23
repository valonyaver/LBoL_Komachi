using KomachiMod.Config;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KomachiMod.Source.GunName
{
    public class KomachiPieceTemplate : PieceTemplate
    {
        public override IdContainer GetId()
        {
            //var id = KomachiDefaultConfig.DefaultID(this);
            //Debug.Log($"Id of the piece is {id}");
            //return id;
            return MakeConfig().Id;
        }


        public override PieceConfig MakeConfig()
        {
            return GetDefaultGunConfig();
        }

        /// <summary>
        /// This default config is the same config as in ShootW.
        /// Id: use ConvertGunId(N) where N is the id of the gun you want.
        /// Type: false is a normal bullet. True is laser.
        /// Projectile: The bullets used. See BulletConfig when dumping the configs of the game.
        /// ShootType: I'm not sure? Something to do with animation. Keep it at 0.
        /// HitAmount: I have no idea. It doesn't seem to dictate the amount of "Hits" that an enemy is taking.
        /// Color: Colour. Duh. Change the number and you get a different one. Idk why it's an array though.
        /// StartTime: Time until you begin shooting
        /// Group: "Rows" of bullets shot.
        /// GInterval: Interval between each row of bullet being shot, I assume in frames.
        /// Way: Amount of bullet change per group. 
        /// First row always has 1 bullet. Second row has Way1+Way2 bullets. Subsequent rows have Way1+(Way2*N) bullets.
        /// (2, 3) means that first row has 1 bullet. Second has 2+3 bullets. Third has 2 + 3 + 3 bullets. 
        /// There are probably other combinations if you mess with how the array is made.
        /// StartSpeed: Starting speed of the bullets.
        /// Ev Properties: Evolution of the bullet's speed over time.
        /// EvStart: Time in frames until the Ev puberty hits.
        /// EvNumber: The speed change. The bullets' speed eventually end up being StartingSpeed+EvNumber
        /// EvDuration: How much time it takes for the speed to reach its final value.
        /// EvType: I have no idea. Type of interpolation? Keep it at 1 unless testing.
        /// Other values need a lot more testing.
        /// </summary>
        /// <returns></returns>
        public static PieceConfig GetDefaultGunConfig()
        {
            PieceConfig config = new PieceConfig(
            Id: 0,
            Type: false,
            Projectile: "DanmaFish",
            ShootType: 0,
            ParentPiece: 0,
            AddParentAngle: false,
            LastWave: true,
            FollowPiece: 0,
            ShootEnd: 0,
            HitAmount: 1,
            HitInterval: 6,
            ZeroHitNotDie: false,
            Scale: new float[][] { },
            Color: new int[][] { new int[] { 3 } },
            RootType: 0,
            X: new float[][] { },
            Y: new float[][] { },
            Radius: new float[][] { },
            RadiusA: new float[][] { },
            Aim: 0,
            StartTime: 0,
            GInterval: 3,
            Group: 4,
            Way: new int[][] { new int[] { 1 }, new int[] { 1 } },
            GAngle: new float[][] { new float[] { 0 } },
            Range: new float[][] { new float[] { 0 }, new float[] { 13 } },
            Life: new int[][] { },
            LaserLastWave: 0,
            StartSpeed: new float[][] { new float[] { 7 } },
            StartAcc: new float[][] { },
            StartAccAngle: new float[][] { },
            EvStart: new int[][][] { new int[][] { new int[] { 8 } } },
            EvDuration: new int[][][] { new int[][] { new int[] { 20 } } },
            EvNumber: new float[][][] { new float[][] { new float[] { 20 } } },
            EvType: new int[][] { new int[] { 1 } },
            VanishV3: new Vector3(0.08f, 0.08f, 0.08f),
            LaunchSfx: "",
            HitBodySfx: "",
            HitAnimationSpeed: 1f
            );
            return config;
        }
    }
}
