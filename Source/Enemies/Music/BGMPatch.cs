using HarmonyLib;
using KomachiMod.Enemies;
using LBoL.Core;
using LBoL.Core.Stations;
using LBoL.Presentation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KomachiMod.Source.Enemies.Music
{
    [HarmonyPatch(typeof(AudioManager), nameof(AudioManager.PlayBossBgm))]
    internal class AudioManager_PlayBossBgm_Patch_Komachi
    {
        private static bool Prefix(AudioManager __instance)
        {
            var station = Singleton<GameMaster>.Instance?.CurrentGameRun?.CurrentStation;
            if (station is BossStation bossStation)
            {
                if (bossStation.EnemyGroup?.Id == nameof(KomachiMod.Enemies.KomachiMod))
                {

                    BepinexPlugin.log.LogInfo($"Playing Komachi Boss BGM: KomachiBossBgm");
                    AudioManager.PlayInLayer1(nameof(KomachiModBossBgm));
                    return false; 
                }
            }

            return true;
        }
    }
}
