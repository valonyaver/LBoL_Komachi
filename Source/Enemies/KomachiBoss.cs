using KomachiMod.BattleActions;
using KomachiMod.Cards;
using KomachiMod.Enemies.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.EventManager;
using KomachiMod.Source.Enemies.Intentions;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Normal.Guihuos;
using LBoL.EntityLib.EnemyUnits.Normal.Shenlings;
using LBoL.EntityLib.EnemyUnits.Opponent;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.Presentation.Effect;
using LBoL.Presentation.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static KomachiMod.Enemies.KomachiModDivineSpiritEnemyDef;
using static KomachiMod.Enemies.KomachiModVengefulSpiritEnemyDef;
using static LBoL.EntityLib.EnemyUnits.Character.Rin;
using static LBoL.EntityLib.EnemyUnits.Opponent.Reimu;
using static LBoL.EntityLib.EnemyUnits.Character.Siji;
using KomachiMod.KomachiUlt;




namespace KomachiMod.Enemies
{

    [EntityLogic(typeof(KomachiBossUnitDef))]
    public sealed class KomachiMod : EnemyUnit
    {
        public int DamageLastWordNormal => 24;
        public int DamageLastWordHard => 27;
        public int DamageLastWordLunatic => 30;
        public int DamageLastWord
        {
            get
            {
                switch(GameRun.Difficulty)
                {
                    case GameDifficulty.Easy:
                    case GameDifficulty.Normal:
                    default:
                        return DamageLastWordNormal;
                    case GameDifficulty.Hard:
                        return DamageLastWordHard;
                    case GameDifficulty.Lunatic:
                        return DamageLastWordLunatic;
                }
            }
        }
        public string GunLastWord => GunNameID.GetGunFromId(4650);

        //Internal list of the boss moves
        public enum MoveType
		{
            Summon,
            AttackMulti,
            AttackAccurate,
            AttackBuff,
            Defend,
            Debuff,
            Spellcard,
            Summon2,
            Nothing,
            LastWord,
            Escape
		}

        //Internal parameters use to track the last move used by the boss.
        public MoveType Last;
        public MoveType Next;
        public MoveType NextNext;
        
        public string SpellcardName => GetSpellCardName(6, 7);
        public string LastWordName => GetSpellCardName(8, 9);
        public bool hasRevived;
        public bool isSummon => HasStatusEffect<MirrorImage>();

        protected override void OnEnterBattle(BattleController battle)
		{
            Last = MoveType.AttackAccurate;
			Next = MoveType.Summon;
            NextNext = MoveType.AttackMulti;
            // Adds the revive status and deals with bribery stuffs
            ReactBattleEvent(base.Battle.BattleStarted, OnBattleStarted);
            // Updates the defence intention
            HandleBattleEvent(KomachiEventsManager.DistanceChanged, OnDistanceChanged);
            return;
            // quik reference to Reimu from visual studio
            var thing = Reimu.MoveType.Spell;
        }
        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs arg)
        {
            // Bribery stuff
            if (!isSummon)
            {
                KomachiModSanzuFare komachiBribeCard = Library.CreateCard<KomachiModSanzuFare>();
                yield return new AddCardsToHandAction(new List<Card>() { komachiBribeCard });
                HandleBattleEvent(base.StatusEffectAdded, OnStatusEffectAdded);
                HandleBattleEvent(base.DamageReceived, OnDamageReceived);
            }


            yield return new ApplyStatusEffectAction<KomachiModBossReviveSe>(this);
            GetStatusEffect<KomachiModBossReviveSe>().Activating += OnRevival;
        }



        public override void OnSpawn(EnemyUnit spawner)
        {
            Last = MoveType.AttackAccurate;
            Next = MoveType.Summon;
            NextNext = MoveType.AttackMulti;
            EikiBoss = Battle.AllAliveEnemies.Where(u  => u is Siji).FirstOrDefault();
            React(new ApplyStatusEffectAction<MirrorImage>(this));
            React(SpawnDialogue());
            // Updates the defence intention
            HandleBattleEvent(KomachiEventsManager.DistanceChanged, OnDistanceChanged);
        }

        public Unit EikiBoss;

        public string EikiMirrorChat1 => LocalizeProperty("EikiMirrorChat1");
        public string EikiMirrorChat2 => LocalizeProperty("EikiMirrorChat2");
        public string EikiMirrorChat3 => LocalizeProperty("EikiMirrorChat3");


        IEnumerable<BattleAction> SpawnDialogue()
        {
            yield return PerformAction.Chat(this, EikiMirrorChat1, 6f, waitTime: 3f);
            yield return PerformAction.Chat(Battle.Player, EikiMirrorChat2, 2.5f, waitTime: 2f);
            yield return PerformAction.Chat(EikiBoss, EikiMirrorChat3, 2, waitTime: 2f);
        } 

        public void OnRevival()
        {
            foreach(var enemy in AllAliveEnemies)
            {
                if (enemy.IsServant){
                    if (enemy.TryGetStatusEffect<DeathExplode>(out var deathExplode))
                    {
                        React(new RemoveStatusEffectAction(deathExplode));
                    }
                    React(new ForceKillAction(this, enemy));
                }
            }
            Next = MoveType.Nothing;
            NextNext = MoveType.LastWord;
            hasRevived = true;
            UpdateTurnMoves();
            // To make sure the player doesn't get unnecessary damage from the turn skip
            if (TryGetStatusEffect<KomachiModGuidedSpiritSe>(out var spirits))
            {
                React(new RemoveStatusEffectAction(spirits));
            }
            React(SkipTurns);
        }

        public string LastWordChat1 => LocalizeProperty("LastWordChat1");
        public string LastWordChat2 => LocalizeProperty("LastWordChat2");

        IEnumerable<BattleAction> SkipTurns()
        {
            yield return PerformAction.Effect(this, "UnitDeathLarge", waitTime:1.5f);
            yield return PerformAction.Sfx("UnitDeathExplodeLarge");
            yield return PerformAction.Effect(this, "Empty", waitTime: 4);
            yield return PerformAction.Chat(this, LastWordChat1, 3, waitTime:3);
            yield return PerformAction.Chat(this, LastWordChat2, 3, waitTime: 3.5f);


            if (Battle.Player.IsInTurn)
            {
                yield return new RequestEndPlayerTurnAction();
            }
            GameRun.SetEnemyHpAndMaxHp(4, MaxHp, this, true);
        }

        

        /// <summary>
        /// Updates defend move
        /// </summary>
        /// <param name="args"></param>
        public void OnDistanceChanged(DistanceChangedEventArgs args)
        {
            if (args.Unit == this && Next == MoveType.Defend)
            {
                UpdateTurnMoves();
            }
        }
        /// <summary>
        /// Escape code
        /// When damaged turn to run when hp is low.
        /// </summary>
        /// <param name="args"></param>
        public void OnDamageReceived(DamageEventArgs args)
        {
            if (!HasStatusEffect<LongEscape>() || hasRevived)
            {
                return;
            }

            LongEscape statusEffect = GetStatusEffect<LongEscape>();
            if (base.Hp <= statusEffect.Level)
            {
                Next = MoveType.Escape;
                if (!base.IsInTurn)
                {
                    UpdateTurnMoves();
                }
            }
        }
        /// <summary>
        /// Escape code
        /// If added while her hp is low (somehow), then turn to escape
        /// </summary>
        /// <param name="arg"></param>
        public void OnStatusEffectAdded(StatusEffectApplyEventArgs arg)
        {
            if (!(arg.Effect is LongEscape) || hasRevived)
            {
                return;
            }

            LongEscape statusEffect = GetStatusEffect<LongEscape>();
            if (base.Hp <= statusEffect.Level)
            {
                Next = MoveType.Escape;
                if (base.IsInTurn)
                {
                    Debug.LogError("LongEscape should not add to Komachi during her turn.");
                }
                else
                {
                    UpdateTurnMoves();
                }
            }
        }

        //Action for the turn.
        protected override IEnumerable<IEnemyMove> GetTurnMoves()
		{
            Debug.Log($"Komachi get turn moves: current move {this.Next}");
            // Intentions and stuff
            switch (this.Next)
            {
                case MoveType.Summon:
                    {
                        yield return new SimpleEnemyMove(Intention.Spawn().WithMoveName(GetMove(0)), Summon1Action());
                        Last = MoveType.Summon;
                        break;
                    }
                case MoveType.Summon2:
                    {
                        yield return new SimpleEnemyMove(Intention.Spawn().WithMoveName(GetMove(0)), Summon1Action());
                        yield return new SimpleEnemyMove(Intention.PositiveEffect(), Summon2Buff());
                        Last = MoveType.Summon;
                        break;
                    }
                case MoveType.AttackMulti:
                    {
                        yield return AttackMove(GetMove(1), base.Gun1, base.Damage1, 3, isAccuracy: false, Gun1, withSpell: true);
                        Last = MoveType.AttackMulti;
                        break;
                    }
                case MoveType.AttackAccurate:
                    {
                        yield return AttackMove(GetMove(2), base.Gun2, base.Damage2, 1, isAccuracy: true, withSpell: true);
                        Last = MoveType.AttackAccurate;
                        break;
                    }
                case MoveType.AttackBuff:
                    {
                        yield return AttackMove(GetMove(3), base.Gun3, base.Damage3, 1, isAccuracy: false, withSpell: true);
                        // Change later to buff summons
                        yield return new SimpleEnemyMove(Intention.PositiveEffect(), BuffSummonsAction());
                        Last = MoveType.AttackBuff;
                        break;
                    }
                case MoveType.Defend:
                    {
                        yield return new SimpleEnemyMove(Intention.Defend().WithMoveName(GetMove(4)), KomachiDefend());
                        if (KomachiModDistanceSe.GetDistanceLevel(this) <= 3)
                        {
                            yield return new SimpleEnemyMove(Intention.Graze());
                        }
                        else
                        {
                            yield return new SimpleEnemyMove(Intention.Defend());
                        }
                        Last = MoveType.Defend;
                        break;
                    }
                case MoveType.Debuff:
                    {
                        yield return new SimpleEnemyMove(Intention.NegativeEffect().WithMoveName(GetMove(5)), KomachiDebuff());
                        yield return new SimpleEnemyMove(Intention.AddCard());
                        Last = MoveType.Debuff;
                        break;
                    }
                case MoveType.Spellcard:
                    {
                        yield return new SimpleEnemyMove(Intention.SpellCard(SpellcardName, Damage4, true), KomachiSpellcard());
                        Last = MoveType.Spellcard;
                        break;
                    }
                case MoveType.Escape:
                    {
                        yield return new SimpleEnemyMove(Intention.Escape(), EscapeActions());
                        Last = MoveType.Escape;
                        break;
                        var t = nameof(Long);
                    }
                case MoveType.Nothing:
                    {
                        yield return new SimpleEnemyMove(Intention.Charge());
                        Last = MoveType.Nothing;
                        break;
                    }
                case MoveType.LastWord:
                    {
                        yield return new SimpleEnemyMove(Intention.SpellCard(LastWordName, DamageLastWord, true), KomachiLastWord());
                        Last = MoveType.LastWord;
                        break;
                    }
            }
            if (Last == MoveType.Summon || Next == MoveType.Escape || Last == MoveType.LastWord) yield break;
            // Displacement shown to the player.
            int displaceKnowledge;
            // Actual displacement for the next move.
            int displaceActual = GetNextMoveDisplacement(NextNext);
            // If lunatic, the displacement is unknown. Otherwise, it's shown.
            if (GameRun.Difficulty == GameDifficulty.Lunatic)
            {
                displaceKnowledge = 0;
            }
            else displaceKnowledge = displaceActual;
            // Add the displacement intention to Komachi.
            yield return new SimpleEnemyMove(KomachiBossDisplaceIntention.Intention(displaceKnowledge), KomachiDisplace(displaceActual));
            yield break;
        }

        public int GetNextMoveDisplacement(MoveType next)
        {
            switch (next)
            {
                case MoveType.Summon2:
                    {
                        return 1;
                    }
                case MoveType.AttackMulti:
                    {
                        return -1;
                    }
                case MoveType.AttackAccurate:
                    {
                        return -2;
                    }
                case MoveType.AttackBuff:
                    {
                        return -1;
                    }
                case MoveType.Defend:
                    {
                        if (Difficulty == GameDifficulty.Lunatic) return 2;
                        return 1;
                    }
                case MoveType.Debuff:
                    {
                        return 1;
                    }
                case MoveType.Spellcard:
                    {
                        return -2;
                    }
                case MoveType.LastWord:
                    {
                        return -4;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }



        #region MOVES
        //Perform a custom action
        public int summonVacancy;
        IEnumerable<BattleAction> Summon1Action()
        {
            Debug.Log("Komachi sumon");
            yield return new EnemyMoveAction(this, GetMove(1));
            yield return PerformAction.Animation(this, "shoot3");

            // List to track enemies summoned. Used for displacing each of them if Komachi has any distance.
            List<EnemyUnit> unitsSpawned = new List<EnemyUnit>();
            
            // Spawns a vengeful Spirit if it's not in the battle.
            if (!Battle.AllAliveEnemies.Any(enemy => enemy.GetType() == typeof(Guihuo))) {
                var spawnVengefulSpirit = new SpawnEnemyAction(this, typeof(KomachiModVengefulSpiritEnemy), 0);
                yield return spawnVengefulSpirit;
                var vengefulSpirit = spawnVengefulSpirit.Args.Unit as KomachiModVengefulSpiritEnemy;
                unitsSpawned.Add(vengefulSpirit);
            }

            // Spawns a divine spirit if it's not in the battle. 
            // Sets its komachi to this. Important for the divine spirit itself.
            if (!Battle.AllAliveEnemies.Any(enemy => enemy.GetType() == typeof(Shenling)))
            {
                var spawnDivineSpirit = new SpawnEnemyAction(this, typeof(KomachiModDivineSpiritEnemy), 1);
                yield return spawnDivineSpirit;
                var divineSpirit = spawnDivineSpirit.Args.Unit as KomachiModDivineSpiritEnemy;

                unitsSpawned.Add(divineSpirit);
            }

            // Displaces each of them to be equal to komachi.
            if (TryGetStatusEffect<KomachiModDistanceSe>(out var bossDistance))
            {
                foreach(var enemy in unitsSpawned)
                {
                    yield return new ApplyStatusEffectAction<KomachiModDistanceSe>(enemy, bossDistance.Level);
                }
            }
            summonVacancy = 7;
        }

        IEnumerable<BattleAction> Summon2Buff()
        {
            yield return new ApplyStatusEffectAction<KomachiModBossGuidedEndTurnSe>(this, Count2);
        }

        // Defends with barrier and block if distance >= 3.
        // Defends with barrier and graze if distance < 2.
        IEnumerable<BattleAction> KomachiDefend()
        {
            yield return new EnemyMoveAction(this, base.GetMove(4));
            int distanceLevel = KomachiModDistanceSe.GetDistanceLevel(this);
            if (distanceLevel <= 3)
            {
                yield return new ApplyStatusEffectAction<Graze>(this, 2);
                yield return new CastBlockShieldAction(this, 0, Defend);
            }
            else
            {
                yield return new CastBlockShieldAction(this, 8, Defend);
            }
            foreach (var enemy in Battle.AllAliveEnemies)
            {
                if (!enemy.IsServant)
                {
                    yield return new ApplyStatusEffectAction<KomachiModDistanceBlockSe>(enemy, Count1);
                }
            }
        }

        IEnumerable<BattleAction> BuffSummonsAction()
        {
            if (Battle.AllAliveEnemies.Count() > 1)
            {
                foreach (var enemy in Battle.AllAliveEnemies)
                {
                    if (enemy != this)
                    {
                        yield return new ApplyStatusEffectAction<Firepower>(enemy, 1);
                    }
                }
            }
            else
            {
                yield return new ApplyStatusEffectAction<KomachiModBossGuidedEndTurnSe>(this, Count2, startAutoDecreasing: false);
            }
        }

        IEnumerable<BattleAction> KomachiDebuff()
        {
            yield return new EnemyMoveAction(this, base.GetMove(4));
            switch(KomachiModDistanceSe.GetDistanceLevel(this))
            {
                case 1:
                case 2:
                    yield return new ApplyStatusEffectAction<Weak>(Battle.Player, duration:2, startAutoDecreasing:false);
                    break;
                default:
                    yield return new ApplyStatusEffectAction<EnemyLockedOn>(this, 3, startAutoDecreasing:false);
                    yield return new ApplyStatusEffectAction<TempFirepowerNegative>(Battle.Player, 3);
                    break;
                case 4:
                case 5:
                    yield return new ApplyStatusEffectAction<EnemyVulnerable>(this, level: 2);
                    break;
            }
            var card = Library.CreateCard<KomachiModWeatheredLily>();
            yield return new AddCardsToDiscardAction(card);
        }

        IEnumerable<BattleAction> KomachiSpellcard()
        {
            yield return PerformAction.Spell(this, nameof(KomachiModUltFinalJudgement));
            foreach (var item in AttackActions(SpellcardName, Gun4, Damage4, isAccuracy: true))
            {
                yield return item;
            }
            yield return new ApplyStatusEffectAction<Firepower>(this, Count1);
        }

        IEnumerable<BattleAction> KomachiLastWord()
        {
            yield return PerformAction.Spell(this, nameof(KomachiModUltA));
            foreach (var item in AttackActions(LastWordName, GunLastWord, DamageLastWord, isAccuracy: true))
            {
                yield return item;
            }

            if (!Battle.Player.IsDead)
            {
                yield return new ForceKillAction(this, this);
            }
        }

        bool firstDisplacementHappened;
        IEnumerable<BattleAction> KomachiDisplace(int displaceAmount)
        {
            foreach(var enemy in Battle.AllAliveEnemies)
            {
                if (enemy == this || enemy.IsServant)
                {
                    yield return new DistanceChangeAction(enemy, displaceAmount)
                    {
                        Source = this
                    };
                }
            }
            if (!firstDisplacementHappened && !isSummon)
            {
                firstDisplacementHappened = true;
                yield return new ApplyStatusEffectAction<KomachiModBossDistanceGeneratorSe>(Battle.Player, 1);
            }
        }

        public string KomaEscape1 => LocalizeProperty("KomaEscape1");
        public string KomaEscape2 => LocalizeProperty("KomaEscape2");
        public IEnumerable<BattleAction> EscapeActions()
        {
            yield return PerformAction.Chat(this, KomaEscape1, 3f, 0f, 3.2f);
            yield return PerformAction.Chat(this, KomaEscape2, 3f, 0f, 3.2f);
            yield return new EscapeAction(this);
        }
        #endregion
        //Update choose the next attack.

        List<MoveType> fillerMoves = new List<MoveType>()
        {
            MoveType.AttackMulti,
            MoveType.AttackAccurate,
            MoveType.AttackBuff,
            MoveType.Defend,
            MoveType.Debuff
        };

        List<MoveType> fillerMovesUsed = new List<MoveType>();

        void ChooseNextMove(MoveType lastMove, out MoveType nextMove)
        {
            if (lastMove == MoveType.Nothing)
            {
                nextMove = MoveType.LastWord;
                return;
            }
            // If all summons are around. No decrementation.
            // If one summon is dead, decrement by 1.
            // If 2 summons are dead, decrement by 2.
            summonVacancy -= (3 - AllAliveEnemies.Count());

            // First move will always be summon. Second move will always be multihit.
            if (lastMove == MoveType.Summon)
            {
                nextMove = MoveType.AttackMulti;
                return;
            }

            // Get a random filler move. If there is no available move, do spellcard.
            if (fillerMoves.Contains(lastMove) || lastMove == MoveType.Summon2)
            {
                fillerMovesUsed.Add(lastMove);
                var availableMoves = fillerMoves.Except(fillerMovesUsed).ToList();

                // If all moves used in this cycle, trigger Spellcard
                if (availableMoves.Count == 0)
                {
                    nextMove = MoveType.Spellcard;
                    fillerMovesUsed.Clear(); // Reset for next cycle
                    return;
                }
                else
                {
                    // Pick random remaining move
                    // Change to use the game's own rng system later.
                    nextMove = availableMoves[EnemyBattleRng.NextInt(0, availableMoves.Count - 1)];
                }
            }
            else
            {
                nextMove = MoveType.AttackMulti;
            }

            // No matter what, unless the spellcard is the next move, get the summon2.
            if (summonVacancy <= 0 && lastMove != MoveType.Summon2)
            {
                nextMove = MoveType.Summon2;
                return;
            }
        }
        protected override void UpdateMoveCounters()
		{
            Next = NextNext;
            ChooseNextMove(Next, out NextNext);
            // REMEMBER TO REMOVE THIS THING WHEN RELEASING THE MOD
            Debug.Log($"Komachi has acted. Her next move (the one with her intention) move is {Next}.");
        }
    }
}