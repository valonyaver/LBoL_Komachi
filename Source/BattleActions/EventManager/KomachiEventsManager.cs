using HarmonyLib;
using LBoL.Core;
using KomachiMod.BattleActions;

namespace KomachiMod.Source.BattleActions.EventManager
{
    [HarmonyPatch]
    class KomachiEventsManager
    {

        public static GameEvent<DistanceChangedEventArgs> DistanceChanged { get; set; }
        public static GameEvent<ApplyVengefulSpiritEventArgs> AppliedVengefulSpirit;
        public static GameEvent<KomachiReleaseEventArgs> spiritsReleasing;
        public static GameEvent<KomachiReleaseEventArgs> spiritsReleased;
        public static GameEvent<DetonateVengefulSpiritEventArgs> DetonatedSpirits;

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
        private static bool Prefix(GameRunController __instance)
        {
            DistanceChanged = new GameEvent<DistanceChangedEventArgs>();
            AppliedVengefulSpirit = new GameEvent<ApplyVengefulSpiritEventArgs>();
            spiritsReleasing = new GameEvent<KomachiReleaseEventArgs>();
            spiritsReleased = new GameEvent<KomachiReleaseEventArgs>();
            DetonatedSpirits = new GameEvent<DetonateVengefulSpiritEventArgs>();
            return true;
        }
    }
}