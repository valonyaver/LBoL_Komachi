using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using KomachiMod.Enemies.Template;
using KomachiMod.GunName;


namespace KomachiMod.Enemies
{
    public sealed class KomachiBossUnitDef : KomachiEnemyUnitTemplate
    {
        public override IdContainer GetId() => nameof(KomachiMod);

        public override EnemyUnitConfig MakeConfig()
        {
            EnemyUnitConfig config = GetEnemyUnitDefaultConfig();
            //Whether the boss should be enabled.
            config.IsPreludeOpponent = BepinexPlugin.enableAct1Boss.Value;
            config.Type = EnemyType.Boss;
            config.NarrativeColor = "#e58c27";

            //Color(s) of the exhibits the boss can drop (right-most exhibit).
            config.BaseManaColor = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };


            //Boss properties
            config.MaxHp = 250;
            config.MaxHpHard = 255;
            config.MaxHpLunatic = 260;

            // Multi attack
            config.Damage1 = 6;
            config.Damage1Hard = 7;
            config.Damage1Lunatic = 8;
            
            // Accurate attack
            config.Damage2 = 16;
            config.Damage2Hard = 16;
            config.Damage2Lunatic = 17;
            
            // Buff attack
            config.Damage3 = 20;
            config.Damage3Hard = 20;
            config.Damage3Lunatic = 22;

            // Spellcard damage
            config.Damage4 = 21;
            config.Damage4Hard = 23;            
            config.Damage4Lunatic = 25;

            // Barrier gained from the defend action
            config.Defend = 10;
            config.DefendHard = 12;
            config.DefendLunatic = 14;
            
            // Spellcard Firepower
            config.Count1 = 1;
            config.Count1Hard = 2;
            config.Count1Lunatic = 2;
            
            // Amount of guided spirits the boss gets
            config.Count2 = 4; 
            config.Count2Hard = 6;
            config.Count2Lunatic = 8;

            config.RealName = true;
            
            config.PowerLoot = new MinMax(100, 100);
            config.BluePointLoot = new MinMax(100, 100);

            config.Gun1 = new List<string> { GunNameID.GetGunFromId(7310) };
            config.Gun2 = new List<string> { GunNameID.GetGunFromId(6162) };
            config.Gun3 = new List<string> { GunNameID.GetGunFromId(7001) };
            config.Gun4 = new List<string> { GunNameID.GetGunFromId(4660) };

            return config;
        }
    }
}
