using LBoL.ConfigData;
using System;
using System.Collections.Generic;
using System.Text;

namespace KomachiMod.Source.Guns
{
    /// <summary>
    /// Bullet event types that modify bullet behavior over time
    /// </summary>
    public enum BulletEventType
    {
        /// <summary>
        /// Modifies Bullet Speed
        /// </summary>
        Speed = 1,
        /// <summary>
        /// Modifies bullet angle/direction
        /// </summary>
        Angle = 2,
        /// <summary>
        /// Modifies acceleration
        /// </summary>
        Acceleration = 3,
        /// <summary>
        /// Modifies acceleration angle
        /// </summary>
        AccelerationAngle = 4,
        /// <summary>
        /// Aims toward target.
        /// If its event mode is 1 (transition), then it will instantly snap to the target.
        /// Use the special homing method for it.
        /// </summary>
        Homing = 5,
        /// <summary>
        /// Bounces off screen boundaries (mirror reflection). Does not bounce off the wall on the opposite side of the shooter.
        /// If its event mode is 1, it will aim towards the target at reflection instead.
        /// Use special bouncing method for it.
        /// </summary>
        BounceReflect = 9,
        /// <summary>
        /// Bounces to cardinal directions at boundaries (hitting the bottom changes its angle straight up). Does not bounce off the wall on the opposite side of the shooter.
        /// If its event mode is 1, it will aim towards the target at reflection.
        /// Use special bouncing method for it.
        /// </summary>
        BounceCardinal = 10,
        /// <summary>
        /// Directly changes X position
        /// </summary>
        PositionX = 11,
        /// <summary>
        /// Directly changes Y position
        /// </summary>
        PositionY = 12,
        /// <summary>
        /// Changes both X and Y scale
        /// </summary>
        ScaleUniform = 13,
        /// <summary>
        /// Changes Y scale only
        /// </summary>
        ScaleY = 14,
        /// <summary>
        /// Changes X scale only
        /// </summary>
        ScaleX = 15,
        /// <summary>
        /// Moves forward in bullet's direction
        /// </summary>
        MoveForward = 16,
        /// <summary>
        /// Moves perpendicular to bullet's direction
        /// </summary>
        MovePerpendicular = 17,
        /// <summary>
        /// Moves forward in AccAngle direction.
        /// Used for bullets that want to have a different direction to its sprite compared to its actual movement direction.
        /// For example, spinning objects.
        /// </summary>
        MoveAccAngleForward = 18,
        /// <summary>
        /// Moves perpendicular to AccAngle direction
        /// </summary>
        MoveAccAnglePerpendicular = 19,
        /// <summary>
        /// Custom sine-based movement. Ignores duration.
        /// </summary>
        Huali = 99           
    }

    /// <summary>
    /// Event calculation mode (affects how EventNumber is applied)
    /// </summary>
    public enum EventMode
    {
        /// <summary>
        /// Add EventNumber directly over duration
        /// </summary>
        Add = 0,
        /// <summary>
        /// Transition from current value to EventNumber
        /// </summary>
        Transition = 1,
        /// <summary>
        /// Multiply current value by EventNumber over duration
        /// </summary>
        Multiply = 2     
    }

    /// <summary>
    /// Represents a single bullet event
    /// </summary>
    public class BulletPieceEvent
    {
        public BulletEventType Type { get; set; }
        public EventMode Mode { get; set; }
        public float[][] Number { get; set; }
        public int[][] Start { get; set; }
        public int[][] Duration { get; set; }

        public BulletPieceEvent(
            BulletEventType type,
            float[][] number,
            int[][] start,
            int[][] duration,
            EventMode mode = EventMode.Add)
        {
            Type = type;
            Mode = mode;
            Number = number;
            Start = start;
            Duration = duration;
        }
    }

    /// <summary>
    /// Builder class for constructing bullet events
    /// </summary>
    public class BulletEventBuilder
    {
        private List<BulletPieceEvent> events = new List<BulletPieceEvent>();

        /// <summary>
        /// Adds a preconstructed event
        /// </summary>
        public BulletEventBuilder Add(BulletPieceEvent bulletEvent)
        {
            events.Add(bulletEvent);
            return this;
        }

        /// <summary>
        /// Adds an event with constant values (no group/way variation)
        /// </summary>
        public BulletEventBuilder Add(
            BulletEventType type,
            float value = 1,
            int startTime = 0,
            int duration = 1,
            EventMode mode = EventMode.Add)
        {
            events.Add(new BulletPieceEvent(
                type,
                BulletMatrixHelper.Constant(value),
                BulletMatrixHelper.ConstantInt(startTime),
                BulletMatrixHelper.ConstantInt(duration),
                mode
            ));
            return this;
        }

        /// <summary>
        /// Adds an event with custom matrices for start, duration, and value
        /// </summary>
        public BulletEventBuilder Add(
            BulletEventType type,
            float[][] number,
            int[][] start,
            int[][] duration,
            EventMode mode = EventMode.Add)
        {
            events.Add(new BulletPieceEvent(type, number, start, duration,  mode));
            return this;
        }

        /// <summary>
        /// Adds an event with matrix builders for more complex patterns
        /// </summary>
        public BulletEventBuilder AddComplex(
            BulletEventType type,
            float valueBase = 1, float valuePerGroup = 0f, float valuePerWay = 0f,
            int startBase = 0, int startPerGroup = 0, int startPerWay = 0,
            int durationBase = 60, int durationPerGroup = 0, int durationPerWay = 0,
            EventMode mode = EventMode.Add)
        {
            events.Add(new BulletPieceEvent(
                type,
                BulletMatrixHelper.Matrix(
                    baseValue: valueBase,
                    perGroup: valuePerGroup,
                    perWay: valuePerWay
                ),
                BulletMatrixHelper.MatrixInt(
                    baseValue: startBase,
                    perGroup: startPerGroup,
                    perWay: startPerWay
                ),
                BulletMatrixHelper.MatrixInt(
                    baseValue: durationBase,
                    perGroup: durationPerGroup,
                    perWay: durationPerWay
                ),
                mode
            ));
            return this;
        }

        /// <summary>
        /// Adds a homing event (targets the enemy/player).
        /// Turn rate must be 0 for snap to target to work.
        /// </summary>
        public BulletEventBuilder AddHoming(
            int startTime,
            int duration = 1,
            float turnRate = 0f,
            bool snapToTarget = false)
        {
            // turnRate = 0 with snapToTarget uses special mode [5, 1]
            EventMode mode = (turnRate == 0f && snapToTarget) ? (EventMode)1 : EventMode.Add;

            events.Add(new BulletPieceEvent(
                BulletEventType.Homing,
                BulletMatrixHelper.Constant(turnRate),
                BulletMatrixHelper.ConstantInt(startTime),
                BulletMatrixHelper.ConstantInt(duration),
                mode
            ));
            return this;
        }

        /// <summary>
        /// Add a bounce event.
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="bounceCount">BROKEN. How many times the bullets will bounce. Does not currently work properly due to a bug in the game.
        /// Technically can work for cardinal if it's high enough but you'll have to test it since it can be inconsistent.
        /// Keep at 0 otherwise.</param>
        /// <param name="useCardinalDirections"></param>
        /// <param name="aimAtTargetOnBounce">If true, will aim at target on the bullets' last bounce.</param>
        /// <returns></returns>
        public BulletEventBuilder AddBounce(
            int startTime,
            int bounceCount = 0,
            bool useCardinalDirections = false,
            bool aimAtTargetOnBounce = false)
        {
            BulletEventType type = useCardinalDirections
                ? BulletEventType.BounceCardinal
                : BulletEventType.BounceReflect;

            EventMode mode = aimAtTargetOnBounce ? (EventMode)1 : EventMode.Add;

            events.Add(new BulletPieceEvent(
                type,
                BulletMatrixHelper.Constant(bounceCount),
                BulletMatrixHelper.ConstantInt(startTime),
                BulletMatrixHelper.ConstantInt(120), // Duration doesn't matter for bounces
                mode
            ));
            return this;
        }

        /// <summary>
        /// Clears all events
        /// </summary>
        public BulletEventBuilder Clear()
        {
            events.Clear();
            return this;
        }

        /// <summary>
        /// Gets the number of events
        /// </summary>
        public int Count => events.Count;

        /// <summary>
        /// Applies all events to a PieceConfig
        /// </summary>
        public static void ApplyEvents(PieceConfig config, BulletEventBuilder builder)
        {
            if (builder.events.Count == 0)
            {
                // Set empty arrays if no events
                config.EvStart = new int[][][] { };
                config.EvDuration = new int[][][] { };
                config.EvNumber = new float[][][] { };
                config.EvType = new int[][] { };
                return;
            }

            int eventCount = builder.events.Count;

            config.EvStart = new int[eventCount][][];
            config.EvDuration = new int[eventCount][][];
            config.EvNumber = new float[eventCount][][];
            config.EvType = new int[eventCount][];

            for (int i = 0; i < eventCount; i++)
            {
                BulletPieceEvent evt = builder.events[i];

                config.EvStart[i] = evt.Start;
                config.EvDuration[i] = evt.Duration;
                config.EvNumber[i] = evt.Number;

                // Handle event type array (type + optional mode)
                if (evt.Mode == EventMode.Add)
                {
                    config.EvType[i] = new int[] { (int)evt.Type };
                }
                else
                {
                    config.EvType[i] = new int[] { (int)evt.Type, (int)evt.Mode };
                }
            }
        }
    }

    /// <summary>
    /// Pre-configured event patterns for common use cases
    /// </summary>
    public static class EventPatterns
    {
        /// <summary>
        /// Bullet accelerates over time
        /// </summary>
        public static BulletPieceEvent Accelerate(int startTime, float targetSpeed, int duration = 1)
        {
            return new BulletPieceEvent(
                BulletEventType.Speed,
                BulletMatrixHelper.Constant(targetSpeed),
                BulletMatrixHelper.ConstantInt(startTime),
                BulletMatrixHelper.ConstantInt(duration),
                EventMode.Transition
            );
        }

        /// <summary>
        /// Bullet decelerates over time
        /// </summary>
        public static BulletPieceEvent Decelerate(int startTime, float targetSpeed, int duration)
        {
            return Accelerate(startTime, targetSpeed, duration);
        }

        /// <summary>
        /// Bullet comes to a stop, then reverses direction
        /// </summary>
        public static BulletEventBuilder StopAndReverse(int stopTime, int reverseDuration)
        {
            var builder = new BulletEventBuilder();
            builder.Add(BulletEventType.Speed, 0f, stopTime, reverseDuration, EventMode.Transition);
            builder.Add(BulletEventType.Angle, 180f, stopTime + reverseDuration, 1);
            builder.Add(BulletEventType.Speed, 3f, stopTime + reverseDuration, reverseDuration, EventMode.Transition);
            return builder;
        }

        /// <summary>
        /// Bullet curves in a direction
        /// </summary>
        public static BulletPieceEvent Curve(int startTime, float angleChangePerFrame, int duration)
        {
            return new BulletPieceEvent(
                BulletEventType.Angle,
                BulletMatrixHelper.Constant(angleChangePerFrame),
                BulletMatrixHelper.ConstantInt(startTime),
                BulletMatrixHelper.ConstantInt(duration),
                EventMode.Add
            );
        }

        /// <summary>
        /// Bullet grows over time
        /// </summary>
        public static BulletPieceEvent Grow(int startTime, float scaleChange, int duration)
        {
            return new BulletPieceEvent(
                BulletEventType.ScaleUniform,
                BulletMatrixHelper.Constant(scaleChange),
                BulletMatrixHelper.ConstantInt(startTime),
                BulletMatrixHelper.ConstantInt(duration),
                EventMode.Add
            );
        }

        /// <summary>
        /// Bullet shrinks over time
        /// </summary>
        public static BulletPieceEvent Shrink(int startTime, float scaleChange, int duration)
        {
            return Grow(startTime, -scaleChange, duration);
        }
    }
}
