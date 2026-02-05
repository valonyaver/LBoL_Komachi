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
    public sealed class LonelyBoundSpiritUnitPiece : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();

            config.Id = PieceTemplate.ConvertGunId(444003, 0);
            config.Projectile = "GuihuoUnitBlue";
            config.ShootType = 0;
            config.Group = 1;
            config.GAngle = BulletMatrixHelper.Matrix(-3);
            config.StartTime = 6;
            config.Way = BulletMatrixHelper.Way(1);
            config.Range = BulletMatrixHelper.Matrix(0);
            config.StartSpeed = BulletMatrixHelper.Constant(3);
            config.LastWave = true;
            config.Color = BulletColorHelper.Constant(BulletColor.Purple);
            config.HitAmount = 2;
            config.Life = BulletMatrixHelper.ConstantInt(120);
            config.HitBodySfx = "JunkoIceFire";

            // Recreating the "Wiggle" movement pattern using the Event Builder
            BulletEventBuilder eventBuilder = new BulletEventBuilder();

            // Event 1: Initial Speed/Direction Setup (0.08 scaling factor equivalent)
            eventBuilder.Add(BulletEventType.Speed, 7f, 0, 120, EventMode.Add);


            // Apply the constructed events to the config
            BulletEventBuilder.ApplyEvents(config, eventBuilder);

            return config;
        }
    }
    #region FOLLOW UP PIECE
    public sealed class LonelyBoundSpiritExplosionPiece : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();
            config.Id = PieceTemplate.ConvertGunId(444003, 1);
            config.Projectile = "FlameJunko";
            config.ParentPiece = 0;
            config.ShootType = 3;
            config.FollowPiece = 0;
            config.RootType = 0;

            // Movement and Shape logic
            config.GInterval = 0;
            config.Group = 1;
            config.Way = BulletMatrixHelper.Way(baseCount: 7);
            config.Range = BulletMatrixHelper.Constant(360f);
            config.GAngle = BulletMatrixHelper.Constant(-20f);
            config.StartSpeed = BulletMatrixHelper.Constant(10f);
            config.Scale = BulletMatrixHelper.Constant(1f);
            config.Color = BulletColorHelper.Constant(BulletColor.Purple);

            // Collision and Life settings
            config.HitAmount = 1;
            config.HitInterval = 6;
            config.ZeroHitNotDie = true;
            config.Life = BulletMatrixHelper.MatrixInt(30);

            config.LastWave = false;

            // Recreating the "Wiggle" movement pattern using the Event Builder
            BulletEventBuilder eventBuilder = new BulletEventBuilder();

            // Event 1: Initial Speed/Direction Setup (0.08 scaling factor equivalent)
            eventBuilder.Add(BulletEventType.Speed, 60f, 0, 100, EventMode.Add);

            // Events 2-6: The Oscillating "Wiggle"
            // This creates that zig-zag movement (40 -> -40 -> 40...)
            int[] wiggleDurations = { 20, 20, 20, 20, 20 };
            float[] wiggleAmounts = { 40f, -40f, 40f, -40f, 40f };
            int startTime = 0;

            for (int i = 0; i < wiggleAmounts.Length; i++)
            {
                eventBuilder.Add(
                    BulletEventType.Angle,
                    wiggleAmounts[i],
                    startTime,
                    wiggleDurations[i],
                    EventMode.Add
                );
                startTime += wiggleDurations[i];
            }

            // Apply the constructed events to the config
            BulletEventBuilder.ApplyEvents(config, eventBuilder);
            return config;
        }
    }
    #endregion

    public sealed class LonelyBoundSpiritSurroundingPiece : KomachiPieceTemplate
    {
        public override PieceConfig MakeConfig()
        {
            PieceConfig config = GetDefaultGunConfig();

            config.Id = PieceTemplate.ConvertGunId(444003, 2);
            config.Projectile = "FlameJunko";
            config.ShootType = 0;
            config.RootType = 1;
            config.Group = 8;
            config.GInterval = 3;
            config.Way = BulletMatrixHelper.Way(1);
            config.Range = BulletMatrixHelper.Matrix(0, perGroup: 0);
            config.GAngle = BulletMatrixHelper.Matrix(0, perGroup: 360f / config.Group);
            config.Scale = BulletMatrixHelper.Constant(0);
            config.Radius = BulletMatrixHelper.Constant(2);
            config.StartSpeed = BulletMatrixHelper.Constant(0);
            config.Color = BulletColorHelper.Constant(BulletColor.Magenta);
            config.HitAmount = 2;
            config.ZeroHitNotDie = false;
            config.Life = BulletMatrixHelper.MatrixInt(120);
            config.LastWave = false;

            BulletEventBuilder eventBuilder = new BulletEventBuilder();

            // Amount of time till all spawns end
            int startWait = config.Group * config.GInterval;

            eventBuilder.Add(
                BulletEventType.Speed,
                number: BulletMatrixHelper.Constant(1),
                start: BulletMatrixHelper.MatrixInt(startWait, perGroup: -config.GInterval),
                duration: BulletMatrixHelper.ConstantInt(startWait / 2)
            );

            eventBuilder.Add(
                BulletEventType.Speed,
                number: BulletMatrixHelper.Constant(-1f),
                start: BulletMatrixHelper.MatrixInt(startWait * 2, perGroup: -config.GInterval),
                duration: BulletMatrixHelper.ConstantInt(0)
            );

            eventBuilder.Add(
                BulletEventType.MoveForward,
                number: BulletMatrixHelper.Constant(-5f),
                start: BulletMatrixHelper.MatrixInt(startWait * 3, perGroup: -config.GInterval),
                duration: BulletMatrixHelper.ConstantInt(40)
            );

            BulletEventBuilder.ApplyEvents(config, eventBuilder);

            return config;
        }
    }
}


