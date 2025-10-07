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
        


        public override PieceConfig MakeConfig()
        {
            return GetDefaultGunConfig();
        }

        /// <summary>
        /// This default config is the same config as in ShootW.
        /// Id: use ConvertGunId(N) where N is the id of the gun you want.
        /// Type: false is a normal bullet. True is laser.
        /// Projectile: The bullets used. See BulletConfig when dumping the configs of the game.
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
        /// 
        /// 
        /// X and Y: Changes the starting position of the bullet relative to the character shooting it. 
        /// Does not get mirrored when shot by the enemy.
        /// 4x2 Matrix.
        /// Row 0 is the starting offset of the bullet. If X is 1 then the bullet will spawn at the character.position.x + 1.
        /// Row 1 is a linear growth of the offset for every group. If X[1][0]= 1 then group 0 will start at X= 1 * 0, group 1 at X = 1 * 1, group 2 at X = 1 * 2 etc.
        /// Row 2 is an exponential growth depending on the group. Similar to the above. If X[2][0]=1, at GroupID = 1 X=1 * 1 * 1, at GroupID = 2 X = 1 * 2 * 2 = 4. Etc.
        /// Row 3 is a linear growth depending on the bulletID within the group (WayID). Same calculations as above.
        /// The final position is a sum of all the above rows.
        /// The second column is a randomizer for each of the rows' base value. If Row2 is (0,1), then the calculation is (0 + Random(-1, 1)) * GroupID * GroupID.
        /// 
        /// Scale: Affects the size of bullets. Note: If a bullet's scale is 0, it will be defaulted to 1.
        /// 4x2 Matrix.
        /// Row 0 is the starting scale of the bullet.
        /// Row 1 is a linear growth of the scale of the bullets for every group.
        /// Row 2 is an exponential growth depending on the group. Similar to the above.
        /// Row 3 is a linear growth depending on the bulletID within the group (WayID). Same calculations as above.
        /// The final scale is a sum of all the above rows.
        /// The second column is a randomizer for each of the rows' base value. If Row2 is (0,1), then the calculation is (0 + Random(-1, 1)) * GroupID * GroupID.
        ///
        /// Life: The lifetime of bullets in frames. Note: If a bullet's life is 0, it will be defaulted to 300 frames (5 secs).
        /// 4x2 Matrix.
        /// Row 0 is the starting lifetime of the bullet.
        /// Row 1 is a linear growth for every group.
        /// Row 2 is an exponential growth depending on the group. Similar to the above.
        /// Row 3 is a linear growth depending on the bulletID within the group (WayID). Same calculations as above.
        /// The final value is a sum of all the above rows.
        /// The second column is a randomizer for each of the rows' base value. If Row2 is (0,1), then the calculation is (0 + Random(-1, 1)) * GroupID * GroupID.
        ///
        /// Radius: Spawns the bullet at a distance from the player in the direction of the bullet itself.
        /// So if it's X, it will spawn as if the bullet spawned at zero and travelled X units.
        /// It's a 4x2 Matrix. Uses the same arraycalculate method as above im not gonna copy it.
        /// 
        /// RadiusA: Changes the angle of the bullet *after* its spawn position was determined by radius.
        /// So if the bullet angle is 0 (points right), and its radius is 1. It will spawn at X=1. After that, its angle will be changed by radiusA.\
        /// It's a 4x2 Matrix. Uses ArrayCalculate.
        /// 
        /// Shootend: Time for the player model to be in its "Shoot" animation in frames. Does not affect the spawning of the bullets itself.
        /// If 0, player animation will persist until all bullets have spawned.
        /// 
        /// FollowPiece: Must use the ID of an earlier piece within the same gun.
        /// If a gun's id is 100 and as such its range of pieces is 100XX.
        /// And you want to follow piece id 10020.
        /// The following piece must be >10020.
        /// Its actual effect is that given a following piece shares some groupIDs or wayIDs with the followed piece, 
        /// its bullets will copy the position and angles of the followed piece.
        /// So if Piece1 has a way of 3 and Piece2 has a way of 5. Both the same amount of groups.
        /// The first 3 bullets of each group in piece2 will copy the positions and angles of piece1.
        /// While the last 2 bullets of each group will be whatever the actual radius, XY, and angle calculations are.
        /// This does mean that if piece2 shares the exact same or lower group/way ids of piece1, those above properties will be overriden.
        /// 
        /// HitAmount: How many times a bullet can hit an enemy before it gets destroyed. Should be at least 1.
        /// But since the interval between each hit is so small, it's hard to control how it should work.
        /// You just gotta test and tune it until you find something that feels good if you care about this variable.
        /// 
        /// ZeroHitNotDie: If hitamount is 0, this makes it so the bullet never dies when hitting an enemy.
        /// 
        /// Hitinterval: You'd think it would control the interval between each hit in hitamount while inside an enemy.
        /// Nope, it only affects lasers. Not sure how exactly it affects them yet though.
        /// 
        /// Last Wave: Specifies that this piece is the last wave of bullets in the gun.
        /// More specifically, what it does is that the visual communication to update damage on enemy will only happen when this piece hits the enemy. 
        /// Other pieces that have lastwave false will not update damage when they collide with an enemy.
        /// In an aoe attack, the damage will be updated for all enemies once a bullet in this last wave hits any of the targets.
        /// 
        /// ParentPiece: Takes an index number.
        /// From the pieces in the gun's range of pieces, the one whose index is equal to this will be the "Parent" of this piece.
        /// Does not have an effect unless shoot type is 2 or 3.
        /// 
        /// ShootType: Seems to do with animation and parenting.
        /// Can be a value from 0-3.
        /// 0: The piece is shot normally, with animation from the player.
        /// 1: Same as 1, but there is no animation from the player.
        /// 2: This piece is a child of ParentPiece. 
        /// While the bullets of the parent piece are alive, the child pieces will copy the location of its parent when it's being shot.
        /// Note; There will be an error if the child is spawned while there are still unspawned bullets from the parent.
        /// Thus, make sure the start time of the child would happen when the parents' bullets are all alive.
        /// 
        /// 3: This piece is a child of ParentPiece.
        /// When the bullets of the parent is dead, this piece will be allowed to spawn its own bullets at the location of each dead parent bullet.
        /// However, the start time of the child piece has to be such that it starts while no parent bullets are alive.
        /// Else it will bug out in weird ways.
        /// So if the parent has a group of 5 and ginterval 10, so the bullets spawn at 0, 10, 20, until 40.
        /// And the piece has a start time less than 40, then it will bug out in a weird behaviour.
        /// Test it yourself. I have no idea why this happens.
        /// This bug unfortunately makes the usage of this shoot type much more narrow than it could be for multi group patterns.
        /// 
        /// AddParentAngle: In addition to copying the position of the parent bullet, the angle of the parent will also be added to the angles of this piece's bullets.
        /// 
        /// Roottype: Determines the relative spawn point of the bullet. To which it is attached to? 
        /// X, Y, and Radius variables offset from the spawn point of the root.
        /// Has 3 values: 0, 1, 2.
        /// 0: Default. Bullets spawn at the position of the shooter.
        /// 1: Bullets spawn at the position of the target.
        /// 2: X, Y are treated as world coordinates, with (0,0) being the center of the screen, regardless of the position of the shooter or target.
        /// Aiming can be wonky with roottype 2 so probably use it with Aim = 1.
        /// 
        /// 
        /// Aim: Determines the Aiming behaviour of the bullets. Has 6 values.
        /// 0: Default. Every calculates its aimings separately.
        /// 1: No aiming. The bullet shoots wherever according to its properties regardless of the shooter or target. Defaults to the right (0 degrees).
        /// 2: Used for multigroup patterns. Calculates the angle for the first group, then uses that angle for all subsequent groups.
        /// 3, 4, and 5 are the exact same as the first 3, but they are used on children and have the additional property of adding in the angle of the parent to themselves.
        /// It's kinda redundant when AddParentAngle exists. At least with shoot type 2.
        /// With shoot type 3, might be useful if the angle of the parent's angle changes overtime from its original angle, since this will add its angle on death.
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

        public override IdContainer GetId()
        {
            //var id = KomachiDefaultConfig.DefaultID(this);
            //Debug.Log($"Id of the piece is {id}");
            //return id;
            return MakeConfig().Id;
            GunManager reference;
            Projectile projectile;
            Bullet bullet;
            Laser laser;
            Launcher launcher;
        }
    }
}
