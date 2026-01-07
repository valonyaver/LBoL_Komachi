using KomachiMod.Enemies.Template;
using KomachiMod.GunName;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Normal.Guihuos;
using LBoL.EntityLib.EnemyUnits.Normal.Shenlings;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace KomachiMod.Enemies
{
    public sealed class KomachiModDivineSpiritEnemyDef : KomachiEnemyUnitTemplate
    {

        public override EnemyUnitConfig MakeConfig()
        {
            EnemyUnitConfig config = GetEnemyUnitDefaultConfig();

            //Color(s) of the exhibits the boss can drop (right-most exhibit).
            config.BaseManaColor = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };

            config.Type = EnemyType.Normal;
            config.ModleName = nameof(ShenlingWhite);
            config.IsPreludeOpponent = false;

            //Boss properties
            config.MaxHp = 23;
            config.MaxHpHard = 25;
            config.MaxHpLunatic = 27;

            config.MaxHpAdd = 0;

            // Double shoot
            config.Damage1 = 3;
            config.Damage1Hard = 4;
            config.Damage1Lunatic = 5;

            // Shoot and buff
            config.Damage2 = 6;
            config.Damage2Hard = 7;
            config.Damage2Lunatic = 8;

            // Block gained from the defend action
            config.Defend = 5;
            config.DefendHard = 7;
            config.DefendLunatic = 10;
            
            // FP Buff amount
            config.Count1 = 1;
            config.Count1Hard = 1;
            config.Count1Lunatic = 1;

            // Start of flawless counter
            config.Count2 = 3;
            config.Count2Hard = 3;
            config.Count2Lunatic = 3;
            
            config.PowerLoot = new MinMax();
            config.BluePointLoot = new MinMax();

            config.Gun1 = new List<string> { "优雅的华光" };
            config.Gun2 = new List<string> { "优美的光扇" };
            config.Gun3 = new List<string> { GunNameID.GetGunFromId(800) };
            config.Gun4 = new List<string> { GunNameID.GetGunFromId(800) };

            return config;
        }

        [EntityLogic(typeof(KomachiModDivineSpiritEnemyDef))]
        public sealed class KomachiModDivineSpiritEnemy : Shenling
        {
            public new enum MoveType
            {
                DoubleShoot,
                ShootAndBuff,
                Defend,
                FlawlessBuff
            }

            public new MoveType Last;
            public new MoveType Next;

            public KomachiMod komachi;

            public override string Name => LocalizeProperty("Name");

            protected override void OnEnterBattle(BattleController battle)
            {
                Next = MoveType.Defend;
                ReactBattleEvent(base.Battle.BattleStarted, OnBattleStarted);
            }

            protected override IEnumerable<BattleAction> OnBattleStarted(GameEventArgs arg)
            {
                yield return new ApplyStatusEffectAction<Amulet>(this, 1);
                yield return new CastBlockShieldAction(this, this, 0, base.MaxHp, BlockShieldType.Normal, cast: false);
            }

            public override void OnSpawn(EnemyUnit spawner)
            {
                counter = Count2;
                // Debug.Log($"{Name} has spawned. its special move is {FlawlessMove}. The counter is at {counter}");
                React(new ApplyStatusEffectAction<Amulet>(this, 1));
                React(new CastBlockShieldAction(this, this, 0, 10, BlockShieldType.Normal, cast: false));

                if (spawner is KomachiMod)
                {
                    komachi = spawner as KomachiMod;
                }

                HandleBattleEvent(komachi.TurnEnded, KomachiTurnEnded);
            }

            

            #region Moves
            protected override IEnumerable<IEnemyMove> GetTurnMoves()
            {
                switch (Next)
                {
                    // Shoots twice
                    case MoveType.DoubleShoot:
                        yield return AttackMove(GetMove(0), base.Gun1, base.Damage1, 2);
                        Last = MoveType.DoubleShoot;
                        break;
                        // Shoots then buffs everyone, permanent FP for summons and temporary for others.
                    case MoveType.ShootAndBuff:
                        yield return new SimpleEnemyMove(Intention.Attack(Damage2), ShootAndBuff());
                        yield return new SimpleEnemyMove(Intention.PositiveEffect());
                        Last = MoveType.ShootAndBuff;
                        break;
                        // Gives block to self and barrier to non summons
                    case MoveType.Defend:
                        yield return new SimpleEnemyMove(Intention.Defend(), DefendKoma());
                        Last = MoveType.Defend;
                        break;
                    case MoveType.FlawlessBuff:
                        yield return new SimpleEnemyMove(Intention.PositiveEffect().WithMoveName(FlawlessMove), FlawlessBuff());
                        Last = MoveType.FlawlessBuff;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            IEnumerable<BattleAction> ShootAndBuff()
            {
                yield return new DamageAction(this, Battle.Player, DamageInfo.Attack(Damage2), Gun2);
                foreach(var enemy in Battle.AllAliveEnemies)
                {
                    if (enemy.IsServant)
                    {
                        yield return new ApplyStatusEffectAction<Firepower>(enemy, Count1);
                    }
                    else
                    {
                        yield return new ApplyStatusEffectAction<KomachiModBossTempFirepowerSe>(enemy, Count1);
                    }
                }
            }

            IEnumerable<BattleAction> DefendKoma()
            {
                Debug.Log("Applying defence to Komachi");
                yield return new CastBlockShieldAction(this, Defend, 0, cast: false);
                foreach (var enemy in Battle.AllAliveEnemies)
                {
                    if (!enemy.IsServant)
                    {
                        yield return new CastBlockShieldAction(enemy, 0, Defend, cast: false);
                    }
                }
            }
            public string FlawlessMove => LocalizeProperty("FlawlessMove");
            IEnumerable<BattleAction> FlawlessBuff()
            {
                foreach(var enemy in Battle.AllAliveEnemies)
                {
                    if (!enemy.IsServant)
                    {
                        yield return new ApplyStatusEffectAction<Invincible>(enemy, duration: 1, startAutoDecreasing: false);
                    }
                }
            }
            #endregion

            List<MoveType> fillerMoves = new List<MoveType>()
            {
                MoveType.ShootAndBuff,
                MoveType.Defend,
                MoveType.DoubleShoot
            };

            List<MoveType> fillerMovesUsed = new List<MoveType>();


            int counter;

            // Has a counter. At the end of each turn decrement the counter.
            // If komachi is using her spellcard, or if the counter is at 0, apply flawless.
            // Otherwise use a bunch of rotating filler moves while that counter is up.
            // KomachiTurnEnded makes sure shoot and buff doesn't get used at a turn where komachi isn't gonna attack.
            void ChooseRandomNextMove(MoveType lastMove, out MoveType nextMove)
            {
                // Debug.Log($"Divine spirit counter is at {counter}. Reducing by 1 to {counter-1}.");
                counter--;
                // Either the counter is 0,
                // or the next move is spellcard and the move wasn't just used last turn
                if ((komachi.NextNext == KomachiMod.MoveType.Spellcard && counter < Count2 - 1) || counter <= 0)
                {
                    // Debug.Log($"Divine spirit intention will be flawless");
                    nextMove = MoveType.FlawlessBuff;
                    counter = Count2;
                    return;
                }
                if (fillerMoves.Contains(lastMove))
                {
                    fillerMovesUsed.Add(lastMove);
                    var availableMoves = fillerMoves.Except(fillerMovesUsed).ToList();

                    // If all moves used in this cycle, trigger Flawless Buff
                    if (availableMoves.Count == 0)
                    {
                        fillerMovesUsed.Clear(); // Reset for next cycle
                        nextMove = fillerMoves[EnemyMoveRng.NextInt(0, fillerMoves.Count-1)];
                    }
                    else
                    {
                        // Pick random remaining move
                        nextMove = availableMoves[EnemyMoveRng.NextInt(0, availableMoves.Count-1)];
                    }
                }
                else nextMove = MoveType.DoubleShoot;
            }

            // Does not use buff and shoot if komachi won't attack next turn.
            // Should probably lock this to hard and above, so that the normal version becomes a bit more dumb.
            public void KomachiTurnEnded(UnitEventArgs args)
            {
                if (komachi.GetNextMoveDisplacement(komachi.NextNext) >= 0 && Next == MoveType.ShootAndBuff)
                {
                    // Debug.Log($"Removing shoot and buff.");
                    counter++;
                    ChooseRandomNextMove(Next, out Next);
                    fillerMovesUsed.Remove(MoveType.ShootAndBuff);
                    UpdateTurnMoves();
                }
            }

            protected override void UpdateMoveCounters()
            {
                ChooseRandomNextMove(Last, out Next);
            }
        }
    }
}
