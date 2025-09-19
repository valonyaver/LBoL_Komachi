using KomachiMod.Config;
using LBoL.ConfigData;
using LBoL.Presentation.Bullet;
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
            GunManager reference;
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
        /// StartTime: Time until you begin shooting
        /// Group: "Rows" of bullets shot.
        /// GInterval: Interval between each row of bullet being shot, I assume in frames.
        /// 
        /// Way: Determines the amount of bullets by group. How many "lanes" or "Columns" it has. Its maximum bound is a 2x2 matrix.
        /// Assuming the first row is X1, X2. The second row is Y1, Y2. And the group id is I
        /// The amount of bullets in each group will = (X1 + Random(-X2+X2)) + ((Y1 + Random(-Y2, Y2)) * I)
        /// 
        /// GAngle: The angle of the center bullet of each group.
        /// Its limit is a 4x2 angle. 4 rows 2 columns. I'll split them this time into X1Y1, X2Y2, X3Y3, X4Y4
        /// X1 determines the base angle of the group.
        /// X2 is a linear growth multiplier.
        /// X3 is a quadratic growth multiplier.
        /// X4 changes the angle depending on the wayID of the bullet, rather than the groupID.
        /// the Y for each, similar to way above, is just a randomizer. It has the same formula. 
        /// So the final center angle becomes, assuming groupID is N, and wayID is M:
        /// X1 + X2 * N + X3 * N * N + X4 * M
        /// 
        /// Range: The spread angle. Determines the angle of the furthest bullet in either direction. 
        /// It works similarly to GAngle, a 4x2 matrix, but more research is needed as to exactly how the range is calculated.
        /// X1 is the base range. If it's 10 and the pattern has a way of 3, the 3 bullets angles will be 10, 0, -10.
        /// X2 increases the range for every group.
        /// X3 increases the range for every group quadratically.
        /// X4 is... weird. It changes the range depending on the way id, similarly to GAngle, but I don't know the exacts of how it works.
        /// 
        /// StartSpeed: Starting speed of the bullets.
        /// Ev Properties: Evolution of the bullet's speed over time.
        /// EvStart: Time in frames until the Ev puberty hits.
        /// EvNumber: The speed change. The bullets' speed eventually end up being StartingSpeed+EvNumber
        /// EvDuration: How much time it takes for the speed to reach its final value.
        /// EvType: I have no idea. Type of interpolation? Keep it at 1 unless testing.
        /// 
        /// Color: Determines the colours of bullets. Its function depends on how many subarrays it has. I'll call them rows.
        /// If there is one row, then the colour of all the bullets will just be the ID of the first element in that row.
        /// If there are 2 rows, then the element in the first row (I'll call it M) determines the mode of the colour spread, all the IDs being taken from row2.
        /// Assuming colours red, blue, green.
        /// If M = 1, the colours will cycle through the IDs in row2 by the groupID. First group is red, second is blue, third is green, fourth is red.
        /// If M = 2, the colours will cycle by the WayID. The first "Lane" of bullets will be red, second blue, third green, fourth red.
        /// If M = 3, the colours will be completely random per bullet.
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
