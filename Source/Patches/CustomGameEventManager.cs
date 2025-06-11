using HarmonyLib;
using LBoL.Core;
using KomachiMod.BattleActions;

namespace KomachiMod.Patches
{
    [HarmonyPatch]
    class CustomGameEventManager
    {
        static public GameEvent<BuffAttackEventArgs> PreCustomEvent { get; set;}
        static public GameEvent<BuffAttackEventArgs> PostCustomEvent { get; set; }

        public static GameEvent<DistanceChangedEventArgs> DistanceChanged { get; set; }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
        private static bool Prefix(GameRunController __instance)
        {
            PreCustomEvent = new GameEvent<BuffAttackEventArgs>();
            PostCustomEvent = new GameEvent<BuffAttackEventArgs>();
            DistanceChanged = new GameEvent<DistanceChangedEventArgs>();
            return true;
        }
    }
}