using HarmonyLib;
using LBoL.Core;
using KomachiMod.BattleActions;

namespace KomachiMod.Source.BattleActions.EventManager
{
    [HarmonyPatch]
    class KomachiEventsManager
    {
        static public GameEvent<BuffAttackEventArgs> PreCustomEvent { get; set;}
        static public GameEvent<BuffAttackEventArgs> PostCustomEvent { get; set; }

        public static GameEvent<DistanceChangedEventArgs> DistanceChanged { get; set; }
        public static GameEvent<ApplyVengefulSpiritEventArgs> AppliedVengefulSpirit;
        public static GameEvent<KomachiReleaseEventArgs> spiritsReleased;

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
        private static bool Prefix(GameRunController __instance)
        {
            PreCustomEvent = new GameEvent<BuffAttackEventArgs>();
            PostCustomEvent = new GameEvent<BuffAttackEventArgs>();
            DistanceChanged = new GameEvent<DistanceChangedEventArgs>();
            AppliedVengefulSpirit = new GameEvent<ApplyVengefulSpiritEventArgs>();
            spiritsReleased = new GameEvent<KomachiReleaseEventArgs>();
            return true;
        }
    }
}