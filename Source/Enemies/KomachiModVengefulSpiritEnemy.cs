using KomachiMod.Enemies.Template;
using KomachiMod.GunName;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Normal.Guihuos;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;


namespace KomachiMod.Enemies
{
    public sealed class KomachiModVengefulSpiritEnemyDef : KomachiEnemyUnitTemplate
    {

        public override EnemyUnitConfig MakeConfig()
        {
            EnemyUnitConfig config = GetEnemyUnitDefaultConfig();

            //Color(s) of the exhibits the boss can drop (right-most exhibit).
            config.BaseManaColor = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };

            config.Type = EnemyType.Normal;
            config.ModleName = nameof(GuihuoBlue);
            config.IsPreludeOpponent = false;

            //Boss properties
            config.MaxHp = 23;
            config.MaxHpHard = 25;
            config.MaxHpLunatic = 27;

            config.MaxHpAdd = 0;

            config.Damage1 = 5;
            config.Damage1Hard = 6;
            config.Damage1Lunatic = 8;

            // Barrier gained from the defend action
            config.Defend = 8;
            config.DefendHard = 10;
            config.DefendLunatic = 12;
            
            // Detonation amount
            config.Count1 = 5;
            config.Count1Hard = 6;
            config.Count1Lunatic = 7;
            
            // Randomize how much higher it will be
            config.Count2 = 1;
            config.Count2Hard = 1;
            config.Count2Lunatic = 2;
            
            config.PowerLoot = new MinMax();
            config.BluePointLoot = new MinMax();

            config.Gun1 = new List<string> { "GuihuoB" };
            config.Gun2 = new List<string> { GunNameID.GetGunFromId(800) };
            config.Gun3 = new List<string> { GunNameID.GetGunFromId(800) };
            config.Gun4 = new List<string> { GunNameID.GetGunFromId(800) };

            return config;
        }

        [EntityLogic(typeof(KomachiModVengefulSpiritEnemyDef))]
        public sealed class KomachiModVengefulSpiritEnemy : Guihuo
        {
            protected override string SkillVFX => "GuihuoUskill";
            protected override Type DebuffType => typeof(Fragil);

            public KomachiMod komachi;

            public int detonationCount = 4;

            public override void OnSpawn(EnemyUnit spawner)
            {
                SetFirstTurn();
                React(PerformAction.Sfx("GhostSpawn"));
                React(new ApplyStatusEffectAction<DeathExplodeCount>(this, base.Count1, limit: detonationCount));
            }
        }
    }
}
